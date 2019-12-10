using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences.Editor {
	[CustomPropertyDrawer(typeof(ReferenceTypeAttribute))]
	public class PropertyReferenceDrawer : PropertyDrawer {

        private static readonly string FIELD_TARGET_OBJECT = "target";
        private static readonly string FIELD_FIELD_NAME = "fieldName";

        private class PropertyData {
	        public string property;
	        public SerializedProperty targetRefProperty;
	        public SerializedProperty propertyNameProperty;
        
	        public UnityEngine.Object targetObject;
	        public List<Component> allComponents;
	        public Dictionary<Type, List<Component>> componentsByType;
	        public string[] displayedCoomponents;
	        public Type selectedType;
	        public List<PropertyInfo> selectedTypeProperties = new List<PropertyInfo>();
	        public string[] propertiesByType;
        }

        private string _prevProperty;
        
        private Dictionary<string, PropertyData> _propertiesData = new Dictionary<string, PropertyData>();

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			
			if (!_propertiesData.ContainsKey(property.propertyPath)) {
				_propertiesData.AddOrSet(property.propertyPath, new PropertyData {
					property = property.propertyPath,
					targetRefProperty = property.FindPropertyRelative(FIELD_TARGET_OBJECT),
					propertyNameProperty = property.FindPropertyRelative(FIELD_FIELD_NAME),
				});
			}
			
			var data = _propertiesData[property.propertyPath];

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			
			var targetRefRect = new Rect(position.x, position.y, position.width, 16);
			
			EditorGUI.PropertyField(targetRefRect, data.targetRefProperty, GUIContent.none);

			if (data.targetRefProperty == null) {
				return;
			}

			var targetObject = data.targetRefProperty.objectReferenceValue;

			if (targetObject == null) {
				return;
			}
			
			var componentsPopupRect = new Rect(position.x, position.y + 20, position.width, 16);

			var targetObjectChanged = data.targetObject == null ||
			                          (_prevProperty == data.property && IsTargetChanged(targetObject, data));

			Component component;
			bool componentTypeChanged = false;
			if (GetObjectFromComponents(data, targetObject, targetObjectChanged, componentsPopupRect, out component, out componentTypeChanged)) {
				data.targetObject = component;
			} else {
				data.targetObject = targetObject;
				GUI.Label(componentsPopupRect, data.targetObject.GetType().Name);
			}

			data.targetRefProperty.objectReferenceValue = data.targetObject;

			if (targetObjectChanged || componentTypeChanged) {
				data.selectedTypeProperties = data.selectedType.GetPublicProperties().SelectNotNull().ToList();
	            
				if (data.selectedTypeProperties.Count == 0) {
					return;
				}

				var drawerAttribute = attribute as ReferenceTypeAttribute;

				if (drawerAttribute != null) {
					var list = data.selectedTypeProperties;
					data.selectedTypeProperties = new List<PropertyInfo>();
					data.selectedTypeProperties.AddRange(list.Where(p => p.PropertyType == drawerAttribute.type));
					if (drawerAttribute.type == typeof(string)) {
						data.selectedTypeProperties.AddRange(list.Where(p => p.PropertyType != drawerAttribute.type));
					}
					else {
						data.selectedTypeProperties.AddRange(list.Where(p => p.PropertyType.IsSubclassOf(drawerAttribute.type)));
						data.selectedTypeProperties.AddRange(list.Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == drawerAttribute.type));
					}
				
					if (drawerAttribute.type.IsSubclassOf(typeof(string))) {
						data.selectedTypeProperties.AddRange(list.Where(p => !p.PropertyType.IsSubclassOf(drawerAttribute.type)));
					}
				}
	            
				data.propertiesByType = data.selectedTypeProperties.Select(p => p.GetPropertyNameByType()).ToArray();
			}
			
			var selectedProperty = data.propertyNameProperty == null 
			                       || string.IsNullOrEmpty(data.propertyNameProperty.stringValue) 
			                       || data.selectedTypeProperties.Select(p => p.Name == data.propertyNameProperty.stringValue).Count() == 0
				? data.selectedTypeProperties.First() 
				: data.selectedTypeProperties.Where(f => f.Name == data.propertyNameProperty.stringValue).First();

			if (selectedProperty == null) {
				selectedProperty = data.selectedTypeProperties.First();
			}

			if (selectedProperty == null) {
				return;
			}

			var selectedFieldIndex = data.propertiesByType.IndexOf(selectedProperty.GetPropertyNameByType());

			var fieldPopupRect = new Rect(position.x, position.y + 40, position.width, 16);
			selectedFieldIndex = EditorGUI.Popup(fieldPopupRect, selectedFieldIndex, data.propertiesByType);
			data.propertyNameProperty.stringValue = data.selectedTypeProperties[selectedFieldIndex].Name;

			_prevProperty = data.property;

            EditorGUI.EndProperty();
		}

        private bool GetObjectFromComponents(PropertyData data, UnityEngine.Object targetObject, 
	        bool changed, Rect position, out Component component, out bool componentTypeChanged) {
	        component = null;
	        componentTypeChanged = false;

	        GameObject gameObject = null;

	        var currentComponent = targetObject as Component;

	        if (currentComponent != null) {
		        gameObject = currentComponent.gameObject;
	        } else {
		        gameObject = targetObject as GameObject;
	        }

	        if (gameObject == null) {
		        return false;
	        }
	        
	        if (changed) {
		        data.allComponents = gameObject.GetComponents<Component>().ToList();
	            
		        data.componentsByType = new Dictionary<System.Type, List<Component>>();
		        data.allComponents.ForEach(c => {
			        var type = c.GetType();
			        if (data.componentsByType.ContainsKey(type)) {
				        data.componentsByType[type].Add(c);
			        } else {
				        var componentsList = new List<Component>();
				        componentsList.Add(c);
				        data.componentsByType.Add(type, componentsList);
			        }
		        });

		        data.displayedCoomponents = data.allComponents.Select(c =>
			        $"{c.GetType().Name}[{data.componentsByType[c.GetType()].IndexOf(c)}]").ToArray();
	            
		        component = currentComponent == null ? data.allComponents.FirstOrDefault() : currentComponent;
	        } else {
		        component = currentComponent;
	        }
	        
	        if (data.allComponents.Count == 0) {
		        return false;
	        }

	        var selectedTypeChanged = false;
	        if (changed || (data.selectedType == null)) {
		        var savedType = component.GetType();
		        if (savedType != data.selectedType) {
			        data.selectedType = savedType;
		        }
	        }
            
	        if (data.selectedType == null || !data.componentsByType.ContainsKey(data.selectedType)) {
		        data.selectedType = data.allComponents.First().GetType();
	        }

	        var componentIndex = data.allComponents.IndexOf(component);

	        componentIndex = EditorGUI.Popup(position, componentIndex, 
		        data.displayedCoomponents);

	        componentIndex = componentIndex >= 0 && componentIndex < data.allComponents.Count ? componentIndex : 0;

	        component = data.allComponents[componentIndex];

	        var newSelectedType = component.GetType();

	        if (data.selectedType != newSelectedType) {
		        componentTypeChanged = true;
		        data.selectedType = newSelectedType;
	        }

	        data.targetObject = component;

	        return true;
        }

        private bool IsTargetChanged(UnityEngine.Object newObject, PropertyData data) {
	        if (newObject == null || data.targetObject == null) {
		        return true;
	        }

	        var newComponent = newObject as Component;
	        var targetComponent = data.targetObject as Component;

	        if (newComponent != null && targetComponent == null) {
		        return true;
	        }
	        
	        if (newComponent == null && targetComponent != null) {
		        return true;
	        }
	        
	        return newObject != data.targetObject;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
//            return _height;
	        return 60;
        }
    }
}
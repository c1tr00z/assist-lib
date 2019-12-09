using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences.Editor {
	[CustomPropertyDrawer(typeof(ReferenceTypeAttribute))]
	public class PropertyReferenceDrawer : PropertyDrawer {

        private static readonly string FIELD_TARGET_OBJECT = "target";
        private static readonly string FIELD_COMPONENT_TYPE = "targetComponentTypeName";
        private static readonly string FIELD_COMPONENT_INDEX = "componentIndex";
        private static readonly string FIELD_FIELD_NAME = "fieldName";

        private class PropertyData {
	        public string property;
	        public SerializedProperty targetRefProperty;
	        public SerializedProperty componentTypeProperty;
	        public SerializedProperty componentIndexProperty;
	        public SerializedProperty propertyNameProperty;
        
	        public GameObject targetGameObject;
	        public List<Component> allComponents;
	        public Dictionary<System.Type, List<Component>> componentsByType;
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
					componentTypeProperty = property.FindPropertyRelative(FIELD_COMPONENT_TYPE),
					componentIndexProperty = property.FindPropertyRelative(FIELD_COMPONENT_INDEX),
					propertyNameProperty = property.FindPropertyRelative(FIELD_FIELD_NAME),
				});
			}

			var data = _propertiesData[property.propertyPath];

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var targetRefRect = new Rect(position.x, position.y, position.width, 16);
            
            EditorGUI.PropertyField(targetRefRect, data.targetRefProperty, GUIContent.none);

            if (data.targetRefProperty.objectReferenceValue == null) {
                return;
            }

            var gameObject = data.targetRefProperty.objectReferenceValue as GameObject;

            if (gameObject == null) {
                return;
            }

            var gameObjectChanged = data.targetGameObject == null || (_prevProperty == data.property && data.targetGameObject != gameObject);

            if (gameObjectChanged) {
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
	            
	            data.targetGameObject = gameObject;
            }

            if (data.allComponents.Count == 0) {
                return;
            }

            var selectedTypeChanged = false;
            if (gameObjectChanged || (data.selectedType == null && data.componentTypeProperty != null && !string.IsNullOrEmpty(data.componentTypeProperty.stringValue))) {
	            var savedType = ReflectionUtils.GetTypeByName(data.componentTypeProperty.stringValue);
				if (savedType != data.selectedType) {
					data.selectedType = savedType;
					selectedTypeChanged = true;
				}
            }
            
            if (data.selectedType == null || !data.componentsByType.ContainsKey(data.selectedType)) {
	            data.selectedType = data.allComponents.First().GetType();
	            selectedTypeChanged = true;
            }

            var componentsPopupRect = new Rect(position.x, position.y + 20, position.width, 16);
            
			var selectedComponentIndex = data.componentIndexProperty.intValue;

            selectedComponentIndex = selectedComponentIndex < data.componentsByType[data.selectedType].Count ? selectedComponentIndex : 0;
			var selectedComponent = data.componentsByType[data.selectedType][selectedComponentIndex];
			var selectedTypeIndex = data.allComponents.IndexOf(selectedComponent);

			selectedTypeIndex = EditorGUI.Popup(componentsPopupRect, selectedTypeIndex, 
				data.displayedCoomponents);

			selectedComponent = data.allComponents[selectedTypeIndex];
			var newSelectedType = selectedComponent.GetType();

			if (data.selectedType != newSelectedType) {
				data.selectedType = newSelectedType;
				selectedTypeChanged = true;
			}
			
			selectedComponentIndex = data.componentsByType[data.selectedType].IndexOf(selectedComponent);

			data.componentTypeProperty.stringValue = data.selectedType.FullName;
			data.componentIndexProperty.intValue = selectedComponentIndex;

            if (selectedTypeChanged || data.selectedTypeProperties.Count == 0) {
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

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 60;
        }
    }
}
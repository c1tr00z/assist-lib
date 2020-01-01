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

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var targetRefRect = new Rect(position.x, position.y, position.width, 16);
            var targetRefProperty = property.FindPropertyRelative(FIELD_TARGET_OBJECT);
            EditorGUI.PropertyField(targetRefRect, targetRefProperty, GUIContent.none);

            if (targetRefProperty.objectReferenceValue == null) {
                return;
            }

            var gameObject = targetRefProperty.objectReferenceValue as GameObject;

            if (gameObject == null) {
                return;
            }

            var allComponents = gameObject.GetComponents<Component>();

            if (allComponents.Length == 0) {
                return;
            }

            System.Type selectedType = allComponents.First().GetType();
            var componentTypeProperty = (property.FindPropertyRelative(FIELD_COMPONENT_TYPE));
            if (componentTypeProperty != null && !string.IsNullOrEmpty(componentTypeProperty.stringValue)) {
				var savedType = ReflectionUtils.GetTypeByName(componentTypeProperty.stringValue);
				selectedType = savedType != null ? savedType : selectedType;
            }

            var componentsPopupRect = new Rect(position.x, position.y + 20, position.width, 16);
            var componentIndexProperty = property.FindPropertyRelative(FIELD_COMPONENT_INDEX);
			var selectedComponentIndex = componentIndexProperty.intValue;

			var componentsByType = new Dictionary<System.Type, List<Component>>();
            allComponents.ForEach(c => {
                var type = c.GetType();
                if (componentsByType.ContainsKey(type)) {
                    componentsByType[type].Add(c);
                } else {
                    var componentsList = new List<Component>();
                    componentsList.Add(c);
                    componentsByType.Add(type, componentsList);
                }
            });

            if (!componentsByType.ContainsKey(selectedType)) {
	            selectedType = allComponents.First().GetType();
            }

            selectedComponentIndex = selectedComponentIndex < componentsByType[selectedType].Count ? selectedComponentIndex : 0;
			var selectedComponent = componentsByType[selectedType][selectedComponentIndex];
			var selectedTypeIndex = allComponents.IndexOf(selectedComponent);

			selectedTypeIndex = EditorGUI.Popup(componentsPopupRect, selectedTypeIndex, 
                allComponents.Select(c => string.Format("{0}[{1}]", c.GetType().Name, componentsByType[c.GetType()].IndexOf(c))).ToArray());

			selectedComponent = allComponents[selectedTypeIndex];
            selectedType = selectedComponent.GetType();
			selectedComponentIndex = componentsByType[selectedType].IndexOf(selectedComponent);

			componentTypeProperty.stringValue = selectedType.FullName;
            componentIndexProperty.intValue = selectedComponentIndex;

            var allProperties = selectedType.GetPublicProperties().SelectNotNull().ToList();
            if (allProperties.Count == 0) {
                return;
            }

			var drawerAttribute = attribute as ReferenceTypeAttribute;

			if (drawerAttribute != null) {
				var list = allProperties;
				allProperties = new List<PropertyInfo>();
				allProperties.AddRange(list.Where(p => p.PropertyType == drawerAttribute.type));
				if (drawerAttribute.type == typeof(string)) {
					allProperties.AddRange(list.Where(p => p.PropertyType != drawerAttribute.type));
				}
				else {
					allProperties.AddRange(list.Where(p => p.PropertyType.IsSubclassOf(drawerAttribute.type)));
					allProperties.AddRange(list.Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == drawerAttribute.type));
				}
				
				if (drawerAttribute.type.IsSubclassOf(typeof(string))) {
					allProperties.AddRange(list.Where(p => !p.PropertyType.IsSubclassOf(drawerAttribute.type)));
				}
			}

			var propertyNameProperty = property.FindPropertyRelative(FIELD_FIELD_NAME);
            var selectedProperty = propertyNameProperty == null 
                || string.IsNullOrEmpty(propertyNameProperty.stringValue) 
                || allProperties.Select(p => p.Name == propertyNameProperty.stringValue).Count() == 0
                ? allProperties.First() 
                : allProperties.Where(f => f.Name == propertyNameProperty.stringValue).First();

			if (selectedProperty == null) {
				selectedProperty = allProperties.First();
			}

            if (selectedProperty == null) {
                return;
            }
			var propertiesByType = allProperties.Select(p => p.GetPropertyNameByType()).ToArray();

            var selectedFieldIndex = propertiesByType.IndexOf(selectedProperty.GetPropertyNameByType());

            var fieldPopupRect = new Rect(position.x, position.y + 40, position.width, 16);
            selectedFieldIndex = EditorGUI.Popup(fieldPopupRect, selectedFieldIndex, propertiesByType);
            propertyNameProperty.stringValue = allProperties[selectedFieldIndex].Name;

            EditorGUI.EndProperty();
		}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 60;
        }
    }
}
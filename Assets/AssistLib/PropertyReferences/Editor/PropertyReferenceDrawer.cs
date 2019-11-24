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

        private SerializedProperty _property;
        
        private GameObject _targetGameObject;
        private List<Component> _allComponents;
        private Dictionary<System.Type, List<Component>> _componentsByType;
        private string[] _displayedCoomponents;
        private Type _selectedType;
        private List<PropertyInfo> _selectedTypeProperties = new List<PropertyInfo>();

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

            var sameGameObject = _targetGameObject == gameObject;

            if (!sameGameObject) {
	            _allComponents = gameObject.GetComponents<Component>().ToList();
	            
	            _componentsByType = new Dictionary<System.Type, List<Component>>();
	            _allComponents.ForEach(c => {
		            var type = c.GetType();
		            if (_componentsByType.ContainsKey(type)) {
			            _componentsByType[type].Add(c);
		            } else {
			            var componentsList = new List<Component>();
			            componentsList.Add(c);
			            _componentsByType.Add(type, componentsList);
		            }
	            });

	            _displayedCoomponents = _allComponents.Select(c =>
		            $"{c.GetType().Name}[{_componentsByType[c.GetType()].IndexOf(c)}]").ToArray();
	            
	            _targetGameObject = gameObject;
            }

            if (_allComponents.Count == 0) {
                return;
            }
            
            var componentTypeProperty = (property.FindPropertyRelative(FIELD_COMPONENT_TYPE));
            if (componentTypeProperty != null && !string.IsNullOrEmpty(componentTypeProperty.stringValue)) {
				var savedType = ReflectionUtils.GetTypeByName(componentTypeProperty.stringValue);
				_selectedType = savedType != null ? savedType : _selectedType;
            }

            var componentsPopupRect = new Rect(position.x, position.y + 20, position.width, 16);
            var componentIndexProperty = property.FindPropertyRelative(FIELD_COMPONENT_INDEX);
			var selectedComponentIndex = componentIndexProperty.intValue;

            if (_selectedType == null || !_componentsByType.ContainsKey(_selectedType)) {
	            _selectedType = _allComponents.First().GetType();
            }

            selectedComponentIndex = selectedComponentIndex < _componentsByType[_selectedType].Count ? selectedComponentIndex : 0;
			var selectedComponent = _componentsByType[_selectedType][selectedComponentIndex];
			var selectedTypeIndex = _allComponents.IndexOf(selectedComponent);

			selectedTypeIndex = EditorGUI.Popup(componentsPopupRect, selectedTypeIndex, 
				_displayedCoomponents);

			selectedComponent = _allComponents[selectedTypeIndex];
			var newSelectedType = selectedComponent.GetType();
			var componentChanged = newSelectedType != _selectedType;
			_selectedType = newSelectedType;
			selectedComponentIndex = _componentsByType[_selectedType].IndexOf(selectedComponent);

			componentTypeProperty.stringValue = _selectedType.FullName;
            componentIndexProperty.intValue = selectedComponentIndex;

            if (componentChanged || _selectedTypeProperties.Count == 0) {
	            _selectedTypeProperties = _selectedType.GetPublicProperties().SelectNotNull().ToList();
	            
	            if (_selectedTypeProperties.Count == 0) {
		            return;
	            }

	            var drawerAttribute = attribute as ReferenceTypeAttribute;

	            if (drawerAttribute != null) {
		            var list = _selectedTypeProperties;
		            _selectedTypeProperties = new List<PropertyInfo>();
		            _selectedTypeProperties.AddRange(list.Where(p => p.PropertyType == drawerAttribute.type));
		            if (drawerAttribute.type == typeof(string)) {
			            _selectedTypeProperties.AddRange(list.Where(p => p.PropertyType != drawerAttribute.type));
		            }
		            else {
			            _selectedTypeProperties.AddRange(list.Where(p => p.PropertyType.IsSubclassOf(drawerAttribute.type)));
			            _selectedTypeProperties.AddRange(list.Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == drawerAttribute.type));
		            }
				
		            if (drawerAttribute.type.IsSubclassOf(typeof(string))) {
			            _selectedTypeProperties.AddRange(list.Where(p => !p.PropertyType.IsSubclassOf(drawerAttribute.type)));
		            }
	            }
	            
	            _propertiesByType = _selectedTypeProperties.Select(p => p.GetPropertyNameByType()).ToArray();
            }

			var propertyNameProperty = property.FindPropertyRelative(FIELD_FIELD_NAME);
            var selectedProperty = propertyNameProperty == null 
                || string.IsNullOrEmpty(propertyNameProperty.stringValue) 
                || _selectedTypeProperties.Select(p => p.Name == propertyNameProperty.stringValue).Count() == 0
                ? _selectedTypeProperties.First() 
                : _selectedTypeProperties.Where(f => f.Name == propertyNameProperty.stringValue).First();

			if (selectedProperty == null) {
				selectedProperty = _selectedTypeProperties.First();
			}

            if (selectedProperty == null) {
                return;
            }

            var selectedFieldIndex = _propertiesByType.IndexOf(selectedProperty.GetPropertyNameByType());

            var fieldPopupRect = new Rect(position.x, position.y + 40, position.width, 16);
            selectedFieldIndex = EditorGUI.Popup(fieldPopupRect, selectedFieldIndex, _propertiesByType);
            propertyNameProperty.stringValue = _selectedTypeProperties[selectedFieldIndex].Name;

            EditorGUI.EndProperty();
		}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 60;
        }
    }
}
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
        private SerializedProperty _targetRefProperty;
        private SerializedProperty _componentTypeProperty;
        private SerializedProperty _componentIndexProperty;
        private SerializedProperty _propertyNameProperty;
        
        private GameObject _targetGameObject;
        private List<Component> _allComponents;
        private Dictionary<System.Type, List<Component>> _componentsByType;
        private string[] _displayedCoomponents;
        private Type _selectedType;
        private List<PropertyInfo> _selectedTypeProperties = new List<PropertyInfo>();
        private string[] _propertiesByType;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			var propertyChanged = false;
			if (_property != property) {
				propertyChanged = true;
				_property = property;
			}

			if (propertyChanged) {
				_targetRefProperty = property.FindPropertyRelative(FIELD_TARGET_OBJECT);
				_componentTypeProperty = property.FindPropertyRelative(FIELD_COMPONENT_TYPE);
				_componentIndexProperty = property.FindPropertyRelative(FIELD_COMPONENT_INDEX);
				_propertyNameProperty = property.FindPropertyRelative(FIELD_FIELD_NAME);
			}

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var targetRefRect = new Rect(position.x, position.y, position.width, 16);
            
            EditorGUI.PropertyField(targetRefRect, _targetRefProperty, GUIContent.none);

            if (_targetRefProperty.objectReferenceValue == null) {
                return;
            }

            var gameObject = _targetRefProperty.objectReferenceValue as GameObject;

            if (gameObject == null) {
                return;
            }

            var gameObjectChanged = _targetGameObject != gameObject;

            if (gameObjectChanged) {
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

            var selectedTypeChanged = false;
            if (gameObjectChanged || (_selectedType == null && _componentTypeProperty != null && !string.IsNullOrEmpty(_componentTypeProperty.stringValue))) {
	            var savedType = ReflectionUtils.GetTypeByName(_componentTypeProperty.stringValue);
				if (savedType != _selectedType) {
					_selectedType = savedType;
					selectedTypeChanged = true;
				}
            }
            
            if (_selectedType == null || !_componentsByType.ContainsKey(_selectedType)) {
	            _selectedType = _allComponents.First().GetType();
	            selectedTypeChanged = true;
            }

            var componentsPopupRect = new Rect(position.x, position.y + 20, position.width, 16);
            
			var selectedComponentIndex = _componentIndexProperty.intValue;

            selectedComponentIndex = selectedComponentIndex < _componentsByType[_selectedType].Count ? selectedComponentIndex : 0;
			var selectedComponent = _componentsByType[_selectedType][selectedComponentIndex];
			var selectedTypeIndex = _allComponents.IndexOf(selectedComponent);

			selectedTypeIndex = EditorGUI.Popup(componentsPopupRect, selectedTypeIndex, 
				_displayedCoomponents);

			selectedComponent = _allComponents[selectedTypeIndex];
			var newSelectedType = selectedComponent.GetType();

			if (_selectedType != newSelectedType) {
				_selectedType = newSelectedType;
				selectedTypeChanged = true;
			}
			
			selectedComponentIndex = _componentsByType[_selectedType].IndexOf(selectedComponent);

			_componentTypeProperty.stringValue = _selectedType.FullName;
            _componentIndexProperty.intValue = selectedComponentIndex;

            if (selectedTypeChanged || _selectedTypeProperties.Count == 0) {
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
			
            var selectedProperty = _propertyNameProperty == null 
                || string.IsNullOrEmpty(_propertyNameProperty.stringValue) 
                || _selectedTypeProperties.Select(p => p.Name == _propertyNameProperty.stringValue).Count() == 0
                ? _selectedTypeProperties.First() 
                : _selectedTypeProperties.Where(f => f.Name == _propertyNameProperty.stringValue).First();

			if (selectedProperty == null) {
				selectedProperty = _selectedTypeProperties.First();
			}

            if (selectedProperty == null) {
                return;
            }

            var selectedFieldIndex = _propertiesByType.IndexOf(selectedProperty.GetPropertyNameByType());

            var fieldPopupRect = new Rect(position.x, position.y + 40, position.width, 16);
            selectedFieldIndex = EditorGUI.Popup(fieldPopupRect, selectedFieldIndex, _propertiesByType);
            _propertyNameProperty.stringValue = _selectedTypeProperties[selectedFieldIndex].Name;

            EditorGUI.EndProperty();
		}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 60;
        }
    }
}
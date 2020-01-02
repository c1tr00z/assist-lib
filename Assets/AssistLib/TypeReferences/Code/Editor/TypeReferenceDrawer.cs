using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.TypeReferences;
using UnityEditor;
using UnityEngine;

namespace AssistLib.TypeReferences.Editor {
    [CustomPropertyDrawer(typeof(BaseTypeAttribute))]
    public class TypeReferenceDrawer : PropertyDrawer {
        
        private static readonly string FIELD_TYPE_FULL_NAME = "typeFullName";

        private class PropertyData {
            public string path;
            public List<Type> types;
            public List<string> typesNames;
            public SerializedProperty typeFullNameProperty;
            public Type currentType;
        }
        
        private Dictionary<string, PropertyData> _properties = new Dictionary<string, PropertyData>();
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var propertyData = GetPropertyData(property, attribute);

            var currentTypeName = propertyData.typeFullNameProperty.stringValue;
            currentTypeName = string.IsNullOrEmpty(currentTypeName) ? propertyData.typesNames.FirstOrDefault() : currentTypeName;
            if (currentTypeName != propertyData.currentType.FullName || propertyData.currentType == null) {
                propertyData.currentType = propertyData.types.FirstOrDefault(t => t.FullName == currentTypeName);
            }
            var selectedIndex = propertyData.types.IndexOf(propertyData.currentType);
            selectedIndex = selectedIndex >= 0 ? selectedIndex < propertyData.types.Count ? selectedIndex : 0 : 0;
            selectedIndex = EditorGUI.Popup(position, selectedIndex, propertyData.typesNames.ToArray());
            selectedIndex = selectedIndex >= 0 ? selectedIndex < propertyData.types.Count ? selectedIndex : 0 : 0;
            var selectedType = propertyData.types[selectedIndex];

            if (selectedType != propertyData.currentType) {
                propertyData.typeFullNameProperty.stringValue = selectedType.FullName;
            }
            
            EditorGUI.EndProperty();
        }

        private PropertyData GetPropertyData(SerializedProperty property, PropertyAttribute attribute) {
            var path = property.propertyPath;
            if (_properties.ContainsKey(path)) {
                return _properties[path];
            }
            
            var baseTypeAttribute = attribute as BaseTypeAttribute;
            var baseClass = baseTypeAttribute != null 
                ? baseTypeAttribute.type != null 
                    ? baseTypeAttribute.type 
                    : typeof(object) 
                : typeof(object);

            var typesList = ReflectionUtils.GetSubclassesOf(baseClass).ToList();
            if (typesList.Count == 0) {
                if (!baseClass.IsAbstract) {
                    typesList.Add(baseClass);
                } else {
                    Debug.LogError("Only abstract classes or no classes in collection");
                }
            }
            var typesNames = typesList.Select(t => t.FullName.Replace(".", "/")).ToList();

            var typeFullNameProperty = property.FindPropertyRelative(FIELD_TYPE_FULL_NAME);
            
            var currentTypeName = typeFullNameProperty.stringValue;
            var currentType = !string.IsNullOrEmpty(currentTypeName) ? ReflectionUtils.GetTypeByName(currentTypeName) : typesList.FirstOrDefault();
            
            var propertyData = new PropertyData {
                path = path, 
                types = typesList, 
                typesNames = typesNames,
                typeFullNameProperty = typeFullNameProperty,
                currentType = currentType
            };
            
            _properties.Add(path, propertyData);

            return propertyData;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 20;
        }
    }
}
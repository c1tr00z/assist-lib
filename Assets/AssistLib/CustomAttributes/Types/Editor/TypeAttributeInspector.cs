using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeAttribute))]
public class TypeAttributeInspector : ExtendedPropertyDrawer {

    private Type[] _selectedTypes;

    private TypeAttribute typeAttribute {
        get { return attribute as TypeAttribute; }
    }
    
    public TypeAttributeInspector() {
        
    }

    public override void Show(SerializedProperty property) {

        if (typeAttribute == null) {
            GUI.Label(position, new GUIContent("No attribute"));
            return;
        }

        if (_selectedTypes == null || _selectedTypes.Length == 0) {
            _selectedTypes = ReflectionUtils.GetSubclassesOf(typeAttribute.baseType);
        }

        //if (EditorGUI.DropdownButton(position, new GUIContent("123"), FocusType.Keyboard)) {
            //PopupWindow.Show(position, )
            //var bridge = new PopupBridge(callback, types);
            //bridge.selectedType = selected;
            //EditorListPopup<Type>.ShowPopup(CalculateRect(rect, types), bridge);
        //}// .Popup( (0, _selectedTypes.Select(t => t.FullName).ToArray());
    }
}

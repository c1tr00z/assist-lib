using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ExtendedPropertyDrawer : PropertyDrawer {

    protected Rect position;
    protected SerializedProperty _currentProperty;

    public void AddX(float width) {
        position.xMin += width;
    }

    public float width {
        get { return position.width; }
    }

    public float propertyNameWidth {
        get { return EditorGUIUtils.GetDisplayNameFieldWidth(width); }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        //base.OnGUI(position, property, label);
        _currentProperty = property;
        this.position = position;
        DrawPropertyName();
        //this.position.y += 5;
        AddX(EditorGUIUtils.GetDisplayNameFieldWidth(width));
        Show(property);
        _currentProperty = null;
    }

    public abstract void Show(SerializedProperty property);

    protected void DrawPropertyName() {
        if (_currentProperty == null) {
            return;
        }
        EditorGUI.PrefixLabel(position, new GUIContent(_currentProperty.displayName));
    }
}

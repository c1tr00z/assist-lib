using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace c1tr00z.AssistLib.PropertyReferences.Editor {
	[CustomPropertyDrawer(typeof(PropertyReference))]
	public class PropertyReferenceDrawer : PropertyDrawer {

		//public override VisualElement CreatePropertyGUI (SerializedProperty property) {
		//	var container = new VisualElement();

		//	// Create property fields.
		//	var reference = new PropertyField(property.FindPropertyRelative("reference"));
		//	//var amountField = new PropertyField(property.FindPropertyRelative("amount"));
		//	//var unitField = new PropertyField(property.FindPropertyRelative("unit"));
		//	//var nameField = new PropertyField(property.FindPropertyRelative("name"), "Fancy Name");

		//	// Add fields to the container.
		//	container.Add(reference);

		//	return container;
		//}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			//base.OnGUI(position, property, label);
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			//EditorGUILayout.PropertyField(property.FindPropertyRelative("target"));		

			//var targetRect = new Rect(position.x, position.y, position.width, position.height);

			//EditorGUI.PropertyField(referenceRect, property.FindPropertyRelative("reference"));

			EditorGUI.EndProperty();
		}
	}
}
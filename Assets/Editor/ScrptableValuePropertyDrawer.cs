using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(ScriptableValueField<>))]
public class ScrptableValuePropertyDrawer : PropertyDrawer
{
    private const string ScriptableFieldName = "scriptableValue";
    private const string PropertyFieldName = "currentValue";
    private const float Spacing = 2;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var scriptableProperty = property.FindPropertyRelative(ScriptableFieldName);
        var height = EditorGUI.GetPropertyHeight(scriptableProperty);
        if (scriptableProperty.objectReferenceValue != null)
            height += Spacing * 2 + EditorGUI.GetPropertyHeight(new SerializedObject(scriptableProperty.objectReferenceValue).FindProperty(PropertyFieldName));
        return height;
            
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var l = label.text;
        EditorGUI.BeginProperty(position, label, property);
        var scriptableObjectProperty = property.FindPropertyRelative(ScriptableFieldName);
        if (scriptableObjectProperty.objectReferenceValue == null)
        {
            EditorGUI.PropertyField(position, scriptableObjectProperty, new GUIContent(l));
            EditorGUI.EndProperty();
            return;
        }

        var refHeight = EditorGUI.GetPropertyHeight(scriptableObjectProperty);
        var base_rect = new Rect(position.x, position.y, position.width, refHeight);
        var parameter_rect = new Rect(position.x, base_rect.yMax + Spacing, position.width, position.height - refHeight - Spacing);
        var serializedObj = new SerializedObject(scriptableObjectProperty.objectReferenceValue);
        var parameter = serializedObj.FindProperty(PropertyFieldName);

        EditorGUI.PropertyField(base_rect, scriptableObjectProperty, new GUIContent(l));
        var bellow_label = new StringBuilder();
        for (var i = 0; i < label.text.Length; i++)
        {
            bellow_label.Append(" ");
        }
        EditorGUI.PropertyField(parameter_rect, parameter, new GUIContent(bellow_label.ToString()));
        serializedObj.ApplyModifiedProperties();
        EditorGUI.EndProperty();
        
    }
}

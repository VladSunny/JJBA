using UnityEditor;
using UnityEngine;

// Custom attribute to mark fields as see-only
public class SeeOnlyAttribute : PropertyAttribute {}

#if UNITY_EDITOR
// Custom property drawer for fields marked with SeeOnlyAttribute
[UnityEditor.CustomPropertyDrawer(typeof(SeeOnlyAttribute))]
public class SeeOnlyDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
    {
        // Display the field's value without any interaction
        EditorGUI.BeginDisabledGroup(true);
        UnityEditor.EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
    }
}
#endif
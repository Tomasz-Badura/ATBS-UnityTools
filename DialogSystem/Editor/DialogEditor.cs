using UnityEditor;
using UnityEngine;
using ATBS.DialogSystem;
[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor
{
    SerializedProperty isDialogOption;
    SerializedProperty dialogOptions;
    SerializedProperty nextDialog;
    SerializedProperty hasEndConversation;

    void OnEnable()
    {
        isDialogOption = serializedObject.FindProperty("isDialogOption");
        dialogOptions = serializedObject.FindProperty("dialogOptions");
        nextDialog = serializedObject.FindProperty("nextDialog");
        hasEndConversation = serializedObject.FindProperty("hasEndConversation");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        if (isDialogOption.boolValue)
        {
            EditorGUILayout.PropertyField(dialogOptions, true);
            EditorGUILayout.PropertyField(hasEndConversation, true);
        }
        else
        {
            EditorGUILayout.PropertyField(nextDialog, true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
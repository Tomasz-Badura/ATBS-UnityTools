using UnityEditor;
namespace ATBS.MenuSystem
{
    [CustomEditor(typeof(ValueCheck))]
    public class ValueCheckEditor : UnityEditor.Editor
    {
        SerializedProperty validationCode;
        SerializedProperty expectValue;
        SerializedProperty expectedFloatValue;
        SerializedProperty expectedIntValue;
        SerializedProperty precisionLeeway;
        SerializedProperty parseType;
        SerializedProperty minMax;
        private void OnEnable() 
        {
            expectValue = serializedObject.FindProperty("expectValue");
            expectedFloatValue = serializedObject.FindProperty("expectedFloatValue");
            expectedIntValue = serializedObject.FindProperty("expectedIntValue");
            precisionLeeway = serializedObject.FindProperty("precisionLeeway");
            parseType = serializedObject.FindProperty("parseType");
            minMax = serializedObject.FindProperty("minMax");
            validationCode = serializedObject.FindProperty("validationCode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(validationCode, true);
            EditorGUILayout.PropertyField(minMax, true);
            EditorGUILayout.PropertyField(parseType, true);
            EditorGUILayout.PropertyField(expectValue, true);
            if(expectValue.boolValue)
            {
                if(parseType.enumValueIndex == 0)
                {
                    EditorGUILayout.PropertyField(expectedFloatValue, true);
                    EditorGUILayout.PropertyField(precisionLeeway, true);
                }
                else
                {
                    EditorGUILayout.PropertyField(expectedIntValue, true);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
using UnityEditor;
using UnityEngine;
using System;

namespace ATBS.MenuSystem
{
    [CustomEditor(typeof(InputWrapper))]
    public class InputWrapperEditor : Editor
    {
        #region variables
        bool useMinValue, UseMaxValue, UseMinLength, UseMaxLength;
        #endregion
        #region SerializedProperties
        SerializedProperty alphabet; // bool
        SerializedProperty digits; // bool
        SerializedProperty specialCharacters; // bool
        SerializedProperty unicode; // bool
        SerializedProperty canBeEmpty; // bool
        SerializedProperty canHaveSpace; // bool

        SerializedProperty capitalization; // bool
        SerializedProperty allCaps; // bool

        SerializedProperty checkLength; // bool
        SerializedProperty minLength; // int
        SerializedProperty maxLength; // int

        SerializedProperty mustBeOn; // bool

        SerializedProperty minValue; // float
        SerializedProperty maxValue; // float

        SerializedProperty _id; // string
        SerializedProperty _type; // InputType
        #endregion
        #region methods
        private void OnEnable()
        {
            canHaveSpace = serializedObject.FindProperty("canHaveSpace");
            unicode = serializedObject.FindProperty("unicode");
            minValue = serializedObject.FindProperty("minValue");
            maxValue = serializedObject.FindProperty("maxValue");
            mustBeOn = serializedObject.FindProperty("mustBeOn");
            _type = serializedObject.FindProperty("_type");
            _id = serializedObject.FindProperty("_id");
            alphabet = serializedObject.FindProperty("alphabet");
            digits = serializedObject.FindProperty("digits");
            specialCharacters = serializedObject.FindProperty("specialCharacters");
            canBeEmpty = serializedObject.FindProperty("canBeEmpty");
            capitalization = serializedObject.FindProperty("capitalization");
            allCaps = serializedObject.FindProperty("allCaps");
            checkLength = serializedObject.FindProperty("checkLength");
            minLength = serializedObject.FindProperty("minLength");
            maxLength = serializedObject.FindProperty("maxLength");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // gui styles
            GUIStyle boldLabelStyle = new GUIStyle(EditorStyles.label);
            boldLabelStyle.fontStyle = FontStyle.Bold;
            // Input data
            EditorGUILayout.LabelField("Input Data", boldLabelStyle);
            EditorGUILayout.PropertyField(_id);
            EditorGUILayout.PropertyField(_type);
            EditorGUILayout.Space(5);
            // Input validation
            EditorGUILayout.LabelField("Input validation", boldLabelStyle);

            InputType inputType = (InputType)_type.enumValueIndex;
            switch (inputType)
            {
                // Define here your own input types editor
                case InputType.Toggle:
                    EditorGUILayout.PropertyField(mustBeOn);
                    break;
                case InputType.Dropdown:
                    // No validation for a dropdown, might be added in future updates
                    break;
                case InputType.Slider:
                    NumberValidation();
                    break;
                case InputType.InputField:
                    TextValidation();
                    break;
                default:
                    //Custom editor hasn't been defined for this input type, showing all
                    base.OnInspectorGUI();
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void TextValidation()
        {
            EditorGUILayout.PropertyField(alphabet, new GUIContent("Can't contain alphabet."));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(digits, new GUIContent("Can't contain digits."));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(specialCharacters, new GUIContent("Can't contain special characters?"));
            if(specialCharacters.boolValue)
            {
                EditorGUILayout.PropertyField(unicode, new GUIContent("Check for unicode?"));
            }
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(canBeEmpty, new GUIContent("Can be empty?"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(canHaveSpace, new GUIContent("Can contain spaces?"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(capitalization, new GUIContent("Check capitalization?"));
            if (capitalization.boolValue)
            {
                EditorGUILayout.PropertyField(allCaps, new GUIContent("Capitalization method", "Uppercase - true, lowercase - false"));
            }
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(checkLength, new GUIContent("Check the length?"));
            if (checkLength.boolValue)
            {
                if (GUILayout.Button("Minimum length"))
                {
                    UseMinLength = !UseMinLength;
                    minLength.intValue = 0;
                }
                if (UseMinLength)
                {
                    minLength.intValue = Mathf.Max(int.MinValue, Mathf.Min(int.MaxValue, minLength.intValue));
                    EditorGUILayout.PropertyField(minLength, new GUIContent("Minimum length"));
                }
                else
                {
                    minLength.intValue = int.MinValue;
                }

                EditorGUILayout.Space(2);

                if (GUILayout.Button("Maximum length"))
                {
                    maxLength.intValue = 0;
                    UseMaxLength = !UseMaxLength;
                }
                if (UseMaxLength)
                {
                    maxLength.intValue = Mathf.Max(int.MinValue, Mathf.Min(int.MaxValue, maxLength.intValue));
                    EditorGUILayout.PropertyField(maxLength, new GUIContent("Maximum length"));
                }
                else
                {
                    maxLength.intValue = int.MaxValue;
                }
            }
        }

        private void NumberValidation()
        {
            if(useMinValue || UseMaxValue)
            {
                EditorGUILayout.Space(45);
                GUI.Label(new Rect(10,60,300,100)," Number validation will always\n be rounded to allow for inclusive\n range checks");
            }
            if (GUILayout.Button("Minimum value"))
            {
                minValue.floatValue = 0;
                useMinValue = !useMinValue;
            }
            if (useMinValue)
            {
                minValue.floatValue = Convert.ToInt32(Mathf.Max(int.MinValue, Mathf.Min(int.MaxValue, minValue.floatValue)));
                EditorGUILayout.PropertyField(minValue, new GUIContent("Minimum value"));
            }
            else
            {
                minValue.floatValue = float.MinValue;
            }

            EditorGUILayout.Space(2);

            if (GUILayout.Button("Maximum value"))
            {
                maxValue.floatValue = 0;
                UseMaxValue = !UseMaxValue;
            }
            if (UseMaxValue)
            {
                maxValue.floatValue = Convert.ToInt32(Mathf.Max(int.MinValue, Mathf.Min(int.MaxValue, maxValue.floatValue)));
                EditorGUILayout.PropertyField(maxValue, new GUIContent("Maximum value"));
            }
            else
            {
                maxValue.floatValue = float.MaxValue;
            }
        }
        #endregion
    }
}
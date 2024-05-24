//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(Movement2D))]
//public class InspectorEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        Movement2D movement2D = (Movement2D)target;
//        GUIStyle customHeaderStyle = movement2D.customHeaderStyle;

//        EditorGUILayout.LabelField("Speed Values", customHeaderStyle);
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("movementSpeed"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("speedUpAccelaration"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("speedDownAccelaration"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("stopAccelaration"));

//        EditorGUILayout.LabelField("Dash", customHeaderStyle);
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("dashButton"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("Dash"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("dashDistance"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("dashDuration"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("dashAccMult"));
        
//        //movement2D.movementSpeed = EditorGUILayout.Slider("Movement Speed", movement2D.movementSpeed, 1f, 15f);


//        serializedObject.ApplyModifiedProperties();
//        base.OnInspectorGUI();
//    }
//}

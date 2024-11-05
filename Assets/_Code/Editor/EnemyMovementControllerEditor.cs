using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyMovementController))]
public class EnemyMovementControllerEditor : Editor
{

    #region Serialize Property
    SerializedProperty movementMode; 
    SerializedProperty movementSpeed; 
    SerializedProperty reloadTime; 
    SerializedProperty rushDistance; 
    SerializedProperty rushTime;
    #endregion Serialize Property

    private void OnEnable()
    {
        movementMode = serializedObject.FindProperty("movementMode");
        movementSpeed = serializedObject.FindProperty("movementSpeed");
        reloadTime = serializedObject.FindProperty("reloadTime");
        rushDistance = serializedObject.FindProperty("rushDistance");
        rushTime = serializedObject.FindProperty("rushTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EnemyMovementController movementController = (EnemyMovementController)target;

        EditorGUILayout.PropertyField(movementMode);
        if (movementController.movementMode == EnemyMovementController.MovementMode.MoveRandom)
        {
            EditorGUILayout.PropertyField(movementSpeed);
        } else if (movementController.movementMode == EnemyMovementController.MovementMode.RushToward)
        {
            EditorGUILayout.PropertyField(reloadTime);
            EditorGUILayout.PropertyField(rushDistance);
            EditorGUILayout.PropertyField(rushTime);
        }

        serializedObject.ApplyModifiedProperties();
    }

}

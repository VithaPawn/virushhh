using UnityEditor;

[CustomEditor(typeof(BulletModeSM))]
public class BulletModeSMEditor : Editor {

    #region SerializeProperty
    SerializedProperty bulletPrefab;
    SerializedProperty reloadTime;
    SerializedProperty shootingDirectionNumber;
    SerializedProperty shootingMode;
    SerializedProperty burstTime;
    SerializedProperty bulletNumberEachBurst;
    #endregion SerializeProperty

    private void OnEnable()
    {
        bulletPrefab = serializedObject.FindProperty("bulletPrefab");
        reloadTime = serializedObject.FindProperty("reloadTime");
        shootingDirectionNumber = serializedObject.FindProperty("shootingDirectionNumber");
        shootingMode = serializedObject.FindProperty("shootingMode");
        burstTime = serializedObject.FindProperty("burstTime");
        bulletNumberEachBurst = serializedObject.FindProperty("bulletNumberEachBurst");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BulletModeSM bulletMode = (BulletModeSM)target;

        EditorGUILayout.PropertyField(bulletPrefab);
        EditorGUILayout.PropertyField(reloadTime);
        EditorGUILayout.PropertyField(shootingDirectionNumber);
        EditorGUILayout.PropertyField(shootingMode);
        if (bulletMode.shootingMode == BulletModeSM.ShootingMode.Burst)
        {
            EditorGUILayout.PropertyField(burstTime);
            EditorGUILayout.PropertyField(bulletNumberEachBurst);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

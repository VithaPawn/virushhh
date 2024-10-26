using UnityEngine;


[CreateAssetMenu(fileName = "Bullet", menuName = "ScriptableObjects/Attack/Bullet")]
public class BulletSO : ScriptableObject {
    [SerializeField] private float existTime;
    [SerializeField] private float flyingForce;

    public float GetExistTime() { return existTime; }
    public float GetFlyingForce() { return flyingForce; }
}

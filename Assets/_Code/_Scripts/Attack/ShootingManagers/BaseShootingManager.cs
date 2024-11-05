using UnityEngine;

public abstract class BaseShootingManager : MonoBehaviour {
    [Header("Ability To Do Something")]
    [SerializeField] protected bool canShoot;
    [Header("Shooting Attributes")]
    [SerializeField] protected float reloadTime;
    

    protected abstract void Shoot();
}

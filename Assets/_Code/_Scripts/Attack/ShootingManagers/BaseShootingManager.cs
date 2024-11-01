using UnityEngine;

public abstract class BaseShootingManager : MonoBehaviour {
    [Header("Shooting Attributes")]
    [SerializeField] protected float reloadTime;
    
    protected bool canShoot;

    protected abstract void Shoot();
}

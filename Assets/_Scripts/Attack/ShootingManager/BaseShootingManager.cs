using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseShootingManager : MonoBehaviour {
    [Header("Bullet Attributes")]
    [SerializeField] protected BaseBullet bulletPrefab;
    [Header("Delay Between Each Shooting Time")]
    [SerializeField] protected float delayTime;

    protected bool canShoot;
    protected IObjectPool<BaseBullet> bulletPool;

    #region ObjectPool
    protected void InitializeBulletPool()
    {
        bulletPool = new ObjectPool<BaseBullet>(CreateBullet, OnGetBulletFromPool, OnReleaseBulletToPool, OnDestroyBullet, defaultCapacity: 2, maxSize: 100);
    }

    protected BaseBullet CreateBullet()
    {
        BaseBullet bullet = Instantiate(bulletPrefab);
        bullet.ParentBulletPool = bulletPool;
        return bullet;
    }

    protected void OnGetBulletFromPool(BaseBullet bullet)
    {
        bullet.Show();
    }

    protected void OnReleaseBulletToPool(BaseBullet bullet)
    {
        bullet.Hide();
    }

    protected void OnDestroyBullet(BaseBullet bullet)
    {
        Destroy(bullet);
    }
    #endregion ObjectPool

    protected abstract void Shoot();
}

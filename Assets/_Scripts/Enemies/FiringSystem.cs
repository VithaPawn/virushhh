using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class FiringSystem : MonoBehaviour {
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float delayTime;
    [SerializeField] private float bulletExistTime;
    [SerializeField] private float bulletSpeed;

    private IObjectPool<Bullet> bulletPool;
    private bool canFire;

    #region ObjectPool
    public void Initialize()
    {
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBulletFromPool, OnReleaseBulletToPool, OnDestroyBullet, defaultCapacity: 2, maxSize: 100);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.ParentBulletPool = bulletPool;
        return bullet;
    }

    private void OnGetBulletFromPool(Bullet bullet)
    {
        bullet.Show();
    }

    private void OnReleaseBulletToPool(Bullet bullet)
    {
        bullet.Hide();
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet);
    }
    #endregion ObjectPool

    private void Awake()
    {
        canFire = false;
    }

    public void Fire()
    {
        StartCoroutine(FireEachCertainTime());
    }

    private IEnumerator FireEachCertainTime()
    {
        while (canFire) {
            Bullet bullet = bulletPool.Get();
            if (bullet.TryGetComponent(out Rigidbody2D bulletRb))
            {
                bulletRb.velocity = transform.forward * bulletSpeed;
            }
            bullet.Deactivate(bulletExistTime);
            
            yield return new WaitForSeconds(delayTime);
        }
    }
}

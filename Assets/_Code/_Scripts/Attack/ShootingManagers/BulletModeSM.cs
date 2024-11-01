using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletModeSM : BaseShootingManager {
    [SerializeField] private int shootingDirectionNumber;
    
    [Header("Bullet Attributes")]
    [SerializeField] private BaseBullet bulletPrefab;
    private IObjectPool<BaseBullet> bulletPool;

    [Header("Shooting Mode")]
    public ShootingMode shootingMode;
    [SerializeField] private float burstTime;
    [SerializeField] private int bulletNumberEachBurst;
    private List<Vector3> shootingDirectionList;

    private GameObject target;

    private void Awake()
    {
        InitializeBulletPool();
        target = GameObject.FindGameObjectWithTag(GameConstants.PLAYER_TAG);
        canShoot = true;
        shootingDirectionList = new List<Vector3>();
    }

    private void Start()
    {
        Shoot();
    }

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

    #region Shoot

    protected override void Shoot()
    {
        StartCoroutine(ShootBulletAtIntervals());
    }

    private IEnumerator ShootBulletAtIntervals()
    {
        while (canShoot)
        {
            yield return new WaitForSeconds(reloadTime);
            shootingDirectionList = GenerateDirectionList(shootingDirectionList, shootingDirectionNumber);
            switch (shootingMode)
            {
                case ShootingMode.Single:
                    ShootSingle();
                    break;
                case ShootingMode.Burst:
                    ShootBurst();
                    break;
                default:
                    ShootSingle();
                    break;
            }
        }
    }

    private void ShootSingle()
    {
        for (int i = 0; i < shootingDirectionNumber; i++)
        {
            PushBulletTowardDirection(shootingDirectionList[i]);
        }
    }

    private void ShootBurst()
    {
        if (bulletNumberEachBurst == 0)
        {
            ShootSingle();
            return;
        }
        float burstTimeCounter = 0;
        for (int i = 0; i < bulletNumberEachBurst; i++)
        {
            StartCoroutine(ShootInBatch(burstTimeCounter));
            burstTimeCounter += burstTime;
        }
    }

    private IEnumerator ShootInBatch(float time)
    {
        yield return new WaitForSeconds(time);
        ShootSingle();
    }

    private void PushBulletTowardDirection(Vector3 direction)
    {
        BaseBullet bullet = bulletPool.Get();
        bullet.SetPosition(transform.position);
        bullet.MoveTowardDirection(direction);
        bullet.DeactivateAfterSeconds();
    }

    #endregion Shoot

    #region Helpers

    private List<Vector3> GenerateDirectionList(List<Vector3> directionList, int directionNumber)
    {
        directionList.Clear();
        float lookingAngle = GetLookingAngle(target.transform.position);
        float angleStep = 2f * Mathf.PI / directionNumber;
        for (int i = 0; i < directionNumber; i++)
        {
            float angle = angleStep * i + lookingAngle;
            float xPos = Mathf.Cos(angle);
            float zPos = Mathf.Sin(angle);
            Vector3 pos = new Vector3(xPos, zPos, 0).normalized;

            directionList.Add(pos);
        }
        return directionList;
    }

    private float GetLookingAngle(Vector3 targetPosition)
    {
        Vector3 lookingVector = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(lookingVector.y, lookingVector.x); // For 2D, use y and x.
        return angle; // Angle in radians
    }

    #endregion Helpers

    public enum ShootingMode {
        Single,
        Burst
    }

}

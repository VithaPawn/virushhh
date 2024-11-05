using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletModeSM : BaseShootingManager {

    public enum ShootingMode {
        Single,
        Burst
    }

    #region VARIABLES
    [SerializeField] private int shootingDirectionNumber;
    private List<Vector3> shootingDirectionList;

    [Header("Bullet Attributes")]
    [SerializeField] private BaseBullet bulletPrefab;
    private GameObject bulletStorage;
    private IObjectPool<BaseBullet> bulletPool;

    [Header("Shooting Mode")]
    public ShootingMode shootingMode;
    [SerializeField] private float burstTime;
    [SerializeField] private int bulletNumberEachBurst;
    #endregion VARIABLES

    private void Awake()
    {
        //Get bullet storage (which store bullet objects)
        bulletStorage = GameObject.FindWithTag(GameConstants.BULLET_STORAGE_TAG);
        //If not, create it
        if (!bulletStorage) bulletStorage = new GameObject("BulletStorage");
        bulletStorage.tag = GameConstants.BULLET_STORAGE_TAG;
        
        //Initialize other class variables
        InitializeBulletPool();
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
        BaseBullet bullet = Instantiate(bulletPrefab, bulletStorage.transform);
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
        shootingDirectionList = GenerateDirectionList(shootingDirectionList, shootingDirectionNumber);
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
        float lookingAngle = GetLookingAngle();
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

    private float GetLookingAngle()
    {
        Vector3 lookingVector = transform.right;
        float angle = Mathf.Atan2(lookingVector.y, lookingVector.x);
        return angle; // Angle in radians
    }

    #endregion Helpers
}

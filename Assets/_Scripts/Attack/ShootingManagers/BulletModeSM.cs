using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModeSM : BaseShootingManager {
    [Header("Shooting Dỉrection Number")]
    [SerializeField] private int shootingDirectionNumber;
    [Header("Shooting Mode")]
    public ShootingMode shootingMode;
    [SerializeField] private float burstTime;
    [SerializeField] private int bulletNumberEachBurst;

    private GameObject target;
    private List<Vector3> shootingDirectionList;

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

    protected override void Shoot()
    {
        StartCoroutine(ShootInTurns());
    }


    private IEnumerator ShootInTurns()
    {
        while (canShoot)
        {
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
            yield return new WaitForSeconds(delayTime);
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
        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x); // For 2D, use y and x.
        return angle; // Angle in radians
    }

    public enum ShootingMode {
        Single,
        Burst
    }
}

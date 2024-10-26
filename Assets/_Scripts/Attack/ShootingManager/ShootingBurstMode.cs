using System.Collections;
using UnityEngine;

public class ShootingBurstMode : BaseShootingManager {
    [Header("Burst Mode")]
    [SerializeField] private float burstTime;
    [SerializeField] private int bulletNumberEachBurst;

    private GameObject target;

    private void Awake()
    {
        InitializeBulletPool();
        target = GameObject.FindGameObjectWithTag(GameConstants.PLAYER_TAG);
        canShoot = true;
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
            ShootBurstly();
            yield return new WaitForSeconds(delayTime);
        }
    }

    private void ShootBurstly()
    {
        float burstTimeCounter = 0;
        for (int i = 0; i < bulletNumberEachBurst; i++)
        {
            StartCoroutine(ShootSingle(burstTimeCounter));
            burstTimeCounter += burstTime;
        }
    }

    private IEnumerator ShootSingle(float time)
    {
        yield return new WaitForSeconds(time);
        PushBulletToward();
    }

    private void PushBulletToward()
    {
        BaseBullet bullet = bulletPool.Get();
        bullet.SetPosition(transform.position);
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        bullet.MoveTowardDirection(targetDirection);
        bullet.DeactivateAfterSeconds();
    }
}

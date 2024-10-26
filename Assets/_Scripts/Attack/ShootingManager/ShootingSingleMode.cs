using System.Collections;
using UnityEngine;

public class ShootingSingleMode : BaseShootingManager {
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
            BaseBullet bullet = bulletPool.Get();
            bullet.SetPosition(transform.position);
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            bullet.MoveTowardDirection(targetDirection);
            bullet.DeactivateAfterSeconds();

            yield return new WaitForSeconds(delayTime);
        }
    }
}

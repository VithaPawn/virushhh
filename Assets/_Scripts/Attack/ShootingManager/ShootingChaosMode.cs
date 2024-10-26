using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingChaosMode : BaseShootingManager {
    [Header("Chaos Mode")]
    [SerializeField] private int shootingDirectionNumber;

    private List<Vector3> shootingDirectionList;

    private void Awake()
    {
        InitializeBulletPool();
        canShoot = true;
        shootingDirectionList = new List<Vector3>();
    }

    private void Start()
    {
        shootingDirectionList = GenerateDirectionList(shootingDirectionList, shootingDirectionNumber);
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
            for (int i = 0; i < shootingDirectionNumber; i++)
            {
                PushBulletTowardDirection(shootingDirectionList[i]);
            }
            yield return new WaitForSeconds(delayTime);
        }
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
        float angleStep = 2f * Mathf.PI / directionNumber;
        for (int i = 0; i < directionNumber; i++)
        {
            float xPos = Mathf.Cos(angleStep * i);
            float zPos = Mathf.Sin(angleStep * i);
            Vector3 pos = new Vector3(xPos, zPos, 0).normalized;

            directionList.Add(pos);
        }
        return directionList;
    }
}

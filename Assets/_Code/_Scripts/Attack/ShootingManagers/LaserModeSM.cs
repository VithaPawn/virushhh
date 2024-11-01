using System.Collections;
using UnityEngine;

[RequireComponent (typeof(LineRenderer))]

public class LaserModeSM : BaseShootingManager {
    [Header("Laser Attributes")]
    [SerializeField] private float laserLength;
    [SerializeField] private float laserChargingTime;

    private LineRenderer laserRenderer;
    private GameObject target;

    private void Awake()
    {
        laserRenderer = GetComponent<LineRenderer>();
        target = GameObject.FindGameObjectWithTag(GameConstants.PLAYER_TAG);
    }

    //private void Start()
    //{
    //    laserRenderer.enabled = false;
    //    Shoot();
    //}

    private void Update()
    {
        DrawLaserRay();
    }

    protected override void Shoot()
    {
        StartCoroutine(ShootLaserAtIntervals());
    }

    private IEnumerator ShootLaserAtIntervals()
    {
        while (canShoot)
        {
            yield return new WaitForSeconds(reloadTime);
            DrawLaserRay();
            yield return new WaitForSeconds(laserChargingTime);
            FireLaser();
            laserRenderer.enabled = false;
        }
    }

    private void DrawLaserRay()
    {
        Vector3 lookingVector = (target.transform.position - transform.position).normalized;

        laserRenderer.SetPosition(0, transform.position);
        laserRenderer.SetPosition(1, transform.position + lookingVector * laserLength);
    }

    private void FireLaser()
    {

    }
}

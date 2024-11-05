using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LaserModeSM : BaseShootingManager {
    [Header("Laser Attributes")]
    [SerializeField] private float laserLength;
    [SerializeField] private float chargingTime;
    [SerializeField] private float waitingTimeBeforeFire;
    [SerializeField] private float disapearTimeAfterFire;

    private LineRenderer laserRenderer;
    private GameObject target;
    private EnemyRotationController rotationController;
    private Vector2 laserDirection;

    private void Awake()
    {
        laserRenderer = GetComponent<LineRenderer>();
        target = GameObject.FindGameObjectWithTag(GameConstants.PLAYER_TAG);
        rotationController = GetComponent<EnemyRotationController>();
    }

    private void Start()
    {
        laserRenderer.enabled = false;
        if (!target || !rotationController) return;
        Shoot();
    }

    protected override void Shoot()
    {
        StartCoroutine(ShootLaserAtIntervals());
    }

    private IEnumerator ShootLaserAtIntervals()
    {
        while (canShoot)
        {
            //Draw laser line
            yield return new WaitForSeconds(reloadTime);
            laserRenderer.enabled = true;
            rotationController.SetFocusMode(EnemyRotationController.FocusMode.None);
            DrawLaserRay();

            //Fire laser
            yield return new WaitForSeconds(chargingTime);
            StartCoroutine(FireLaser());
        }
    }

    private void DrawLaserRay()
    {
        SetLineWidth(laserRenderer, 0.035f);

        Vector3 lookingVector = transform.right;
        laserRenderer.SetPosition(0, transform.position);
        laserRenderer.SetPosition(1, transform.position + lookingVector * laserLength);
        laserDirection = new Vector2(lookingVector.x, lookingVector.y);
    }

    private IEnumerator FireLaser()
    {
        //The width of the laser expands to max
        DOLineWidth(laserRenderer, 0.15f, waitingTimeBeforeFire);
        yield return new WaitForSeconds(waitingTimeBeforeFire);

        //The laser deals damage
        DealDamage();

        //Then, it shrinks and disapears
        DOLineWidth(laserRenderer, 0f, disapearTimeAfterFire);
        yield return new WaitForSeconds(disapearTimeAfterFire);
        laserRenderer.enabled = false;
        rotationController.SetFocusMode(EnemyRotationController.FocusMode.TargetFocusing);
    }

    private void DealDamage()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, laserDirection);

        if (hit.collider != null && hit.transform.TryGetComponent(out Virus virus))
        {
            Debug.Log(virus.ToString());
        }
    }

    private void SetLineWidth(LineRenderer line, float width)
    {
        line.startWidth = width;
        line.endWidth = width;
    }

    private void DOLineWidth(LineRenderer line, float endValue, float duration)
    {
        DOTween.To(() => line.startWidth, x => { line.startWidth = x; }, endValue, duration);
        DOTween.To(() => line.endWidth, x => { line.endWidth = x; }, endValue, duration);
    }
}

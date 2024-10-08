using System.Collections;
using UnityEngine;

public class DashingManager : MonoBehaviour {
    [Header("Visual")]
    [SerializeField] private DashingTarget dashingTarget;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private DrawingCircle drawingCircle;
    [Header("Dashing Attributes")]
    [SerializeField] private float dashingTargetMovementSpeed;
    [SerializeField] private float dashingDuration;
    private bool isDashing = false;
    [Header("Ghosting Effect Attributes")]
    [SerializeField] private float distanceBetweenEachGhost;
    private Vector3 currentDashingPos;

    //private GhostingEffectManager ghostingEffectManager;
    private GameObject movementAllowedArea;
    private CustomFloatingJoystic floatingJoystick;

    private void Awake()
    {
        movementAllowedArea = GameObject.FindGameObjectWithTag(GameConstants.PLAYING_AREA_TAG);
        GameObject joystick = GameObject.FindGameObjectWithTag(GameConstants.JOYSTICK_TAG);
        floatingJoystick = joystick.GetComponent<CustomFloatingJoystic>();
        //ghostingEffectManager = GetComponent<GhostingEffectManager>();
    }

    private void OnEnable()
    {
        floatingJoystick.PointerUpEvent += OnPointerUp;
        floatingJoystick.PointerDownEvent += OnPointerDown;
    }

    private void OnDisable()
    {
        floatingJoystick.PointerUpEvent -= OnPointerUp;
        floatingJoystick.PointerDownEvent -= OnPointerDown;
    }

    private void OnPointerUp()
    {
        StartCoroutine(PlayDashEffect());
    }

    private void OnPointerDown()
    {
        dashingTarget.Show();
    }

    private void Update()
    {
        if (isDashing && Vector3.Distance(transform.position, currentDashingPos) >= distanceBetweenEachGhost)
        {
            currentDashingPos = transform.position;
            //ghostingEffectManager.GenerateGhost(transform.position, 0.3f);
        }
    }

    private IEnumerator PlayDashEffect()
    {
        //Play dash animation
        dashingTarget.Hide();
        currentDashingPos = transform.position;
        isDashing = true;
        trailRenderer.emitting = true;
        drawingCircle.HideCircle();

        //Wait for dash animation
        yield return new WaitForSeconds(dashingDuration);

        //Reset dashing variables
        isDashing = false;
        trailRenderer.emitting = false;
        drawingCircle.ShowCircle();
    }

    public void MoveDashingTarget(Vector3 movementVector)
    {
        Vector3 dashingTargetPos = dashingTarget.GetPosition();
        Vector3 tempDashingTargetPos = dashingTargetPos + movementVector * dashingTargetMovementSpeed * Time.deltaTime;
        float distance = Vector3.Distance(tempDashingTargetPos, transform.position) + MovementUtilities.GetObjectWidth(GetDashingTargetObj()) / 2;
        dashingTarget.SetPosition(MovementUtilities.LimitPositionInsideArea(movementAllowedArea, GetDashingTargetObj(),
            distance <= drawingCircle.GetRadius() ? tempDashingTargetPos : dashingTargetPos));
    }

    public Vector3 GetDashingTargetPos() { return dashingTarget.GetPosition(); }

    public float GetDashingDuration() { return dashingDuration; }

    public GameObject GetDashingTargetObj() { return dashingTarget.gameObject; }
}

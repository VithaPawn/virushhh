using System.Collections;
using UnityEngine;

public class DashingManager : MonoBehaviour {
    [Header("Joystick")]
    [SerializeField] private CustomFloatingJoystic _floatingJoystick;
    [Header("Dashing Attributes")]
    [SerializeField] private DashingTarget dashingTarget;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private DrawingCircle drawingCircle;
    [SerializeField] private float dashingTargetMovementSpeed;
    [SerializeField] private float dashingDuration;

    private GameObject movingArea;

    private void Awake()
    {
        movingArea = GameObject.FindGameObjectWithTag("");
    }

    private void OnEnable()
    {
        _floatingJoystick.PointerUpEvent += OnPointerUp;
        _floatingJoystick.PointerDownEvent += OnPointerDown;
    }

    private void OnDisable()
    {
        _floatingJoystick.PointerUpEvent -= OnPointerUp;
        _floatingJoystick.PointerDownEvent -= OnPointerDown;
    }

    private void OnPointerUp()
    {
        StartCoroutine(Dash());
    }

    private void OnPointerDown()
    {
        dashingTarget.Show();
    }

    private IEnumerator Dash()
    {
        //Play dash animation
        dashingTarget.Hide();
        trailRenderer.emitting = true;
        drawingCircle.HideCircle();
        //Wait for dash animation
        yield return new WaitForSeconds(dashingDuration);
        //Reset dashing variables
        trailRenderer.emitting = false;
        drawingCircle.ShowCircle();
    }

    public void MoveDashingTarget(Vector3 movementVector)
    {
        Vector3 dashingTargetPos = dashingTarget.GetPosition();
        Vector3 tempDashingTargetPos = dashingTargetPos + movementVector * dashingTargetMovementSpeed * Time.deltaTime;
        float distance = Vector3.Distance(tempDashingTargetPos, transform.position) + MovementUtilities.GetObjectWidth(GetDashingTargetObj()) / 2;
        dashingTarget.SetPosition(MovementUtilities.LimitPositionInsideArea(movingArea, GetDashingTargetObj(),
            distance <= drawingCircle.GetRadius() ? tempDashingTargetPos : dashingTargetPos));
    }

    public Vector3 GetDashingTargetPos() { return dashingTarget.GetPosition(); }

    public float GetDashingDuration() { return dashingDuration; }

    public GameObject GetDashingTargetObj() { return dashingTarget.gameObject; }
}

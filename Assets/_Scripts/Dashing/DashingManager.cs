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

    private GameObject movementAllowedArea;
    private CustomFloatingJoystic floatingJoystick;

    private void Awake()
    {
        movementAllowedArea = GameObject.FindGameObjectWithTag(GameConstants.PLAYING_AREA_TAG);
        GameObject joystick = GameObject.FindGameObjectWithTag(GameConstants.JOYSTICK_TAG);
        floatingJoystick = joystick.GetComponent<CustomFloatingJoystic>();
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

    private IEnumerator PlayDashEffect()
    {
        //Play dash animation
        dashingTarget.Hide();
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
        
        //Get dashing target width
        Renderer objRenderer = dashingTarget.gameObject.GetComponent<Renderer>();
        float dashingTargetWidth = objRenderer ? objRenderer.bounds.size.x : 0;

        float distance = Vector3.Distance(tempDashingTargetPos, transform.position) + dashingTargetWidth / 2;
        dashingTarget.SetPosition(MovementUtilities.LimitPositionInsideArea(movementAllowedArea, dashingTarget.gameObject,
            distance <= drawingCircle.GetRadius() ? tempDashingTargetPos : dashingTargetPos));
    }


    private float GetObjectWidth(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        return objRenderer ? objRenderer.bounds.size.x : 0;
    }

    public DashingTarget GetDashingTarget() { return dashingTarget; }

    public float GetDashingDuration() { return dashingDuration; }
}

using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VirusDashingController : MonoBehaviour {
    [Header("Visual")]
    [SerializeField] private DashingTarget dashingTarget;
    [SerializeField] private DrawingCircle drawingCircle;
    [Header("Dashing Attributes")]
    [SerializeField] private float dashingTargetMovementSpeed;
    [SerializeField] private float dashingDuration;
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannelSO startDashingSO;
    [SerializeField] private VoidEventChannelSO endDashingSO;

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
        transform.DOMove(dashingTarget.GetPosition(), dashingDuration);
        StartCoroutine(PlayDashEffect());
    }

    private void OnPointerDown()
    {
        dashingTarget.Show();
    }

    private void Update()
    {
        if (floatingJoystick.Direction != Vector2.zero)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        Vector2 joystickDirectionVector = floatingJoystick.Direction.normalized;
        Vector3 movementVector = new Vector3(joystickDirectionVector.x, joystickDirectionVector.y, 0);
        MoveDashingTarget(movementVector);
    }

    private IEnumerator PlayDashEffect()
    {
        startDashingSO.RaiseEvent();

        //Wait for dash animation
        yield return new WaitForSeconds(dashingDuration);

        endDashingSO.RaiseEvent();
    }

    private void MoveDashingTarget(Vector3 movementVector)
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
}

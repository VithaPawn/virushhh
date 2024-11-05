using DG.Tweening;
using UnityEngine;

public class MovementManager : MonoBehaviour {
    [Header("Movement Attributes")]
    [SerializeField] private float movementSpeed;
    
    private GameObject movementAllowedArea;
    private CustomFloatingJoystic floatingJoystick;
    private DashingManager dashingManager;

    private void Awake()
    {
        movementAllowedArea = movementAllowedArea = GameObject.FindGameObjectWithTag(GameConstants.PLAYING_AREA_TAG);
        GameObject joystick = GameObject.FindGameObjectWithTag(GameConstants.JOYSTICK_TAG);
        floatingJoystick = joystick.GetComponent<CustomFloatingJoystic>();
        dashingManager = GetComponent<DashingManager>();
    }

    private void OnEnable()
    {
        floatingJoystick.PointerUpEvent += OnPointerUp;
    }

    private void OnDisable()
    {
        floatingJoystick.PointerUpEvent -= OnPointerUp;
    }
    private void OnPointerUp()
    {
        transform.DOMove(dashingManager.GetDashingTarget().GetPosition(), dashingManager.GetDashingDuration());
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
        // Move
        Vector3 pos = transform.position + movementVector * movementSpeed * Time.deltaTime;
        transform.position = MovementUtilities.LimitPositionInsideArea(movementAllowedArea, dashingManager.GetDashingTarget().gameObject, pos);
        // Look at
        Quaternion lookingQuater = MovementUtilities.GetQuaternionByTargetPosition(
            dashingManager.GetDashingTarget().transform.position, transform.position);
        transform.rotation = lookingQuater;
        // Move dashing target
        dashingManager.MoveDashingTarget(movementVector);
    }
}

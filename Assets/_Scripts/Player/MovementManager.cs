using DG.Tweening;
using UnityEngine;

public class MovementManager : MonoBehaviour {
    [Header("Joystick")]
    [SerializeField] private CustomFloatingJoystic _floatingJoystick;
    [Header("Movement Attributes")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject movingArea;

    private DashingManager dashingManager;

    private void Awake()
    {
        dashingManager = GetComponent<DashingManager>();
    }

    private void OnEnable()
    {
        _floatingJoystick.PointerUpEvent += OnPointerUp;
    }

    private void OnDisable()
    {
        _floatingJoystick.PointerUpEvent -= OnPointerUp;
    }
    private void OnPointerUp()
    {
        transform.DOMove(dashingManager.GetDashingTargetPos(), dashingManager.GetDashingDuration());
    }

    private void Update()
    {
        if (_floatingJoystick.Direction != Vector2.zero)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        Vector2 joystickDirectionVector = _floatingJoystick.Direction.normalized;
        Vector3 movementVector = new Vector3(joystickDirectionVector.x, joystickDirectionVector.y, 0);
        // Move
        Vector3 pos = transform.position + movementVector * movementSpeed * Time.deltaTime;
        transform.position = MovementUtilities.LimitPositionInsideArea(movingArea, dashingManager.GetDashingTargetObj(), pos);
        // Look at
        float angle = Mathf.Atan2(movementVector.y, movementVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // Move dashing target
        dashingManager.MoveDashingTarget(movementVector);
    }
}

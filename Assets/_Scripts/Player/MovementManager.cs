using DG.Tweening;
using UnityEngine;

public class MovementManager : MonoBehaviour {

    [Header("Joystick")]
    [SerializeField] private CustomFloatingJoystic _floatingJoystick;
    
    [Header("Movement Attributes")]
    [SerializeField] private VirusShadow virusShadow;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationDuration;

    private Vector3 currentMovementVector;
    private DashingController dashingController;

    private void Awake()
    {
        dashingController = GetComponent<DashingController>();
    }

    private void OnEnable()
    {
        _floatingJoystick.PointerUpEvent += OnPointerUp;
    }

    private void OnDisable()
    {
        _floatingJoystick.PointerUpEvent -= OnPointerUp;
    }

    private void FixedUpdate()
    {
        if (_floatingJoystick.Direction != Vector2.zero)
        {
            MoveAndLook();
        }
    }

    private void MoveAndLook()
    {
        Vector2 joystickDirectionVector = _floatingJoystick.Direction.normalized;
        Vector3 movementVector = new Vector3(joystickDirectionVector.x, joystickDirectionVector.y, 0);
        if (currentMovementVector.x != movementVector.x || currentMovementVector.y != movementVector.y)
        {
            currentMovementVector = movementVector;
        }
        // Move
        transform.position += movementVector * Time.deltaTime * movementSpeed;
        // Look at
        float angle = Mathf.Atan2(movementVector.y, movementVector.x) * Mathf.Rad2Deg;
        transform.DORotate(new Vector3(0, 0, angle), rotationDuration);
        // Prepare for dashing
        dashingController.PrepareForDash(movementVector);
    }

    private void OnPointerUp()
    {
        transform.position = transform.position = Vector3.Lerp(transform.position, virusShadow.GetPosition(), 1f);
    }
}

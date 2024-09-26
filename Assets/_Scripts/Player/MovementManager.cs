using UnityEngine;

public class MovementManager : MonoBehaviour {

    [Header("Joystick")]
    [SerializeField] private CustomFloatingJoystic _floatingJoystick;
    [Header("Movement Attributes")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private VirusShadow virusShadow;
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

    private void Update()
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
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // Prepare for dashing
        dashingController.PrepareForDash(currentMovementVector);
    }

    private void OnPointerUp()
    {
        transform.position = transform.position = Vector3.Lerp(transform.position, virusShadow.GetPosition(), 1f);
    }
}

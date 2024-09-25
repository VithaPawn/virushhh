using UnityEngine;

public class MovementManager : MonoBehaviour {

    [Header("Joystick")]
    [SerializeField] private CustomFloatingJoystic _floatingJoystick;
    [Header("Movement Attributes")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dashingDistanceIncreaseSpeed;
    [Header("Dashing")]
    [SerializeField] private VirusShadow virusShadow;
    private Vector3 currentMovementVector;
    private float dashingDistance;

    private Rigidbody2D _rb;

    private void OnEnable()
    {
        _floatingJoystick.PointerDownEvent += OnPointerDown;
        _floatingJoystick.PointerUpEvent += OnPointerUp;
    }

    private void OnDisable()
    {
        _floatingJoystick.PointerDownEvent -= OnPointerDown;
        _floatingJoystick.PointerUpEvent -= OnPointerUp;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        // Increase dashing distance
        dashingDistance += dashingDistanceIncreaseSpeed * Time.deltaTime;
        Vector3 positionAfterDash = transform.position + currentMovementVector * dashingDistance;
        virusShadow.SetPosition(positionAfterDash);
    }

    private void OnPointerUp()
    {
        //Dash to shadow
        transform.position = virusShadow.GetPosition();
        //Reset dashing variables
        virusShadow.Hide();
        dashingDistance = 0;
    }

    private void OnPointerDown()
    {
        virusShadow.Show();
    }
}

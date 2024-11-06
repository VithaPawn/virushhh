using UnityEngine;

public class VirusMovementController : MonoBehaviour {
    [Header("Visual")]
    [SerializeField] private DashingTarget dashingTarget;
    [Header("Movement Attributes")]
    [SerializeField] private float movementSpeed;

    private GameObject movementAllowedArea;
    private CustomFloatingJoystic floatingJoystick;

    private void Awake()
    {
        movementAllowedArea = movementAllowedArea = GameObject.FindGameObjectWithTag(GameConstants.PLAYING_AREA_TAG);
        GameObject joystick = GameObject.FindGameObjectWithTag(GameConstants.JOYSTICK_TAG);
        floatingJoystick = joystick.GetComponent<CustomFloatingJoystic>();
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
        transform.position = MovementUtilities.LimitPositionInsideArea(movementAllowedArea, dashingTarget.gameObject, pos);
        // Look at
        Quaternion lookingQuater = MovementUtilities.GetQuaternionByTargetPosition(dashingTarget.GetPosition(), transform.position);
        transform.rotation = lookingQuater;
    }
}

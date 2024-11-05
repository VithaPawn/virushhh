using UnityEngine;

public class EnemyRotationController : MonoBehaviour {
    [SerializeField] private FocusMode focusMode;
    [SerializeField] private float rotationSpeed;

    private GameObject target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(GameConstants.PLAYER_TAG);
    }

    void Update()
    {
        ControlLookingBehavior();
    }

    private void ControlLookingBehavior()
    {
        switch (focusMode)
        {
            case FocusMode.None:
                break;

            case FocusMode.TargetFocusing:
                Vector3 lookingDirection = (target.transform.position - transform.position).normalized;
                float targetAngle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg;
                float currentAngle = transform.rotation.eulerAngles.z;
                // Rotate smoothly
                float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, newAngle);
                break;

            case FocusMode.Rotation:
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    public void SetFocusMode(FocusMode mode) { focusMode = mode; }

    public enum FocusMode {
        None = 0, // The enemy does not rotate
        TargetFocusing = 1, // The enemy rotate follow player
        Rotation = 2 // The enemy rotate continuously 
    }
}

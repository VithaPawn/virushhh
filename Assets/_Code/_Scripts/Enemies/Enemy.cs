using UnityEngine;

public class Enemy : MonoBehaviour {
    private const float CLOSET_DISTANCE_BEFORE_CHANGE_DIRECTION = 0.1f;

    private GameObject movementAllowedArea;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool canMove = true;

    public GameObject Target { get; private set; }
    private Vector3 movementDestination;
    private Vector3 movementDirection;

    private void Awake()
    {
        Target = GameObject.FindGameObjectWithTag(GameConstants.PLAYER_TAG);
        movementAllowedArea = GameObject.FindGameObjectWithTag(GameConstants.PLAYING_AREA_TAG);
    }

    private void OnEnable()
    {
        SetNewDestination();
    }

    private void Update()
    {
        if (canMove)
        {
            //Move
            transform.position += movementDirection * Time.deltaTime * movementSpeed;
            if (Vector3.Distance(movementDestination, transform.position) <= CLOSET_DISTANCE_BEFORE_CHANGE_DIRECTION)
            {
                SetNewDestination();
            }
            //Look at
            Quaternion lookingQuater = MovementUtilities.Rotate2dByTargetPosition(
                Target.transform.position, transform.position);
            transform.rotation = lookingQuater;
        }
    }

    private void SetNewDestination()
    {
        movementDestination = MovementUtilities.GenerateRandomVectorInsideArea(movementAllowedArea);
        movementDirection = movementDestination - transform.position;
        movementDirection.Normalize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Virus virus))
        {
            Destroy(gameObject);
        }
    }
}

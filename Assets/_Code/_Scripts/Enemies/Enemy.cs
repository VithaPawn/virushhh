using UnityEngine;

public class Enemy : MonoBehaviour {
    private const float CLOSET_DISTANCE_BEFORE_CHANGE_DIRECTION = 0.1f;

    private GameObject movementAllowedArea;
    [Header("Ability To Do Something")]
    [SerializeField] private bool canMove = true;
    [Header("Movement Attributes")]
    [SerializeField] private float movementSpeed;

    private Vector3 movementDestination;
    private Vector3 movementDirection;

    private void Awake()
    {
        movementAllowedArea = GameObject.FindGameObjectWithTag(GameConstants.PLAYING_AREA_TAG);
    }

    private void OnEnable()
    {
        GenerateDestination();
    }

    private void Update()
    {
        //Move
        if (canMove)
        {
            transform.position += movementDirection * Time.deltaTime * movementSpeed;
            if (Vector3.Distance(movementDestination, transform.position) <= CLOSET_DISTANCE_BEFORE_CHANGE_DIRECTION)
            {
                GenerateDestination();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Virus virus))
        {
            Destroy(gameObject);
        }
    }

    private void GenerateDestination()
    {
        movementDestination = MovementUtilities.GenerateRandomVectorInsideArea(movementAllowedArea);
        movementDirection = movementDestination - transform.position;
        movementDirection.Normalize();
    }

    public void SetCanMove(bool value) { canMove = value; }
}

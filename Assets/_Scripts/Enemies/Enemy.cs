using UnityEngine;

public class Enemy : MonoBehaviour {
    private const float CLOSET_DISTANCE_BEFORE_CHANGE_DIRECTION = 0.1f;

    private GameObject movementAllowedArea;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool canMove = true;

    private Vector3 movementDestination;
    private Vector3 movementDirection;

    private void Awake()
    {
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
            transform.position += movementDirection * Time.deltaTime * movementSpeed;
            if (Vector3.Distance(movementDestination, transform.position) <= CLOSET_DISTANCE_BEFORE_CHANGE_DIRECTION)
            {
                SetNewDestination();
            }
        }
    }

    private void SetNewDestination()
    {
        movementDestination = MovementUtilities.GenerateRandomVectorInsideArea(movementAllowedArea);
        movementDirection = movementDestination - transform.position;
        movementDirection.Normalize();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, movementDestination);
    }
}

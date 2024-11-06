using DG.Tweening;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour {
    #region CONSTANTS
    private const float CLOSET_DISTANCE_BEFORE_CHANGE_DIRECTION = 0.1f;
    #endregion CONSTANTS

    public enum MovementMode {
        Fixed,
        MoveRandom,
        RushToward,
    }

    #region VARIABLES
    private GameObject movementAllowedArea;
    [Header("Movement Attributes")]
    public MovementMode movementMode;
    [Header("MoveRandom Mode Attributes")]
    [SerializeField] private float movementSpeed;
    private Vector3 movementDestination;
    private Vector3 movementDirection;
    [Header("RushForward Mode Attributes")]
    [SerializeField] private float reloadTime;
    [SerializeField] private float rushDistance;
    [SerializeField] private float rushTime;

    private EnemyRotationController rotationController;
    #endregion VARIABLES

    private void Awake()
    {
        movementAllowedArea = GameObject.FindGameObjectWithTag(GameConstants.PLAYING_AREA_TAG);
        rotationController = GetComponent<EnemyRotationController>();
    }

    private void OnEnable()
    {
        if (movementMode == MovementMode.MoveRandom)
        {
            GenerateDestination();
        }
    }

    private void Start()
    {
        if (movementMode == MovementMode.RushToward)
        {
            StartCoroutine(RushAtIntervals());
        }
    }

    private void Update()
    {
        switch (movementMode)
        {
            case MovementMode.Fixed:
                break;
            case MovementMode.MoveRandom:
                MoveRandom();
                break;
            default:
                break;
        }
    }

    private void MoveRandom()
    {
        transform.position += movementDirection * Time.deltaTime * movementSpeed;
        if (Vector3.Distance(movementDestination, transform.position) <= CLOSET_DISTANCE_BEFORE_CHANGE_DIRECTION)
        {
            GenerateDestination();
        }
    }

    private IEnumerator RushAtIntervals()
    {
        while (movementMode == MovementMode.RushToward && rotationController)
        {
            yield return new WaitForSeconds(reloadTime);
            //Lock the looking direction
            rotationController.ChangeToFixedMode();
            //Change object transform
            RushToward();
            yield return new WaitForSeconds(rushTime);
            //Unlock the looking direction
            rotationController.ChangeToFocusMode();
        }
    }

    private void RushToward()
    {
        Vector3 targetPosition = transform.position + transform.right * rushDistance;
        targetPosition = MovementUtilities.LimitPositionInsideArea(movementAllowedArea, gameObject, targetPosition);
        transform.DOMove(targetPosition, rushTime);
    }

    private void GenerateDestination()
    {
        movementDestination = MovementUtilities.GenerateRandomVectorInsideArea(movementAllowedArea);
        movementDirection = movementDestination - transform.position;
        movementDirection.Normalize();
    }
}

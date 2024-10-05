using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MovementManager : MonoBehaviour {
    [Header("Joystick")]
    [SerializeField] private CustomFloatingJoystic _floatingJoystick;

    [Header("Movement Attributes")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject movingArea;

    [Header("Dashing Attributes")]
    [SerializeField] private ObjectBase dashingTarget;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private DrawingCircle drawingCircle;
    [SerializeField] private float shadowMovementSpeed;
    [SerializeField] private float dashingDuration;

    private void OnEnable()
    {
        _floatingJoystick.PointerUpEvent += OnPointerUp;
        _floatingJoystick.PointerDownEvent += OnPointerDown;
    }

    private void OnDisable()
    {
        _floatingJoystick.PointerUpEvent -= OnPointerUp;
        _floatingJoystick.PointerDownEvent -= OnPointerDown;
    }
    private void OnPointerUp()
    {
        transform.DOMove(dashingTarget.GetPosition(), dashingDuration);
        StartCoroutine(Dash());
    }

    private void OnPointerDown()
    {
        dashingTarget.Show();
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
        transform.position = LimitPositionInsideArea(movingArea, dashingTarget.gameObject, pos);
        // Look at
        float angle = Mathf.Atan2(movementVector.y, movementVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // Move shadow
        Vector3 shadowPos = dashingTarget.GetPosition();
        Vector3 tempShadowPos = shadowPos + movementVector * shadowMovementSpeed * Time.deltaTime;
        float distance = Vector3.Distance(tempShadowPos, transform.position) + GetObjectWidth(dashingTarget.gameObject) / 2;
        dashingTarget.SetPosition(LimitPositionInsideArea(movingArea, dashingTarget.gameObject,
            distance <= drawingCircle.GetRadius() ? tempShadowPos : shadowPos));
    }

    private IEnumerator Dash()
    {
        //Play dash animation
        dashingTarget.Hide();
        trailRenderer.emitting = true;
        drawingCircle.HideCircle();
        //Wait for dash animation
        yield return new WaitForSeconds(dashingDuration);
        //Reset dashing variables
        trailRenderer.emitting = false;
        drawingCircle.ShowCircle();
    }

    private Vector3 LimitPositionInsideArea(GameObject area, GameObject obj, Vector3 pos)
    {
        Vector3 limitedPos = pos;
        Renderer areaRenderer = area.GetComponent<Renderer>();
        Renderer objRenderer = obj.GetComponent<Renderer>();

        Bounds areaBounds = areaRenderer ? areaRenderer.bounds : new Bounds(Vector3.zero, Vector3.zero);
        float objWidth = objRenderer ? objRenderer.bounds.size.x : 0;
        float objHeight = objRenderer ? objRenderer.bounds.size.y : 0;

        limitedPos.x = Mathf.Clamp(limitedPos.x, areaBounds.min.x + objWidth, areaBounds.max.x - objWidth);
        limitedPos.y = Mathf.Clamp(limitedPos.y, areaBounds.min.y + objHeight, areaBounds.max.y - objHeight);

        return limitedPos;
    }

    private float GetObjectWidth(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        return objRenderer ? objRenderer.bounds.size.x : 0;
    }
}

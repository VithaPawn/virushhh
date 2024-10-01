using System.Collections;
using UnityEngine;

public class DashingController : MonoBehaviour {
    [Header("Joystick")]
    [SerializeField] private CustomFloatingJoystic _floatingJoystick;
    [Header("Dashing Attributes")]
    [SerializeField] private VirusShadow virusShadow;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private DrawingCircle drawingCircle;
    [SerializeField] private float shadowMovementSpeed;
    [SerializeField] private float dashingDuration;

    private float dashingDistance;

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

    private void OnPointerUp()
    {
        StartCoroutine(Dash());
    }

    private void OnPointerDown()
    {
        virusShadow.Show();
    }

    public void PrepareForDash(Vector3 movementVector)
    {
        dashingDistance += shadowMovementSpeed * Time.deltaTime;
        Vector3 positionAfterDash = transform.position + movementVector * dashingDistance;
        virusShadow.SetPosition(positionAfterDash);
    }

    public IEnumerator Dash()
    {
        //Play dash animation
        virusShadow.Hide();
        trailRenderer.emitting = true;
        drawingCircle.HideCircle();

        //Wait for dash animation
        yield return new WaitForSeconds(trailRenderer.time + 0.1f);

        //Reset dashing variables
        dashingDistance = 0;
        trailRenderer.emitting = false;
        drawingCircle.ShowCircle();
    }
}

using UnityEngine;

public class VirusVisual : MonoBehaviour {
    [Header("Visual Components")]
    [SerializeField] private TrailRenderer trailRenderer;
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannelSO startDashingSO;
    [SerializeField] private VoidEventChannelSO endDashingSO;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer ? spriteRenderer.color : Color.red;
    }

    private void OnEnable()
    {
        startDashingSO.OnEventRaised += OnDashing;
        endDashingSO.OnEventRaised += AfterDashing;
    }

    private void OnDisable()
    {
        startDashingSO.OnEventRaised -= OnDashing;
        endDashingSO.OnEventRaised -= AfterDashing;
    }

    private void OnDashing()
    {
        ChangeColorWhileDashing();
        ShowTrail();
    }

    private void AfterDashing()
    {
        RestoreColor();
        HideTrail();
    }

    private void RestoreColor()
    {
        spriteRenderer.color = defaultColor;
    }

    private void ChangeColorWhileDashing()
    {
        spriteRenderer.color = Color.green;
    }

    private void ShowTrail() { trailRenderer.emitting = true; }

    private void HideTrail() { trailRenderer.emitting = false; }
}

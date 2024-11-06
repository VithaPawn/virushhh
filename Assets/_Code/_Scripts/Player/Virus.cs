using UnityEngine;

public class Virus : MonoBehaviour {
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannelSO startDashingSO;
    [SerializeField] private VoidEventChannelSO endDashingSO;

    public bool IsTouchable { get; private set; }

    private void Awake()
    {
        IsTouchable = true;
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
        IsTouchable = false;
    }

    private void AfterDashing()
    {
        IsTouchable = true;
    }
}

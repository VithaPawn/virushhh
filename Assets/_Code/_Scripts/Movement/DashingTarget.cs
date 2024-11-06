using UnityEngine;

public class DashingTarget : MonoBehaviour {

    [Header("Event Channels")]
    [SerializeField] private VoidEventChannelSO startDashingSO;
    [SerializeField] private VoidEventChannelSO endDashingSO;

    private void OnEnable()
    {
        startDashingSO.OnEventRaised += Hide;
    }

    private void OnDisable()
    {
        startDashingSO.OnEventRaised -= Hide;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

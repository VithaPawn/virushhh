using UnityEngine;

public class DashingTarget : MonoBehaviour {
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

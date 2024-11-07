using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDot : MonoBehaviour {
    public void SetPosition(Vector3 dotPos)
    {
        transform.position = dotPos;
    }

    public void SetLocalPosition(Vector3 localPos)
    {
        transform.localPosition = localPos;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}

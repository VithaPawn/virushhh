using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/Void")]
public class VoidEventChannelSO : ScriptableObject {
    [SerializeField] private string description;

    public Action OnEventRaised;

    public void RaiseEvent()
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke();
        }
        else
        {
            Debug.LogWarning("An void event was raised, but nothing picked it up!\n" +
                "Here is the event's description: " + description);
        }
    }
}

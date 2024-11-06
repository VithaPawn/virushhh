using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Variables/Integer")]
public class IntegerVariableSO : ScriptableObject
{
    [SerializeField] private string description;
    [SerializeField] private int value;

    private void OnEnable()
    {
        ResetValueToZero();
    }

    public void IncreaseValue(int number)
    {
        value += number;
    }

    public void ResetValueToZero()
    {
        value = 0;
    }

    public int GetValue() { return value; }
}

using System;
using UnityEngine;

public class TriggerRelay : MonoBehaviour
{
    public event Action<Collider> OnEnter;

    private void OnTriggerStay(Collider other)
    {
        OnEnter?.Invoke(other);
    }
}
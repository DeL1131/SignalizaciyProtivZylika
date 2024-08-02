using System;
using UnityEngine;

public class House : MonoBehaviour
{
    public event Action HouseEntryDetected;
    public event Action HouseExitDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player _))
        {
            HouseEntryDetected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HouseExitDetected?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitAdaptor : MonoBehaviour, IInteractable
{
    public UnityEvent OnHitInteract;

    public void Interact()
    {
        OnHitInteract?.Invoke();
    }
}

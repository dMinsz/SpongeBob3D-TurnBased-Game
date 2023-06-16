using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBoss : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("SceneChangeBossBattle");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitter : MonoBehaviour, IHittable
{
    public void TakeHit()
    {
        Debug.Log("Player hit");
    }
}

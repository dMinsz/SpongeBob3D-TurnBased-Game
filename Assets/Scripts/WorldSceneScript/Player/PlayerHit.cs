using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour, IHittable
{
    public void TakeHit()
    {
        Debug.Log("몬스터가 플레이어 공격, 몬스터 어드벤지티");
    }
}

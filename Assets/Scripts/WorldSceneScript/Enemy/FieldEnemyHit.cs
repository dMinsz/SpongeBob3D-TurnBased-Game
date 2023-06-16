using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEnemyHit : MonoBehaviour, IHittable
{
    public void TakeHit()
    {
        Debug.Log("플레이어가 몬스터 공격, 플레이어 어드벤티지");
        GameManager.Battle.SettingBattle(BattleState.PLAYERTURN,2,3,0);
        GameManager.Scene.LoadScene("BattleScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEnemyHit : MonoBehaviour, IHittable
{
    public void TakeHit()
    {
        Debug.Log("�÷��̾ ���� ����, �÷��̾� ��庥Ƽ��");
        GameManager.Battle.SettingBattle(BattleState.PLAYERTURN,2,3,0);
        GameManager.Scene.LoadScene("BattleScene");
    }
}

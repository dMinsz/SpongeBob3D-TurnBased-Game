using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour, IHittable
{
    public void TakeHit()
    {
        Debug.Log("���Ͱ� �÷��̾� ����, ���� ��庥��Ƽ");
        GameManager.Battle.SettingBattle(BattleState.ENEMYTURN, 2, 3, 0);
        GameManager.Scene.LoadScene("BattleScene");
    }
}

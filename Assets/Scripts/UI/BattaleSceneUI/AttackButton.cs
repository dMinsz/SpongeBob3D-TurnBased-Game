using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButton : BattleMenuButton
{
   
    protected override void Awake()
    {
        base.Awake();
        GetComponent<Button>().onClick.AddListener(GameManager.Battle.OnPlayerAttack);
    }

 
}

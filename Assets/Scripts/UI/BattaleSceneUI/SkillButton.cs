using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : BattleMenuButton
{

    protected override void Awake()
    {
        base.Awake();
        GetComponent<Button>().onClick.AddListener(GameManager.Battle.OnPlayerUseSkill);
    }


}

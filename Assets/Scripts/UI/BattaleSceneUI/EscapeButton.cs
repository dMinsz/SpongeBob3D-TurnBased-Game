using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeButton : BattleMenuButton
{

    protected override void Awake()
    {
        base.Awake();


        GameManager.Scene.LoadScene("WorldScene");
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenuUI : BaseUI
{
    private void Update()
    {
        if (BattleManager.Instance.GetState() != BattleState.PLAYERTURN)
        {
            this.gameObject.SetActive(false);
        }
    }
}

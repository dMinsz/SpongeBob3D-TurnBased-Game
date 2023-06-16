using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{

    public Transform partyUI;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Battle Scene Init");
        
        //Debug
        //BattleManager.Instance.Setup();
    }

    protected override IEnumerator LoadingRoutine()
    {
        // fake loading
        Debug.Log("Battle Scene Road Somethings");
        GameManager.Battle.Init();
        progress = 0.2f;
        yield return new WaitForSecondsRealtime(0.2f);
        GameManager.Battle.Setup();
        progress = 0.4f;
        yield return new WaitForSecondsRealtime(0.2f);
        GameManager.Battle.SetingUI();
        progress = 0.6f;
        yield return new WaitForSecondsRealtime(0.2f);

        var cavans = GameObject.Find("BattleCanvas");
        var BattleUI = cavans.transform.Find("BattleUI");
        var _partyUI = BattleUI.transform.Find("PartyUI");
        partyUI = _partyUI;

        partyUI.GetComponent<PartyUI>().DataSetUp();
        progress = 0.8f;
        yield return new WaitForSecondsRealtime(0.2f);

        var players=GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in players)
        {
            p.GetComponent<PlayerMover>().enabled = false;
        }

        var Enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var e in Enemys)
        {
            e.GetComponent<FieldEnemy>().enabled = false;
        }

        progress = 1.0f;
    }

    private void OnDestroy()
    {
        Debug.Log("Battle Scene Release");
    }


    public override void Clear()
    {
        
    }

}

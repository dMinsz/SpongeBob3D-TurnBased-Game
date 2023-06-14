using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    #region Singleton
    private static BattleManager instance;
    public static BattleManager Instance 
    { 
        get 
        {
            return instance; 
        } 
    }

    private BattleManager() { }


    private void Awake() // Editor Inspector prevent Add 
    {
        if (instance != null)
        {
            Debug.LogWarning("Battle Manager Instance: valid instance already registered.");
            Destroy(this);
            return;
        }

        //DontDestroyOnLoad(this);

        instance = this;

        Setup();
    }
    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
    #endregion

    [SerializeField]private BattleState state;

    //public List<GameObject> playerPrefabs;
    //public List<GameObject> enemyPrefabs;

    public Transform[] playerBattleStation;
    public Transform[] enemyBattleStation;

    [HideInInspector] public PlayableDatas playerDatas;
    [HideInInspector] public EnemyDatas enemyDatas;

    List<Unit> playerUnits;
    List<Unit> enemyUnits;
    
    public void Setup()
    {
        playerUnits = new List<Unit>();

        state = BattleState.START;

        //Data Bind

        playerDatas = GameManager.Resource.Load<PlayableDatas>("Datas/PlayableDatas");
        enemyDatas = GameManager.Resource.Load<EnemyDatas>("Datas/EnemyDatas");

        //Init Enemy and Player
        foreach (var player in playerDatas.players) 
        {
            Player temp = new Player();
            temp.unitName = player.name;
            temp.MaxHP = player.MaxHP;
            temp.curHP = player.MaxHP;
            temp.MaxSP = player.MaxSP;
            temp.curSP = player.MaxSP;
            playerUnits.Add(temp);
        }



        //getComponent unit and Status set
    }
}

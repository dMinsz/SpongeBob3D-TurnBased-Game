using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{

    #region Singleton
    private static BattleManager instance;
    public static BattleManager Instance { get { return instance; } }

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

    public List<GameObject> playerPrefab;
    public List<GameObject> enemyPrefab;

    public Transform[] playerBattleStation;
    public Transform[] enemyBattleStation;

    List<Unit> playerUnit;
    List<Unit> enemyUnit;
    private void Setup()
    {
        state = BattleState.START;

        //TODO::

        //Init Enemy and Player

        //getComponent unit and Status set
    }
}

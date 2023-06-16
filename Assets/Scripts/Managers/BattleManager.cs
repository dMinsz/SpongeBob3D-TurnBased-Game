using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WAVE, WON, LOST }


public class Wave
{
    public List<EnemyDatas.EnemyInfo> EnemyList;
}
public class BattleManager : MonoBehaviour
{
    public bool isDebug = false;

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

        //for Debug
        if (isDebug == true)
        {
            Setup();
        }

    }
    private void OnDestroy()
    {

        if (instance == this)
            instance = null;
    }
    #endregion

    [SerializeField] private BattleState state = BattleState.START;

    public BattleState GetState() { return state; }

    public Transform[] playerBattleStation;
    public Transform[] enemyBattleStation;

    [HideInInspector] public PlayableDatas playerDatas;
    [HideInInspector] public EnemyDatas enemyDatas;

    [HideInInspector]  public List<Player> playerUnits;
    [HideInInspector] public List<Enemy> enemyUnits;

    public CinemachineVirtualCamera[] Cams;

    EnemyDatas.EnemyInfo encounteredUnit; 

    Enemy targetEnemy;
    [HideInInspector] public Player targetPlayer;

    [HideInInspector] public Player nowPlayer;

    //Monster Wave
    private const int MAXWAVE = 3; //for Check

    public int curentWave = 0;
    private int maxWaveCount = 2;
    
    private int MaxEachEnemyCount = 2;
    private int MaxTypeNum = 1; // Enemy Type


    //public WaveDatas waveData; 
    public List<Wave> _wave;

    Coroutine TurnRoutine;

    Transform MenuUI;
    Transform SkillUI;
    Transform EndUI;
    TMP_Text EndText;
    private void Start()
    {
        //Menu hud Open
        GameObject canvas = GameObject.Find("BattleCanvas");
        var BattleUI = canvas.transform.Find("BattleUI");
        MenuUI = BattleUI.transform.Find("SelectMenuUI");

        SkillUI = canvas.transform.Find("Skill");
        EndUI = canvas.transform.Find("EndBackGround");
        EndText = EndUI.GetComponentInChildren<TMP_Text>();

        MenuUI.gameObject.SetActive(true);
    }


    public void SettingBattle(BattleState _state, int _waveMaxCount , int _EachEnemyCount , int MaxEnemytype)
    {
        curentWave = 0;
        maxWaveCount = _waveMaxCount;
        MaxEachEnemyCount = _EachEnemyCount;
        MaxTypeNum = MaxEnemytype;

        state = _state;
      
        if (maxWaveCount > MAXWAVE)
        {
            maxWaveCount = MAXWAVE;
        }
    }

    #region cameraFuncs
    public void ChangeCamera(int index) 
    {
        foreach (var cam in Cams)
        {
            cam.Priority = 1;
        }

        Cams[index].Priority = 10;
    }
    #endregion

    #region Target Funcs
    public bool IsTargeting()
    {
        if (targetEnemy != null)
        {
            return true;
        }
        return false;
    }

    public void FreeTarget(Enemy target)
    {
        targetEnemy = null;
    }
    public void FreeTarget(Player target)
    {
        targetPlayer = null;
    }

    public void SetTaget(Enemy target)
    {
        targetEnemy = target;
    }

    public void SetTaget(Player target)
    {
        targetPlayer = target;
    }

    public Player GetTargetedPlayer()
    {
        return targetPlayer;
    }
    public Enemy GetTargetedEnemy()
    {
        return targetEnemy;
    }
    #endregion
    public void SetEnemysRotation()
    {
        foreach (var enemy in enemyUnits)
        {
            enemy.transform.LookAt(nowPlayer.transform.position);
        }
    }

    public void SetPlayersRotation()
    {
        foreach (var player in playerUnits)
        {
            player.transform.LookAt(targetEnemy.transform.position);
        }
    }

   
    public void MakeWave(int maxEnemyTypeNum, int maxEachEnemyCount)
    {

        if (curentWave > maxWaveCount)
        {
            Debug.Log("Make Enemy Too Much");
            return;
        }

        
        _wave = new List<Wave>();

        int curMonsterCount = 0;

        while (curMonsterCount < maxEachEnemyCount)
        {
            //test
            int EachWaveEnemyCount = Random.Range(1, maxEachEnemyCount+1);
            //int EachWaveEnemyCount = 3;
            int index = Random.Range(0, maxEnemyTypeNum + 1); //maxEnemyNum

            Wave temp = new Wave();
            temp.EnemyList = new List<EnemyDatas.EnemyInfo>();
            _wave.Add(temp);

            //_wave[curWave].EnemyList = new List<EnemyDatas.EnemyInfo>();

            for (int i = 0; i < EachWaveEnemyCount; i++)
            {
                _wave[curMonsterCount].EnemyList.Add(enemyDatas.Enemys[index]);
            }
            curMonsterCount++;
        }
        InitiateEnemys();
        curentWave++;
    }

    public void InitiateEnemys() 
    {
        for (int i = 0; i < _wave[0].EnemyList.Count; i++)
        {
            Enemy temp = GameManager.Resource.Instantiate(_wave[0].EnemyList[i].enemyPrefab, enemyBattleStation[i].position, Quaternion.identity);
            //temp.transform.position = enemyBattleStation[i].position;

            temp.unitName = _wave[0].EnemyList[i].name;
            temp.MaxHP = _wave[0].EnemyList[i].MaxHP;
            temp.MaxSP = _wave[0].EnemyList[i].MaxSP;
            temp.curHP = _wave[0].EnemyList[i].MaxHP;
            temp.curSP = _wave[0].EnemyList[i].MaxSP;
            temp.AttackDamage = _wave[0].EnemyList[i].AttackDamage;
            temp.SkillDamage = _wave[0].EnemyList[i].SkillDamage;

            temp.transform.LookAt(nowPlayer.transform.position);

            enemyUnits.Add(temp);
        }

        SetTaget(enemyUnits[0]);

        SetPlayersRotation();

        FreeTarget(enemyUnits[0]);
    }
    public void Setup()
    {
        //isSetUp = true;

        playerUnits = new List<Player>();
        enemyUnits = new List<Enemy>();

        //Data Bind

        playerDatas = GameManager.Resource.Load<PlayableDatas>("Datas/PlayableDatas");
        enemyDatas = GameManager.Resource.Load<EnemyDatas>("Datas/EnemyDatas");

        //Init Enemy and Player
        for (int i = 0; i < playerDatas.players.Length; i++)
        {
            Player temp = GameManager.Resource.Instantiate(playerDatas.players[i].PreFab, playerBattleStation[i].position, Quaternion.identity);
            //temp.transform.position = playerBattleStation[i].position;


            temp.unitName = playerDatas.players[i].name;
            temp.MaxHP = playerDatas.players[i].MaxHP;
            temp.curHP = playerDatas.players[i].MaxHP;
            temp.MaxSP = playerDatas.players[i].MaxSP;
            temp.curSP = playerDatas.players[i].MaxSP;
            temp.AttackDamage = playerDatas.players[i].AttackDamage;
            temp.SkillDamage = playerDatas.players[i].SkillDamage;
            playerUnits.Add(temp);
        }

        nowPlayer = playerUnits[0];
        SetTaget(playerUnits[0]);

        //Enemy Position and Data Setting
        MakeWave(MaxTypeNum, MaxEachEnemyCount);


        if (state == BattleState.START)
        {
            TurnRoutine = StartCoroutine(EnemyTurnRoutine());
        }
    }


    public Player NextPlayer() 
    {
        int index = playerUnits.FindIndex(x => x == nowPlayer);

        if (index < 0)
            Debug.Log("NextPlayer() Now Player Not Found");

        if (index < playerUnits.Count )
        {
            for (int i = index; i < playerUnits.Count ; i++)
            {
                if (index + 1 >= playerUnits.Count)
                {
                    return null;
                }

                if(!playerUnits[index + 1].IsDie())
                        return playerUnits[index + 1];
            }

            return null;
        }
        else 
        {
            return null;
        }
    }

    
    public Player AlivePlayer()
    {
        foreach (Player player in playerUnits)
        {
            if (player.IsDie() == false)
            {
                return player;
            }
        }

        return null;
    }


    public void OnPlayerAttack()
    {
       
        Debug.Log("Player Attack start");
        TurnRoutine = StartCoroutine(PlayerMoveAndAttack());
        MenuUI.gameObject.SetActive(false);
 
    }

    public void OnPlayerUseSkill() 
    {
        Debug.Log("Player Skill start");

        MenuUI.gameObject.SetActive(false);

        SkillUI.gameObject.SetActive(true);

        int index = playerUnits.FindIndex(x => x == nowPlayer);
        SkillUI.transform.GetComponentInChildren<Image>().sprite = playerDatas.players[index].SkillImage;


        TurnRoutine = StartCoroutine(PlayerMoveAndAttack());


    }

    //bool isPlayerAttackDone = false;
    IEnumerator PlayerMoveAndAttack() 
    {
        yield return new WaitForSeconds(0.5f);
        SkillUI.gameObject.SetActive(false);

        while (true)
        {
            Vector3 tempPos = nowPlayer.transform.position;

            if (targetEnemy == null)
            {
                Debug.Log("You Need Set Target");
                yield return null;
            }

            UnitMove(targetEnemy.transform.position, nowPlayer.transform);
            yield return new WaitForSeconds(0.2f);

            //enemy hp check and Remove
            var hp = targetEnemy.HP - nowPlayer.AttackDamage;

            if (hp <= 0)
            {
                enemyUnits.Remove(targetEnemy);
            }

            nowPlayer.Attack(nowPlayer.AttackDamage, targetEnemy);

            //Comback to origin pos
            UnitMove(tempPos, nowPlayer.transform);
            yield return new WaitForSeconds(0.2f);

            //isPlayerAttackDone = true;
            //MenuUI.gameObject.SetActive(true);
            Debug.Log("Player Attack Done");


            if (enemyUnits.Count <= 0)
            {
                if (curentWave < maxWaveCount)
                {
                    //wave Make
                    MakeWave(MaxTypeNum, MaxEachEnemyCount);
                }
                else 
                {
                    state = BattleState.WON;
                    EndBattle();
                }
            }

            //next Player
            Player next = NextPlayer();

            if (next != null)
            {
                nowPlayer = next;
                PlayerTurn();
                break;
            }
            else 
            {
                MenuUI.gameObject.SetActive(false);
                EnemyTurn();
                break;
            }
        }
       
    }

    public void PlayerTurn()
    {
        StopCoroutine(TurnRoutine);

        //Menu hud Open
        MenuUI.gameObject.SetActive(true);

        //Enemy Look Change
        SetEnemysRotation();

        //Player Cameara Setting

        int index = playerUnits.FindIndex(x => x == nowPlayer);

        ChangeCamera(index);

    }

    bool IsMovedone = false;

    void UnitMove(Vector3 target, Transform moveUnit)
    {
        while (true)
        {
            moveUnit.LookAt(target);
            Vector3 dir = (target - moveUnit.position).normalized;
            float speed = 1f;
            var attackRange = 2;

            moveUnit.transform.Translate(dir * speed * Time.deltaTime, Space.World);
            var distance = Vector3.Distance(target, moveUnit.position);


            if (attackRange > distance)
            {
                if (distance <= 0.1f)
                {
                    IsMovedone = true;
                    break;
                }
            }
        }
    }

    public void EnemyTurn()
    {
        StopCoroutine(TurnRoutine);

        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        Player temp = new Player();
        int hp = int.MaxValue;

        //Find hp Lower Player
        foreach (var player in playerUnits)
        {
            if (player.curHP < hp)
            {
                hp = player.curHP;
                temp = player;
            }

        }

        foreach (Enemy enemy in enemyUnits)
        {
            Vector3 tempPos = enemy.transform.position;

            UnitMove(temp.transform.position, enemy.transform);// Move To Player

            if (IsMovedone == true)
                IsMovedone = false;
            else
                continue;

            enemy.Attack(enemy.AttackDamage, temp);
            
            yield return new WaitForSeconds(0.2f);
            UnitMove(tempPos, enemy.transform);//Move Back Origin Pos

            enemy.transform.LookAt(temp.transform.position); // Re Look up Player

            if (IsMovedone == true)
                IsMovedone = false;
            else
                continue;

            yield return new WaitForSeconds(0.2f);
        }

        state = BattleState.PLAYERTURN;

        Player next = AlivePlayer();
        
        if (next != null) 
        { 
            nowPlayer = next;
            PlayerTurn();
        }
        else 
        {
            state = BattleState.LOST;
            EndBattle(); 
        }

        yield return null;
    }

    public void EndBattle()
    {
        Debug.Log("Battle Done");
        if (state == BattleState.WON)
        {
            EndUI.gameObject.SetActive(true);
            EndText.text = "You Win";
        }
        else if (state == BattleState.LOST)
        {
            EndUI.gameObject.SetActive(true);
            EndText.text = "You Lost";
        }
    }
}

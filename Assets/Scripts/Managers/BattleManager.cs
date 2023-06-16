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

    [SerializeField] private BattleState state = BattleState.START;

    public BattleState GetState() { return state; }

    public List<Transform> playerBattleStation;
    public List<Transform> enemyBattleStation;

    [HideInInspector] public PlayableDatas playerDatas;
    [HideInInspector] public EnemyDatas enemyDatas;

    [HideInInspector]  public List<Player> playerUnits;
    [HideInInspector] public List<Enemy> enemyUnits;

    public List<CinemachineVirtualCamera> Cams;

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

    public void SetingUI() 
    {
        //Menu hud Open
        GameObject canvas = GameObject.Find("BattleCanvas");
        var BattleUI = canvas.transform.Find("BattleUI");
        MenuUI = BattleUI.transform.Find("SelectMenuUI");

        SkillUI = canvas.transform.Find("Skill");
        EndUI = canvas.transform.Find("EndBackGround");
        EndText = EndUI.GetComponentInChildren<TMP_Text>();

        //MenuUI.gameObject.SetActive(true);
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

            temp.GetComponent<FieldEnemy>().enabled = false;

            enemyUnits.Add(temp);
        }

        SetTaget(enemyUnits[0]);

        SetPlayersRotation();

        FreeTarget(enemyUnits[0]);
    }
    private void Awake()
    {
        
    }

    public void Init()
    {

        playerBattleStation = new List<Transform>();


        var rootPlayerStations = GameObject.Find("PlayerStations");
        playerBattleStation.Add(rootPlayerStations.transform.Find("Player1"));
        playerBattleStation.Add(rootPlayerStations.transform.Find("Player2"));
        playerBattleStation.Add(rootPlayerStations.transform.Find("Player3"));
        playerBattleStation.Add(rootPlayerStations.transform.Find("Player4"));

        enemyBattleStation = new List<Transform>();

        var rootEnemyStations = GameObject.Find("EnemyStations");
        enemyBattleStation.Add(rootEnemyStations.transform.Find("Enemy1"));
        enemyBattleStation.Add(rootEnemyStations.transform.Find("Enemy2"));
        enemyBattleStation.Add(rootEnemyStations.transform.Find("Enemy3"));

        Cams = new List<CinemachineVirtualCamera>();

        Cams.Add(GameObject.Find("vcam1").transform.GetComponent<CinemachineVirtualCamera>());
        Cams.Add(GameObject.Find("vcam2").transform.GetComponent<CinemachineVirtualCamera>());
        Cams.Add(GameObject.Find("vcam3").transform.GetComponent<CinemachineVirtualCamera>());
        Cams.Add(GameObject.Find("vcam4").transform.GetComponent<CinemachineVirtualCamera>());



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


        if (state == BattleState.ENEMYTURN)
        {
            TurnRoutine = StartCoroutine(EnemyTurnRoutine());
        }
        else if (state == BattleState.PLAYERTURN)
        {
           PlayerTurn();
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
        //SkillUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            Vector3 tempPos = nowPlayer.transform.position;
            Vector3 tempEnemyPos = targetEnemy.transform.position;
            if (targetEnemy == null)
            {
                Debug.Log("You Need Set Target");
                yield return null;
            }

            nowPlayer.animator.SetTrigger("Attack");
            UnitMove(targetEnemy.transform.position, nowPlayer.transform);
            yield return new WaitForSeconds(1f);

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

            nowPlayer.transform.LookAt(tempEnemyPos);

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
        SkillUI.gameObject.SetActive(false);

        if (TurnRoutine != null)
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
                if (distance <= 1f)
                {
                    IsMovedone = true;
                    break;
                }
            }
        }
    }

    public void EnemyTurn()
    {
        SkillUI.gameObject.SetActive(false);
        StopCoroutine(TurnRoutine);

        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(1f);

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

            enemy.animator.SetTrigger("Attack");

            UnitMove(temp.transform.position, enemy.transform);// Move To Player
            yield return new WaitForSeconds(0.2f);
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

            //GameManager.Scene.LoadScene("WolrdScene");
        }
        else if (state == BattleState.LOST)
        {
            EndUI.gameObject.SetActive(true);
            EndText.text = "You Lost";

            //GameManager.Scene.LoadScene("WolrdScene");
        }

        EndUI.transform.Find("Button").GetComponent<Button>().onClick.AddListener(EndBattleSystem);

    }

    public void EndBattleSystem() 
    {
        playerUnits =null;
        enemyUnits = null;
        targetPlayer = null;
        nowPlayer = null;

        GameManager.Scene.LoadScene("WolrdScene");

    }
}

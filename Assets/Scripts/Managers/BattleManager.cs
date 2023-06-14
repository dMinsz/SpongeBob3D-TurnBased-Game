using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적은 최대 셋
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }


public class Wave
{
    public List<EnemyDatas.EnemyInfo> EnemyList;
}
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

        //Debug용
        Setup();
    }
    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
    #endregion

    [SerializeField]private BattleState state = BattleState.START;

    public Transform[] playerBattleStation;
    public Transform[] enemyBattleStation;
    
    [HideInInspector] public PlayableDatas playerDatas;
    [HideInInspector] public EnemyDatas enemyDatas;

    List<Unit> playerUnits;
    List<Unit> enemyUnits;

    EnemyDatas.EnemyInfo encounteredUnit; // 마주친 적

    //Monster Wave
    private const int MAXWAVE = 3;
    private int waveCount = 0;
    private int curWave = 0;

    public WaveDatas waveData; // 전체 웨이브데이터 저장용
    public List<Wave> _wave = new List<Wave>();
    //
    private bool isSetUp = false;

    //씬넘겨줄때 아래의 함수는 실행시켜줘야함
    public void BattleStart(BattleState _state, EnemyDatas.EnemyInfo _encounteredUnit , int _waveCount) 
    {
        state = _state;
        encounteredUnit = _encounteredUnit;

        if (_waveCount <= MAXWAVE)
        {
            waveCount = _waveCount;
        }
        else 
        {
            waveCount = MAXWAVE;
        }
    }

    //웨이브 만들기
    //error
    public void AddWave(int waveNum,int enemyNum) 
    {
        if (!isSetUp)
        {
            Debug.LogWarning("Error : BattleManager Setup is not done");
            return;
        }

        if (waveNum > MAXWAVE - 1)
        {
            Debug.LogWarning("Error : Invalid Wave Num");
            return;
        }

        //enemy 생성
        EnemyDatas.EnemyInfo tempEnemy;
        tempEnemy = enemyDatas.Enemys[enemyNum];

        
        if (tempEnemy == null)
        {//유요한 에너미 번호가 아닐때
            Debug.LogWarning("Error : BattleManager.AddWave() Enemy datas Null");
            return;
        }

        //웨이브 생성
        if (waveData.Waves.Count >= waveNum + 1 && waveData.Waves.Count != 0)
        {
            waveData.Waves[waveNum].EnemyList.Add(tempEnemy);
        }
        else
        {
            WaveDatas.WaveInfo tempWave = new WaveDatas.WaveInfo();
            tempWave.EnemyList.Add(tempEnemy);

            waveData.Waves.Add(tempWave);
        }

        waveCount++;

    }

    public void MakeWave(int maxWaveCount, int maxEnemyTypeNum , int maxEachEnemyCount)
    {
        //범위 에러 조심

        int curWave = 0;

        while (curWave < maxWaveCount) 
        {
            int EachWaveEnemyCount = Random.Range(1, maxEachEnemyCount+1);
            //int EachWaveEnemyCount = 2;
            int index = Random.Range(0, maxEnemyTypeNum); //maxEnemyNum 은 포함안함 초과임
            
            Wave temp = new Wave();
            temp.EnemyList = new List<EnemyDatas.EnemyInfo>();
            _wave.Add(temp);
            
            //_wave[curWave].EnemyList = new List<EnemyDatas.EnemyInfo>();

            for (int i = 0; i < EachWaveEnemyCount; i++)
            {
                _wave[curWave].EnemyList.Add(enemyDatas.Enemys[index]);
            }
            curWave++;
        }
    }

    public void Setup()
    {
        isSetUp = true;

        playerUnits = new List<Unit>();
        enemyUnits = new List<Unit>();

        //Data Bind

        playerDatas = GameManager.Resource.Load<PlayableDatas>("Datas/PlayableDatas");
        enemyDatas = GameManager.Resource.Load<EnemyDatas>("Datas/EnemyDatas");
        //waveData = GameManager.Resource.Load<WaveDatas>("Datas/WaveDatas");

        //debug 용
        if (encounteredUnit == null)
        {
            encounteredUnit = enemyDatas.enemys[0];
        }

        //wave Binding

        //Test Data
        MakeWave(2, 1 , 3);


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
            playerUnits.Add(temp);
        }


        //첫 웨이브만 만들어둔다

        for (int i = 0; i < _wave[0].EnemyList.Count; i++)
        {
            Enemy temp = GameManager.Resource.Instantiate(_wave[0].EnemyList[i].enemyPrefab, enemyBattleStation[i].position, Quaternion.identity);
            //temp.transform.position = enemyBattleStation[i].position;

            temp.unitName = _wave[0].EnemyList[i].name;
            temp.MaxHP = _wave[0].EnemyList[i].MaxHP;
            temp.MaxSP = _wave[0].EnemyList[i].MaxSP;
            temp.curHP = _wave[0].EnemyList[i].MaxHP;
            temp.curSP = _wave[0].EnemyList[i].MaxSP;

            enemyUnits.Add(temp);
        }

        curWave = 1;
    }
}

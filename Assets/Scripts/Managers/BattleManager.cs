using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �ִ� ��
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

        //Debug��
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

    EnemyDatas.EnemyInfo encounteredUnit; // ����ģ ��

    //Monster Wave
    private const int MAXWAVE = 3;
    private int waveCount = 0;
    private int curWave = 0;

    public WaveDatas waveData; // ��ü ���̺굥���� �����
    public List<Wave> _wave = new List<Wave>();
    //
    private bool isSetUp = false;

    //���Ѱ��ٶ� �Ʒ��� �Լ��� ������������
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

    //���̺� �����
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

        //enemy ����
        EnemyDatas.EnemyInfo tempEnemy;
        tempEnemy = enemyDatas.Enemys[enemyNum];

        
        if (tempEnemy == null)
        {//������ ���ʹ� ��ȣ�� �ƴҶ�
            Debug.LogWarning("Error : BattleManager.AddWave() Enemy datas Null");
            return;
        }

        //���̺� ����
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
        //���� ���� ����

        int curWave = 0;

        while (curWave < maxWaveCount) 
        {
            int EachWaveEnemyCount = Random.Range(1, maxEachEnemyCount+1);
            //int EachWaveEnemyCount = 2;
            int index = Random.Range(0, maxEnemyTypeNum); //maxEnemyNum �� ���Ծ��� �ʰ���
            
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

        //debug ��
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


        //ù ���̺길 �����д�

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyDatas", menuName = "Data/Enemy")]
public class MonsterDatas : ScriptableObject
{
    [SerializeField] public EnemyInfo[] enemys;
    public EnemyInfo[] Enemys { get { return enemys; } }

    [Serializable]
    public class EnemyInfo
    {
        [Header("Prefab")]
        public Enemy enemyPrefab;

        [Header("Status")]
        public string name;
        public int MaxHP;
        public int MaxSP;
    }

}

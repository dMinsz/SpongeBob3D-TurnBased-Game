using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyDatas;

[CreateAssetMenu(fileName = "WaveDatas", menuName = "Data/Wave")]
public class WaveDatas : ScriptableObject
{
    [SerializeField] public List<WaveInfo> waves = new List<WaveInfo>();
    public List<WaveInfo> Waves { get; set; }

    [Serializable]
    public class WaveInfo
    {
        [Header("Wave Monsters")]
        public List<EnemyInfo> EnemyList = new List<EnemyInfo>();
    }
}

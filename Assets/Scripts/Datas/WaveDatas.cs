using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyDatas;

[CreateAssetMenu(fileName = "WaveDatas", menuName = "Data/Wave")]
public class WaveDatas : ScriptableObject
{
    [SerializeField] public WaveInfo[] waves;
    public WaveInfo[] Waves { get { return waves; } }

    [Serializable]
    public class WaveInfo
    {
        [Header("Wave Monsters")]
        public List<EnemyInfo> Wave;
    }
}

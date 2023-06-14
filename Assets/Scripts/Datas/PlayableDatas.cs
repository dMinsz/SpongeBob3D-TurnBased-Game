using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayableDatas", menuName = "Data/PlayAble")]
public class PlayableDatas : ScriptableObject
{
    [SerializeField] public PlayableInfo[] players;
    public PlayableInfo[] Players { get { return players; }  }

    [Serializable]
    public class PlayableInfo
    {
        [Header("Prefab")]
        public Player player;

        public Sprite CharacterIamge;

        [Header("Status")]
        public string name;
        public int MaxHP;
        public int MaxSP;
    }
}

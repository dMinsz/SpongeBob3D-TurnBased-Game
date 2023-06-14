using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Unit : MonoBehaviour
{
    //public enum State { Alive, Die,}
    [SerializeField] public string unitName;
    [SerializeField] public int MaxHP;
    [HideInInspector] public int curHP;

    [SerializeField] public int MaxSP;
    [HideInInspector] public int curSP;

    private bool isDied = false;

    public bool IsDie() 
    {
        return isDied;
    }

    public abstract void Attack(int damage);
    public abstract void TakeDamage(int damage);
    public abstract void DoSkill();

}

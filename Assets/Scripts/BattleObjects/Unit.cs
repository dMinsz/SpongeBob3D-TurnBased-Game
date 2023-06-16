using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    //public enum State { Alive, Die,}
    [SerializeField] public string unitName;
    [SerializeField] public int MaxHP;
    [SerializeField] public int curHP;

    [SerializeField] public int MaxSP;
    [SerializeField] public int curSP;



    public int AttackDamage;
    public int SkillDamage;

    public int HP { get { return curHP; } protected set { curHP = value; OnHpChanged?.Invoke(curHP); } }
    public int SP { get { return curSP; } protected set { curSP = value; OnSpChanged?.Invoke(curSP); } }


    public UnityEvent<int> OnHpChanged;
    public UnityEvent<int> OnSpChanged;

    protected bool isDied = false;

    public virtual bool IsDie() 
    {
        return isDied;
    }

    public abstract void Attack(int damage , Unit target);
    public abstract void TakeDamage(int damage);
    public abstract void DoSkill(int damage, Unit target);

}

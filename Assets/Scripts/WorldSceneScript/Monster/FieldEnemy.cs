using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EnemyState;
using Unity.IO.LowLevel.Unsafe;

public class FieldEnemy : MonoBehaviour
{
    private Animator animator;
    private StateBaseFieldEnemy[] states;
    private StateEnemy curState;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        states = new StateBaseFieldEnemy[(int)StateEnemy.Size];
        states[(int)StateEnemy.Idle] = new IdleState(this);
        states[(int)StateEnemy.Patrol] = new PatrolState(this);
        states[(int)StateEnemy.Tracking] = new TrackingState(this);
    }

    private void Start()
    {
        curState = StateEnemy.Idle;

        // 네비게이션으로 순찰
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    public void ChangeState(StateEnemy state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
        states[(int)curState].Update();
    }
}

namespace EnemyState
{
    public enum StateEnemy { Idle, Patrol, Tracking, Size }

    public class IdleState : StateBaseFieldEnemy
    {
        private FieldEnemy fieldEnemy;

        public IdleState(FieldEnemy fieldEnemy)
        {
            this.fieldEnemy = fieldEnemy;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            
        }
    }

    public class PatrolState : StateBaseFieldEnemy
    {
        private FieldEnemy fieldEnemy;

        public PatrolState(FieldEnemy fieldEnemy)
        {
            this.fieldEnemy = fieldEnemy;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }

    public class TrackingState : StateBaseFieldEnemy
    {
        private FieldEnemy fieldEnemy;

        public TrackingState(FieldEnemy fieldEnemy)
        {
            this.fieldEnemy = fieldEnemy;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }
}

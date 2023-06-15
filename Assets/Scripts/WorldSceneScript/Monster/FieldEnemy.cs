using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EnemyState;
using Unity.IO.LowLevel.Unsafe;
using Unity.Burst.Intrinsics;
using UnityEngine.AI;
using System.Net;

public class FieldEnemy : MonoBehaviour
{
    [SerializeField] public Transform patrolPoint;
    [SerializeField] public Transform enemySpawnPoint;

    public NavMeshAgent agent;
    private Animator animator;
    private StateBaseFieldEnemy[] states;
    private StateEnemy curState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

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

        private float idleTime;

        public IdleState(FieldEnemy fieldEnemy)
        {
            this.fieldEnemy = fieldEnemy;
        }

        public override void Enter()
        {
            Debug.Log("Enter Idle");
            idleTime = 0;
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;

            if (idleTime > 3)
            {
                idleTime = 0;
                fieldEnemy.ChangeState(StateEnemy.Patrol);
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit Idle");
        }
    }

    public class PatrolState : StateBaseFieldEnemy
    {
        private FieldEnemy fieldEnemy;

        private Transform curPoint;

        public PatrolState(FieldEnemy fieldEnemy)
        {
            this.fieldEnemy = fieldEnemy;
        }

        public override void Enter()
        {
            Debug.Log("Enter Patrol");

            curPoint = Vector3.Distance(fieldEnemy.transform.position, fieldEnemy.patrolPoint.position) > Vector3.Distance(fieldEnemy.transform.position, fieldEnemy.enemySpawnPoint.position) ? 
            fieldEnemy.patrolPoint : fieldEnemy.enemySpawnPoint;
            fieldEnemy.agent.destination = curPoint.position;
        }

        public override void Update()
        {
            if (Vector3.Distance(fieldEnemy.transform.position, curPoint.position) < 0.5f)
            {
                fieldEnemy.ChangeState(StateEnemy.Idle);
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit Patrol");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EnemyState;
using UnityEngine.AI;
using System;

public class FieldEnemy : MonoBehaviour
{
    [SerializeField] public Transform patrolPoint;
    [SerializeField] public Transform enemySpawnPoint;
    [SerializeField] public float trackingSpeed;

    [Header("StateMachine")]
    [NonSerialized] public NavMeshAgent agent;
    [NonSerialized] public Animator animator;
    private StateBaseFieldEnemy[] states;
    private StateEnemy curState;

    [Header("FieldOfView")]
    [SerializeField] float range;
    [SerializeField, Range(0, 360)] float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    [NonSerialized] public bool foundPlayer;

    public void FindPlayer()
    {
        // 1. 범위 안에 있는지
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetMask);
        foreach (Collider collider in colliders)
        {
            // 2. 앞에 있는지
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad)) // Dot은 내적
            {                                                                             // 호도법      
                foundPlayer = false;
                continue;
            }
            // 3. 중간에 장애물이 없는지
            float distToTarget = Vector3.Distance(transform.position, collider.transform.position);
            if (Physics.Raycast(transform.position, dirTarget, distToTarget, obstacleMask))
            {
                foundPlayer = false;
                continue;
            }

            foundPlayer = true;
            Debug.DrawRay(transform.position, dirTarget * distToTarget, Color.red);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);
        Debug.DrawRay(transform.position, rightDir * range, Color.red);
        Debug.DrawRay(transform.position, leftDir * range, Color.red);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

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
        FindPlayer();
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

            if (fieldEnemy.foundPlayer)
                fieldEnemy.ChangeState(StateEnemy.Tracking);
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

            curPoint = Vector3.Distance(fieldEnemy.transform.position, fieldEnemy.patrolPoint.position) 
                > Vector3.Distance(fieldEnemy.transform.position, fieldEnemy.enemySpawnPoint.position) ? 
                fieldEnemy.patrolPoint : fieldEnemy.enemySpawnPoint;
            fieldEnemy.agent.destination = curPoint.position;
        }

        public override void Update()
        {
            if (Vector3.Distance(fieldEnemy.transform.position, curPoint.position) < 0.5f)
            {
                fieldEnemy.ChangeState(StateEnemy.Idle);
            }

            if (fieldEnemy.foundPlayer)
                fieldEnemy.ChangeState(StateEnemy.Tracking);
        }

        public override void Exit()
        {
            Debug.Log("Exit Patrol");
        }
    }

    public class TrackingState : StateBaseFieldEnemy
    {
        private FieldEnemy fieldEnemy;

        private GameObject player;

        public TrackingState(FieldEnemy fieldEnemy)
        {
            this.fieldEnemy = fieldEnemy;
        }

        IEnumerator TrackingRoutine()
        {
            while (true)
            {
                fieldEnemy.transform.LookAt(player.transform);
                fieldEnemy.transform.Translate(Vector3.forward * fieldEnemy.trackingSpeed * Time.deltaTime, Space.Self);

                if (Vector3.Distance(player.transform.position, fieldEnemy.transform.position) < 1f)
                {
                    fieldEnemy.animator.SetTrigger("Attack");
                }
                yield return null;
            }
        }

        public override void Enter()
        {
            Debug.Log("Enter Tracking");
            fieldEnemy.agent.destination = fieldEnemy.transform.position;
            player = GameObject.FindGameObjectWithTag("Player");

            StartCoroutine(TrackingRoutine);
        }

        public override void Update()
        {
            if (!fieldEnemy.foundPlayer)
                fieldEnemy.ChangeState(StateEnemy.Idle);
        }

        public override void Exit()
        {
            Debug.Log("Exit Tracking");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField, Range(0, 306)] float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;

    [SerializeField] UnityEvent DiscoveryPlayer;
    [SerializeField] UnityEvent NotFoundPlayer;

    private void Update()
    {
        FindTarget();
    }

    public void FindTarget()
    {
        // 1. 범위 안에 있는지
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetMask);
        foreach (Collider collider in colliders)
        {
            // 2. 앞에 있는지
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad)) // Dot은 내적
                continue;                                                                     // 호도법

            // 3. 중간에 장애물이 없는지
            float distToTarget = Vector3.Distance(transform.position, collider.transform.position);
            if (Physics.Raycast(transform.position, dirTarget, distToTarget, obstacleMask))
                continue;

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
}

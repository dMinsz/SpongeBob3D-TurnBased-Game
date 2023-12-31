using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] bool debug;

    [SerializeField] Transform point;
    [SerializeField] float range;

    private Animator animator;

    private void Update()
    {
        
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(point.position, range);

        foreach (Collider collider in colliders)
        {
            if(collider.tag != "Enemy" && collider.tag != "Player")
            {
                IInteractable interactable = collider.GetComponent<IInteractable>();
                interactable?.Interact();
            }
        }
    }

    private void OnInteract(InputValue value)
    {
        Debug.Log("상호작용 키 누름");
        Interact();
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void AttackInteract()
    {
        Collider[] colliders = Physics.OverlapSphere(point.position, range);

        foreach (Collider collider in colliders)
        {
            if (collider.tag != "Enemy")
                continue;

            IHittable hittable = collider.GetComponent<IHittable>();
            hittable?.TakeHit();
        }
    }

    private void OnAttack(InputValue value)
    {
        AttackInteract();
        Attack();
    }

    private void OnDrawGizmosSelected()
    {
        if(!debug) 
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(point.position, range);
    }
}

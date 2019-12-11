using System;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public GameObject player;
    public float pursuitDistance;
    public float attackRange;
    public float attackDamage;
    
    // Capitalized to match names in Unity
    // This method is more efficient than using strings to look up names
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Die");

    private Animator animator;
    private NavMeshAgent agent;
    private Damageable damageable;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        damageable = player.GetComponent<Damageable>();
    }

    private void Update()
    {
        if (!agent.enabled) 
            return;

        float dist = Vector3.Distance(player.transform.position, transform.position);
        bool attack = false;
        bool pursue = dist < pursuitDistance;

        if (pursue)
        {
            if (dist < attackRange)
            {
                attack = true;
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.SetDestination(transform.position);
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }

        animator.SetBool(Attack, attack);
        animator.SetBool(Run, pursue);
    }

    public void AttackEvent()
    {
        damageable.Damage(attackDamage);
    }

    public void Die()
    {
        agent.enabled = false;
        
        animator.SetBool(Walk, false);
        animator.SetBool(Run, false);
        animator.SetBool(Attack, false);
        animator.SetTrigger(Death);
        
        Destroy(gameObject);  // TODO
    }
}

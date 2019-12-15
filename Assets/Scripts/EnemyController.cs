using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public float pursuitDistance;
    public float attackRange;
    public float attackDamage;
    public Transform[] patrol;

    // Capitalized to match names in Unity
    // This method is more efficient than using strings to look up names
    private static readonly int Stand = Animator.StringToHash("Stand");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Die");

    private Animator animator;
    private NavMeshAgent agent;
    private Damageable playerHealth;

    private int nextIndex;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerHealth = player.GetComponent<Damageable>();
        
        NextPoint();
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
                agent.SetDestination(transform.position);
                animator.SetTrigger(Attack);
            }
            else
            {
                agent.SetDestination(player.transform.position);
                animator.SetTrigger(Run);
            }
        }
        
        bool patrolling = !attack && !pursue && patrol.Length > 0;
        if (patrolling && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            NextPoint();
            animator.SetTrigger(Walk);
        }
        else
        {
            animator.SetTrigger(Stand);
        }
    }

    private void NextPoint()
    {
        if (patrol.Length <= 0)
            return;

        agent.destination = patrol[nextIndex].position;

        nextIndex++;
        nextIndex %= patrol.Length;
    }

    public void AttackEvent()
    {
        playerHealth.Damage(attackDamage);
    }

    public void Die()
    {
        agent.enabled = false;
        animator.SetTrigger(Death);
        Destroy(gameObject);  // TODO
    }
}

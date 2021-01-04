using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public GameObject bullet;
    public Transform bulletSpawn;

    public LayerMask isGround;
    public LayerMask isPlayer;

    //Patrol
    private Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange;
    private bool playerInSightRange;

    public float attackRange;
    private bool playerInAttackRange;

    //Attacking
    public float timeToAttack;
    private bool alreadyAttacked;

    private Rigidbody rb;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        Physics.IgnoreLayerCollision(9, 10);
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerInAttackRange && !playerInSightRange)
            Patroling();

        if (!playerInAttackRange && playerInSightRange)
            HuntPlayer();

        if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SetWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToPoint = transform.position - walkPoint;
        if (distanceToPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SetWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
            walkPointSet = true;
    }

    private void HuntPlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody _rb = Instantiate(bullet, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            _rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
            _rb.AddForce(transform.up * 3f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeToAttack);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            rb.velocity = Vector3.zero;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : DamageHandler
{
    public EnemyStateMachineController stateMachine;
    public GameObject teleportPrefab;
    public int maxHealth = 500;
    public float visionRange = 50f;
    public float visionAngle = 180f;

    [SerializeField]
    protected Player player;
    protected NavMeshAgent agent;
    protected bool _destinationLocked = false; //To store if Enemy has locked onto player
    protected bool _patrol = true;
    protected float stuckTimer = 0f;
    
    [SerializeField]
    private Transform[] _patrolCheckpoints; // Patrol points
    private int _patrolCheckpoint = 0; // Index tracks current patrol point
    private bool _stunned = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        stateMachine = new EnemyStateMachineController(this);
        stateMachine.ChangeState(new EnemyPatrolState());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.Update();
        IfStuck();
    }

    protected virtual void IfStuck()
    {
        if (agent.velocity.magnitude < 0.1f)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        if (stuckTimer > 5f)
        {
            // Find random position nearby
            Vector3 randomDirection = Random.insideUnitSphere * 5.0f;
            randomDirection += transform.position;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, 5.0f, NavMesh.AllAreas))
            {
                // Move agent
                agent.Warp(navHit.position + new Vector3(0, 1.5f, 0));
                var warpEffect = Instantiate(teleportPrefab, agent.transform.position + new Vector3(0, -1.7f, 0), Quaternion.identity);
                Destroy(warpEffect, 1f);
            }
            stuckTimer = 0f;
        }
    }

    public virtual bool CanSeePlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > visionRange)
            {
                return false;
            }

            float verticalVisionLimit = 90f;
            float verticalAngle = Mathf.Asin((player.transform.position.y - transform.position.y) / distanceToPlayer) * Mathf.Rad2Deg;
            float horizontalAngle = Vector3.Angle(transform.forward, directionToPlayer);

            if (horizontalAngle > visionAngle / 2f && Mathf.Abs(verticalAngle) > verticalVisionLimit)
            {
                return false;
            }

            // Exclude Enemy layer
            var layerExclusion = ~LayerMask.GetMask("Enemy"); 

            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, visionRange, layerExclusion))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public void Stun(float duration)
    {
        if (_stunned) return; // Stop stuns stacking
        _stunned = true;

        // Disable
        agent.isStopped = true;

        // Start the stun timer
        StartCoroutine(StunDuration(duration));
    }

    private IEnumerator StunDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Re enable
        agent.isStopped = false;
        _stunned = false;
    }

    public bool GetDestinationLocked()
    {
        return _destinationLocked;
    }

    public void setDestinationLocked(bool lockState)
    {
        _destinationLocked = lockState;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return agent;
    }

    public int GetPatrolCheckpoint()
    {
        return _patrolCheckpoint;
    }

    public void SetPatrolCheckpoint(int checkpoint)
    {
        _patrolCheckpoint = checkpoint;
    }

    public Transform[] GetPatrolCheckPoints()
    {
        return _patrolCheckpoints;
    }

    public Player GetPlayer()
    {
        return player;
    }
}

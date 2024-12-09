using UnityEngine;
using UnityEngine.AI;

public class EnemySearchState : IEnemyState
{
    private Enemy _enemy;
    private Vector3 _searchPosition;
    private bool _foundDestination = false;
    private float _patrolStateChance = 0.01f;

    public void EnterState(Enemy enemy)
    {
        this._enemy = enemy;
        PickRandomPosition();
        _enemy.GetNavMeshAgent().updateRotation = true;
    }

    public void UpdateState()
    {
        if (_enemy.CanSeePlayer())
        {
            _enemy.stateMachine.ChangeState(new EnemyChaseState());
            return;
        }

        // Introduce probabilistic/stochastic state transition
        if (Random.value < _patrolStateChance)
        {
            _enemy.stateMachine.ChangeState(new EnemyPatrolState());
            return;
        }

        if (_foundDestination && !_enemy.GetNavMeshAgent().pathPending && _enemy.GetNavMeshAgent().remainingDistance <= _enemy.GetNavMeshAgent().stoppingDistance)
        {
            _foundDestination = false;
        }

        if (!_foundDestination)
        {
            PickRandomPosition();
        }
    }

    //mimics searching for player
    protected void PickRandomPosition()
    {
        float radius = 8.0f;
        // Maximum attempts
        int attempts = 10;

        for (int i = 0; i < attempts; i++)
        {
            // Generate random direction within radius
            Vector2 randomDirection = Random.insideUnitCircle * radius;
            Vector3 newPosition = _enemy.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);

            // Check if random position is on NavMesh
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(newPosition, out navHit, radius, NavMesh.AllAreas))
            {
                // Distance is far enough - avoids jitter
                if (Vector3.Distance(_enemy.transform.position, navHit.position) > 4.0f)
                {
                    _searchPosition = navHit.position;
                    _enemy.GetNavMeshAgent().SetDestination(_searchPosition);
                    _foundDestination = true;
                    return;
                }
            }
        }

        _searchPosition = _enemy.transform.position;
        _foundDestination = false;
    }
}

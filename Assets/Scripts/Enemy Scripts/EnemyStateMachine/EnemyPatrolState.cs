using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IEnemyState
{
    private Enemy _enemy;
    private int _checkPoint;
    private Transform[] _checkPointList;

    public void EnterState(Enemy enemy)
    {
        this._enemy = enemy;
        this._checkPointList = _enemy.GetPatrolCheckPoints();
    }

    public void UpdateState()
    {
        if (_enemy.CanSeePlayer())
        {
            _enemy.stateMachine.ChangeState(new EnemyChaseState());
        }
        Patrol();
    }

    private void Patrol()
    {
        NavMeshAgent agent = _enemy.GetNavMeshAgent();
        _checkPoint = _enemy.GetPatrolCheckpoint();

        // Continue patrolling if no player lock on
        if (!_enemy.GetDestinationLocked() && agent.remainingDistance < 0.1f)
        {
            // Move to next checkpoint. Return to start at the end
            _enemy.SetPatrolCheckpoint((_checkPoint + 1) % _checkPointList.Length);
            agent.SetDestination(_checkPointList[_checkPoint].position);
        }
    }
}

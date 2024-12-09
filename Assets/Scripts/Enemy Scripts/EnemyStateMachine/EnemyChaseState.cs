using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IEnemyState
{
    private Enemy _enemy;
    private ShroudedEnemy _shroudedEnemy;

    public void EnterState(Enemy enemy)
    {
        if (enemy is ShroudedEnemy)
        {
            this._shroudedEnemy = (ShroudedEnemy)enemy;
        }
        else if (enemy is CryoEnemy)
        {
            this._enemy = enemy;
        }
    }

    public void UpdateState()
    {
        if (_enemy != null && _enemy.CanSeePlayer() && _enemy is CryoEnemy)
        {
            CryoChasePlayer();
        } else if(_enemy != null && !_enemy.CanSeePlayer() && _enemy is CryoEnemy)
        {
            _enemy.setDestinationLocked(false);
            _enemy.stateMachine.ChangeState(new EnemySearchState());
        }
        
        if (_shroudedEnemy != null && _shroudedEnemy.CanSeePlayer())
        {
            ShroudedChasePlayer();
        } else if(_shroudedEnemy != null && !_shroudedEnemy.CanSeePlayer())
        {
            _shroudedEnemy.setDestinationLocked(false);
            _shroudedEnemy.stateMachine.ChangeState(new EnemySearchState());
        }
    }

    private void CryoChasePlayer()
    {
        NavMeshAgent agent = _enemy.GetNavMeshAgent();

        _enemy.setDestinationLocked(true);

        agent.updateRotation = false;

        Vector3 directionToPlayer = (_enemy.GetPlayer().transform.position - _enemy.transform.position).normalized;

        directionToPlayer.y = 0;
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            float rotationSpeed = 5f;
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        agent.SetDestination(_enemy.GetPlayer().transform.position);
    }

    private void ShroudedChasePlayer()
    {
        NavMeshAgent agent = _shroudedEnemy.GetNavMeshAgent();

        _shroudedEnemy.setDestinationLocked(true);

        float distanceToPlayer = Vector3.Distance(_shroudedEnemy.transform.position, _shroudedEnemy.GetPlayer().transform.position);

        agent.updateRotation = false;

        if (distanceToPlayer > _shroudedEnemy.GetTempestRange())
        {
            agent.SetDestination(_shroudedEnemy.GetPlayer().transform.position);
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;

        }

        Vector3 directionToPlayer = (_shroudedEnemy.GetPlayer().transform.position - _shroudedEnemy.transform.position).normalized;

        //avoid tilting on the y axis
        directionToPlayer.y = 0;

        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            float rotationSpeed = 100f;
            _shroudedEnemy.transform.rotation = Quaternion.Slerp(_shroudedEnemy.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        if (distanceToPlayer <= _shroudedEnemy.GetTempestRange())
        {
            _shroudedEnemy.stateMachine.ChangeState(new ShroudedAttackState());
        }
    }

}

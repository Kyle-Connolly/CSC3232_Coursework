using UnityEngine;

public abstract class EnemyAttackState : IEnemyState
{
    protected Enemy _enemy;
    protected float _distanceToPlayer;

    public virtual void EnterState(Enemy enemy)
    {
        this._enemy = enemy;
    }

    public abstract void UpdateState();
}
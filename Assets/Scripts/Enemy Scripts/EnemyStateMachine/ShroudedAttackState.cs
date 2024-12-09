using UnityEngine;

public class ShroudedAttackState : EnemyAttackState
{
    private ShroudedEnemy _shroudedEnemy;

    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        if (enemy is ShroudedEnemy)
        {
            _shroudedEnemy = (ShroudedEnemy)enemy;
        }
    }

    public override void UpdateState()
    {
        if (_shroudedEnemy == null || _shroudedEnemy.GetPlayer() == null)
        {
            return;
        }

        _distanceToPlayer = Vector3.Distance(_shroudedEnemy.transform.position, _shroudedEnemy.GetPlayer().transform.position);

        if (!_shroudedEnemy.CanSeePlayer())
        {
            _shroudedEnemy.setDestinationLocked(false);
            _shroudedEnemy.stateMachine.ChangeState(new EnemySearchState());
            return;
        }

        if (_distanceToPlayer > _shroudedEnemy.GetTempestRange())
        {
            _shroudedEnemy.stateMachine.ChangeState(new EnemyChaseState());
            return;
        }

        if (_distanceToPlayer <= _shroudedEnemy.GetTempestRange() && _shroudedEnemy.GetCharges() > 0)
        {
            ShroudedAttack();
        }
    }

    public void ShroudedAttack()
    {
        _shroudedEnemy.SetCurrentCharges(-1);
        Vector3 target = _shroudedEnemy.GetPlayer().transform.position;
        
        CreateProjectile(target);
    }


    public void CreateProjectile(Vector3 target)
    {
        var projectile = GameObject.Instantiate(_shroudedEnemy.GetTempestProjectile(), _shroudedEnemy.GetProjectileOrigin().position, Quaternion.identity);
        Rigidbody body = projectile.GetComponent<Rigidbody>();

        // Calculate direction + velocity
        Vector3 projectilePosition = _shroudedEnemy.GetProjectileOrigin().position;
        Vector3 targetDistance = target - projectilePosition;
        float distance = targetDistance.magnitude;

        // Get height difference
        float heightDiff = target.y - projectilePosition.y;

        // Set initial launch angle
        float angle = Mathf.Deg2Rad * 20;
        float gravity = Physics.gravity.y;
        float velocityMagnitude = Mathf.Sqrt(distance * Mathf.Abs(gravity) / Mathf.Sin(2 * angle));

        // Launch velocity vector
        Vector3 horizontalDirection = new Vector3(targetDistance.x, 0, targetDistance.z).normalized;
        Vector3 launchVelocity = horizontalDirection * velocityMagnitude * Mathf.Cos(angle);
        launchVelocity.y = velocityMagnitude * Mathf.Sin(angle);

        // Calculated velocity to tempest projectile rigid body
        body.velocity = launchVelocity;
    }
}
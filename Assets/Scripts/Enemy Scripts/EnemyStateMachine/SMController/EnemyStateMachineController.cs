public class EnemyStateMachineController
{
    private IEnemyState currentState;
    private Enemy enemy;

    public EnemyStateMachineController(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState = newState;
        currentState.EnterState(enemy);
    }

    public void Update()
    {
        currentState?.UpdateState();
    }
}

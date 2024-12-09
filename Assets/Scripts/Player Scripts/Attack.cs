using System.Collections;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public Camera cameraView;
    public GameObject tempestProjectile;
    public Transform projectileOrigin;
    public float meleeAttackRange = 2f;
    public float tempestRange = 15f;
    public float tempestSpeed = 30f;

    protected float _lastAttackTimer = 0f;
    protected float _timeSinceAttack = 5.0f;

    [SerializeField]
    protected GameManager _gameManager;

    private Vector3 _target;
    private string _playerLastAttack;

    public bool MeleeAttack(string attackName, int chargeCount, LayerMask layer)
    {
        AttackType currentAttack = ValidateAttackRequest(attackName, chargeCount, layer);
        
        // check that the current attack is set and is a registered attack in the AttackSet
        if ((currentAttack.Equals(new AttackType())))
        {
            return false;
        }

        // Check if enough charges are available
        if (!CheckCharge(chargeCount, currentAttack))
        {
            return false;
        }

        // Detect enemies in range
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, meleeAttackRange, layer);
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Vector3 hitPosition = enemy.transform.position;

                    int damage = comboMove(currentAttack, currentAttack.damage);
                    _lastAttackTimer = _timeSinceAttack;
                    setLastAttack(currentAttack.name);

                    enemy.TakeDamage(damage);

                    _gameManager.UpdatePlayerScore(10);

                    //Knockback effect only if player is using Inferno attack
                    if (attackName.Equals("Inferno"))
                    {
                        Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
                        UnityEngine.AI.NavMeshAgent enemyAgent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
                        enemyAgent.GetComponent<Rigidbody>().isKinematic = false;
                        // Stop NavMesh Agent to be able to knock back
                        enemyAgent.isStopped = true;
                        // Knockback direction
                        Vector3 knockDirection = (enemy.transform.position - transform.position).normalized;
                        // Apply knockback
                        float knockForce = 100f;
                        enemyRigidbody.AddForce(knockDirection * knockForce, ForceMode.Impulse);
                        // Re-enable NavMesh Agent
                        enemy.StartCoroutine(ResumeAgent(enemyAgent, enemyRigidbody, knockDirection, knockForce, 0.5f));
                    } 

                    return true;
                }
            }
        }
        return true;
    }

    private IEnumerator ResumeAgent(UnityEngine.AI.NavMeshAgent agent, Rigidbody rBody, Vector3 direction, float force, float delay)
    {
        float timer = 0f;

        while (timer < delay)
        {
            timer += Time.deltaTime;

            // Ensure Rigidbody moves in correct direction
            rBody.AddForce(direction * force * 0.2f, ForceMode.Force);

            yield return null;
        }

        // Stop velocity 
        rBody.velocity = Vector3.zero;

        agent.GetComponent<Rigidbody>().isKinematic = true;

        //re-enable agent
        agent.isStopped = false;
    }

    public bool RangedAttack(string attackName, int chargeCount, LayerMask layer)
    {
        AttackType currentAttack = ValidateAttackRequest(attackName, chargeCount, layer);
        
        // check that the current attack is set and is a registered attack in the AttackSet
        if ((currentAttack.Equals(new AttackType())))
        {
            return false;
        }

        
        // Check if enough charges are available
        if (!CheckCharge(chargeCount, currentAttack))
        {
            return false;
        }

        Ray ray = cameraView.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            _target = rayHit.point;
        }
        else
        {
            _target = ray.GetPoint(1000);
        }

        CreateProjectile();
        return true;
    }

    public void CreateProjectile()
    {
        var projectile = Instantiate(tempestProjectile, projectileOrigin.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody>().velocity = (_target - projectileOrigin.position).normalized * tempestSpeed;
    }

    public AttackType ValidateAttackRequest(string attackName, int chargeCount, LayerMask layer)
    {
        // Switch expression to set currentAttack
        AttackType currentAttack = attackName switch
        {
            "Inferno" => AttackSet.Attacks["Inferno"],
            "Hydro" => AttackSet.Attacks["Hydro"],
            "Tempest" => AttackSet.Attacks["Tempest"],
            _ => new AttackType()
        };

        return currentAttack;
    }

    public bool CheckCharge(int chargeCount, AttackType currentAttack)
    {
        // Check if enough charges are available
        if (chargeCount < currentAttack.elementalChargeCost)
        {
            return false;
        }

        return true;
    }

    // Visualise attack range (for debugging purposes)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * meleeAttackRange);
        Gizmos.DrawWireSphere(transform.position + transform.forward * meleeAttackRange, 0.5f);
    }

    private int comboMove(AttackType currentAttack, int currentAttackDamage)
    {
        if(_playerLastAttack != null && _playerLastAttack.Equals("Hydro") && currentAttack.name.Equals("Inferno") && _lastAttackTimer > 0f)
        {
            currentAttackDamage += 50;
            _gameManager.UpdateComboCount(2);
            _gameManager.UpdatePlayerScore(20);
            
            return currentAttackDamage;
        }

        return currentAttackDamage;
    }

    private void setLastAttack(string currentAttack)
    {
        _playerLastAttack = currentAttack;
    }
}

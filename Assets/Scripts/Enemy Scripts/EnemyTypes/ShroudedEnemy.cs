using UnityEngine;

public class ShroudedEnemy : Enemy
{
    public int damage = 20; //damage inflicted to player

    [SerializeField]
    public LayerMask targetLayer;
    public GameObject tempestProjectile;
    public float tempestSpeed = 15.0f; // Projectile speed
    public Transform projectileOrigin;

    [SerializeField]
    private float tempestRange = 30f;

    private string _currentAttack;
    private int _currentCharges = 1; // 1 charge max
    private int _chargeLimit = 1; // Maximum tempest charge count
    private float _elementalRechargeTime = 1f; // Generate charge every 1.5 seconds
    private float _chargeTimer = 0f; // Timer to track time since last charge gen
    [SerializeField]
    private int _currentHealth;
    private Vector3 _target;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        GenerateCharges();
    }

    private void GenerateCharges()
    {
        if (Time.time >= _chargeTimer + _elementalRechargeTime && _currentCharges < _chargeLimit)
        {
            _currentCharges++;
            _chargeTimer = Time.time;
        }
    }

    public override int GetHealth()
    {
        return _currentHealth;
    }

    public override void SetHealth(int value)
    {
        _currentHealth = value;
    }

    public float GetTempestRange()
    {
        return tempestRange;
    }

    public int GetCharges()
    {
        return _currentCharges;
    }

    public void SetCurrentCharges(int change)
    {
        _currentCharges += change;
    }

    public LayerMask GetTargetLayer()
    {
        return targetLayer;
    }

    public GameObject GetTempestProjectile()
    {
        return tempestProjectile;
    }


    public Transform GetProjectileOrigin()
    {
        return projectileOrigin;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tempestRange); 
    }
}

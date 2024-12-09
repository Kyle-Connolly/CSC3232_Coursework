using UnityEngine;

public class CryoEnemy : Enemy
{
    private int _damage = 10; // Damage inflicted to player on contact
    private float damageTime = 0f;
    private float _damageTick = 1f; // To damage the player for every tick (one sec per tick)
    private Player _contact;
    [SerializeField]
    private int _currentHealth;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            _contact = player;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_contact != null)
        {
            damageTime += Time.deltaTime;
            if (damageTime >= _damageTick)
            {
                _contact.TakeDamage(_damage);
                damageTime = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset everything for next contact
        _contact = null;
        damageTime = 0f;

    }

    public override int GetHealth()
    {
        return _currentHealth;
    }

    public override void SetHealth(int value)
    {
        _currentHealth = value;
    }
}

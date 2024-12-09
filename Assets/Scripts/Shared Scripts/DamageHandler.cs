using UnityEngine;

public abstract class DamageHandler : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        int currentHealth = GetHealth();
        currentHealth -= damage;
        SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public abstract int GetHealth();
    public abstract void SetHealth(int value);
}

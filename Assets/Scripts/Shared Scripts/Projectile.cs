using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactPrefab;
    public int tempestDamage = 75;

    private bool _collided;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(tempestDamage);
                enemy.Stun(2.0f);
                _gameManager.UpdatePlayerScore(10);
            }
        }

        if (collision.CompareTag("Player") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(tempestDamage);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Tempest" && collision.gameObject.tag != "Player" && !_collided)
        {
            _collided = true;
            var impact = Instantiate(impactPrefab, collision.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
    }
}

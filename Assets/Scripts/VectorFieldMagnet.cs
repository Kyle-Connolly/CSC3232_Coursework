using System.Collections;
using UnityEngine;

public class MagneticAttraction : MonoBehaviour
{
    public Transform attractionPoint;  // Centre of attraction zone
    public float magnetForce = 100f;  // Attraction strength

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tempest"))
        {
            // Attracting Tempest projectile
            StartCoroutine(MagnetCatch(other.gameObject));
        }
    }

    private IEnumerator MagnetCatch(GameObject projectile)
    {
        while (projectile != null)
        {
            Vector3 centreDirection = (attractionPoint.position - projectile.transform.position).normalized;
            float distance = Vector3.Distance(attractionPoint.position, projectile.transform.position);
            // Magnet force
            projectile.GetComponent<Rigidbody>().AddForce(centreDirection * magnetForce / distance, ForceMode.Acceleration);
            // Destroy projectile if in range of centre
            if (distance < 1f)
            {
                Destroy(projectile);
                break;
            }
            yield return null;
        }
    }
}



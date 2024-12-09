using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterEntrance : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) 
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
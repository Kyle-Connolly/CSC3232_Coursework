using System.Collections;
using UnityEngine;

public class LavaEvent : MonoBehaviour
{
    public int damage = 10; 
    public Material lavaMaterial;
    public GameObject ground;
    public float eventDuration = 10f;
    private bool _eventOver = false;
    private bool _eventActive = false;
    private Material _ogMaterial;
    private Renderer _renderer; // Floor renderer reference

    void Start()
    {
        _renderer = ground.GetComponent<Renderer>();
        if (_renderer != null)
        {
            _ogMaterial = _renderer.material;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !_eventActive && !_eventOver)
        {
            StartCoroutine(StartLavaEvent());
        }
    }

    private IEnumerator StartLavaEvent()
    {
        _eventActive = true;
        
        _renderer.material = lavaMaterial;
        
        float timeElapsed = 0f;

        while (timeElapsed < eventDuration)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (_renderer != null)
        {
            // Revert floor material
            _renderer.material = _ogMaterial;
        }

        _eventActive = false;
        _eventOver = true;
    }
}


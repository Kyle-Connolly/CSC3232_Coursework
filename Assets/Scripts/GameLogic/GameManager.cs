using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private BoxCollider _invisGateCollider;
    [SerializeField]
    private BoxCollider _telporterCollider;

    [SerializeField]
    private float _colliderDistance = 100f; // How much the invisible gate collider will move by to allow the player through
    [SerializeField]
    private float _colliderSpeed = 7f; // How fast the collider will move
    [SerializeField]
    private Light[] _lights; // Lights turn on when gate (collider) lowers
    [SerializeField]
    private GameObject _endGameUI;
    [SerializeField]

    private Vector3 _colliderStartingPosition;
    private List<Enemy> _startingEnemies = new List<Enemy>();
    private bool _gameOver;

    // Get the initial state
    void Start()
    {
        if (_invisGateCollider != null)
        {
            _colliderStartingPosition = _invisGateCollider.transform.position;
        }

        if(_telporterCollider)
        {
            _telporterCollider.enabled = false;
        }

        foreach (ShroudedEnemy shrouded in FindObjectsOfType<ShroudedEnemy>())
        {
            _startingEnemies.Add(shrouded);
        }
        foreach (CryoEnemy cryo in FindObjectsOfType<CryoEnemy>())
        {
            _startingEnemies.Add(cryo);
        }

        foreach (var light in _lights)
        {
            if (light != null)
                light.enabled = false;
        }

        if (_endGameUI != null)
        {
            _endGameUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.GetHealth() <= 0)
        {
            PlayerDied();

            if (Input.GetKeyDown(KeyCode.Return) && _gameOver)
            {
                ResetGame();
            }
        }
        
        if (!_gameOver)
        {
            if (CheckEnemyStatus())
            {
                LowerGate();
            }
        }
    }

    private bool CheckEnemyStatus()
    {
        bool winCondition = true;

        foreach (CryoEnemy cryo in FindObjectsOfType<CryoEnemy>())
        {
            if (cryo != null)
            {
                winCondition = false;
                break;
            }
        }

        foreach (ShroudedEnemy shrouded in FindObjectsOfType<ShroudedEnemy>())
        {
            if (shrouded != null)
            {
                winCondition = false;
                break;
            }
        }

        return winCondition;
    }

    private void LowerGate()
    {
        foreach (var light in _lights)
        {
            if (light != null)
                light.enabled = true;
        }

        _telporterCollider.enabled = true;

        Vector3 targetPosition = _invisGateCollider.center + new Vector3(0, -_colliderDistance, 0);

        while (Vector3.Distance(_invisGateCollider.center, targetPosition) > 0.01f)
        {
            _invisGateCollider.center = Vector3.MoveTowards(
                _invisGateCollider.center,
                targetPosition,
                _colliderSpeed * Time.deltaTime
            );
        }
    }

    public void PlayerDied()
    {
        _gameOver = true;
        _endGameUI.SetActive(true);
    }

    public void ResetGame()
    {
        //reload the scene to begin a new level/round
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdatePlayerScore(int scoreAmount)
    {
        _player.increaseScore(scoreAmount);
    }

    public void UpdateComboCount(int comboVal)
    {
        _player.setComboCount(comboVal);
    }

    public void resetPlayerCombo()
    {
        _player.resetComboCount();
    }
}

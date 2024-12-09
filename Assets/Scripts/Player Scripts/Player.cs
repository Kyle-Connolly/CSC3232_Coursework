using UnityEngine;

public class Player : DamageHandler
{
    [Header("Player Settings")]
    public int maxHealth = 500;
    public int score = 1;

    private int _currentHealth;
    private int _comboCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
    }

    public int getComboCount()
    {
        return _comboCount;
    }

    public void setComboCount(int comboVal)
    {
        _comboCount = comboVal;
    }

    public void resetComboCount()
    {
        _comboCount = 0;
    }

    public int getScore()
    {
        return score;
    }

    public void increaseScore(int scoreAmount)
    {
        score += scoreAmount;
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

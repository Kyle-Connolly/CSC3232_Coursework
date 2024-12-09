using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI chargesText;
    public TextMeshProUGUI attackTypeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboCounter;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private PlayerAttackManager _attackManager;

    // Update is called once per frame
    void Update()
    {
        healthText.text = $"{_player.GetHealth()}%";

        string elementalCharges = new string('-', _attackManager.getCurrentChargeCount()) + new string(' ', 10 - _attackManager.getCurrentChargeCount());
        chargesText.text = elementalCharges;


        attackTypeText.text = _attackManager.getCurrentAttack().ToUpper();
        if (attackTypeText.text.Equals("HYDRO"))
        {
            attackTypeText.color = new Color(0f, 182f / 255f, 241f / 255f);

        }
        if (attackTypeText.text.Equals("INFERNO"))
        {
            attackTypeText.color = new Color(241f / 255f, 72f / 255f, 0f);

        }
        if (attackTypeText.text.Equals("TEMPEST"))
        {
            attackTypeText.color = new Color(147f / 255f, 0f, 254f / 255f);

        }

        scoreText.text = _player.getScore().ToString();

        var comboCountertext = _player.getComboCount();
        comboCounter.text = $"x{comboCountertext}";
    }
}

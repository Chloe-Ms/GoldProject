using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdatePlayModeCharacter : MonoBehaviour
{
    [SerializeField] Image _heroHead;
    [SerializeField] TextMeshProUGUI _health;
    [SerializeField] TextMeshProUGUI _maxHealth;

    public void Init(Hero hero)
    {
        _maxHealth.text = hero.MaxHealth.ToString();
        _health.text = hero.MaxHealth.ToString();
    }
    public void UpdateHealth(int health)
    {
        _health.text = health.ToString();  
    }
}

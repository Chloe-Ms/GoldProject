using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "GoldProject/Create Hero", order = 1)]
public class HeroData : ScriptableObject
{
    public int maxHealth;
    public string heroName;
    public Role role;
    [ResizableTextArea]
    public string description;
    public RuntimeAnimatorController animatorController;
    public GameObject atlas;
    public Sprite sprite;
    public Sprite headSprite;
    public Color color;
    public string _soundDamage;
    public string _soundDeath;
    public string atlasTroncName;
}

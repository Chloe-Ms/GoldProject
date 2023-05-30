/*using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeroData))]
public class HeroDataEditor : Editor
{
    private HeroData heroData;
    SerializedObject serializedObject;
    SerializedProperty propMaxHealth;
    SerializedProperty propHeroName;
    SerializedProperty propRole;
    SerializedProperty propISUnlocked;
    private void OnEnable()
    {
        heroData = target as HeroData;

        if (heroData.Effects == null)
        {
            heroData.Effects = new SerializedDictionary<Effect, int>();
            foreach (Effect effectType in Enum.GetValues(typeof(Effect)))
            {
                if (!heroData.Effects.Contains(effectType))
                {
                    heroData.Effects.Add(effectType, 0);
                }
            }
        }
    }
    public override void OnInspectorGUI()
    {
        heroData.heroName = EditorGUILayout.TextField("Hero name", heroData.heroName);
        heroData.maxHealth = EditorGUILayout.IntField("Max Health", heroData.maxHealth);
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Role");
            heroData.role = (Role)EditorGUILayout.EnumPopup(heroData.role);
        }
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Is Unlocked");
            heroData.IsUnlocked = EditorGUILayout.Toggle(heroData.IsUnlocked);
        }

        GUILayout.Label("Effects sensibility",EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            heroData.Effects.dictionary.ForEach(effect =>
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    effect.Value = EditorGUILayout.IntField(effect.Key.ToString(), effect.Value);
                }
            });
        }
    }
}
*/
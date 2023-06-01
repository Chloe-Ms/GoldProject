using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroesSensibilities", menuName = "GoldProject/Heroes Sensibilities", order = 1)]
public class HeroesSensibility : ScriptableObject
{
    public int[] heroesSensibilities;
    private bool modified = false;

    int GetSensibility (Effect effect,Role role)
    {
        return heroesSensibilities[(int)effect + ((int)role * Enum.GetNames(typeof(Effect)).Length)];
    }

    private void OnEnable()
    {
        if (!modified)
        {
            int columns = Enum.GetNames(typeof(Effect)).Length;
            int rows = Enum.GetNames(typeof(Role)).Length;
            heroesSensibilities = new int[columns* rows];
            modified = true;
            
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    heroesSensibilities[i + (j * columns)] = 0;
                }
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap List", menuName = "GoldProject/Trap List", order = 4)]
public class TrapList : ScriptableObject
{
    [SerializeField] private List<TrapData> _trapData = new List<TrapData>();

    public List<TrapData> TrapData
    {
        get { return _trapData; }
    }

    public Color GetColorFromEffect(Effect effect)
    {
        int i = 0;
        Color color = Color.white;
        bool found = false;
        while (i < _trapData.Count && !found)
        {
            if (_trapData[i].Effect == effect)
            {
                color = _trapData[i].Color;
                found = true;
            }
            i++;
        }
        return color;
    }
    public Sprite GetSpriteFromEffect(Effect effect)
    {
        int i = 0;
        Sprite sprite = null;
        while (i < _trapData.Count && sprite == null)
        {
            if (_trapData[i].Effect == effect)
            {
                sprite = _trapData[i].Sprite;
            }
            i++;
        }
        return sprite;
    }

    public Sprite GetImageBgFromEffect(Effect effect)
    {
        int i = 0;
        Sprite bgImage = null;
        while (i < _trapData.Count && bgImage == null)
        {
            if (_trapData[i].Effect == effect)
            {
                bgImage = _trapData[i].BgEffectUIImage;
            }
            i++;
        }
        return bgImage;
    }

    public Sprite GetSpriteMonsterFromEffect(Effect effect)
    {
        int i = 0;
        Sprite sprite = null;
        while (i < _trapData.Count && sprite == null)
        {
            if (_trapData[i].Effect == Effect.MONSTRE)
            {
                sprite = _trapData[i].RoomMonsterImages[(int)effect];
            }
            i++;
        }
        return sprite;
    }
}

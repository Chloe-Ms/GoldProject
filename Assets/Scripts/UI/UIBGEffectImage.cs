using UnityEngine;
using UnityEngine.UI;

public class UIBGEffectImage : MonoBehaviour
{
    [SerializeField] Image _spriteBgEffectUI;

    [SerializeField] GeneralData _generalData;

    void Start()
    {
        GameManager.Instance.OnEffectApplied += OnEffectApplied;
    }

    private void OnEffectApplied(Effect obj)
    {
        _spriteBgEffectUI.sprite = _generalData.TrapList.GetImageBgFromEffect(obj);
        if (_spriteBgEffectUI.sprite == null)
        {
            _spriteBgEffectUI.color = new Color(1,1,1,0);
        } else
        {
            _spriteBgEffectUI.color = new Color(1, 1, 1, 1);
        }
    }
}

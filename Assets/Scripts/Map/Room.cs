using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using UnityEditor;

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] private RoomData _roomData;
    [SerializeField] private TrapData _trapData;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private UIUpgradeButton _upgradeIcon;
    [SerializeField] private SpriteRenderer _borderRenderer;
    [SerializeField] private ParticleSystem _particleSystemSpawn;
    private SpriteRenderer _iconRenderer;
    [SerializeField] private SpriteRenderer _spriteTopMonster;
    private GameObject _icon;
    [SerializeField] float _iconScale = 1f;
    [HorizontalLine]
    [SerializeField] private GameObject _layerSelection;
    private Tween _layerSelectionTween;
    [SerializeField] private float _offsetMovementLayerSelection = 5f;
    [SerializeField] private float _timeMovementLayerSelection = 0.5f;
    [SerializeField] private GeneralData _generalData;
    [SerializeField] private float _scaleSelection = 1.15f;
    [SerializeField] private float _scaleFade = 0.5f;
    [SerializeField] private float _timeAfterFade = 0.1f;
    private Tween _tweenSelection;
    private Tween _tweenFade;
    private Coroutine _routineFade;
    public RoomData RoomData
    {
        get { return _roomData; }
        set { _roomData = value; }
    }

    public TrapData TrapData
    {
        get { return _trapData; }
        set { _trapData = value; }
    }

    public SpriteRenderer SpriteRenderer
    {
        get { return _spriteRenderer; }
        set { _spriteRenderer = value; }
    }

    private bool _isSelected;
    [ShowNonSerializedField] private RoomColor _roomColor = RoomColor.NotBuyable;

    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }

    public RoomColor RoomColor
    {
        get { return _roomColor; }
        set {
            _roomColor = value;
            switch (_roomColor)
            {
                case RoomColor.Buyable:
                    _spriteRenderer.color = Color.white;
                    break;
                case RoomColor.NotBuyable:
                    _spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
                    break;
                case RoomColor.Usable:
                    _spriteRenderer.color = Color.white;
                    break;
                case RoomColor.Selected:
                    _spriteRenderer.color = Color.yellow;
                    break;
                case RoomColor.Unclickable:
                    _spriteRenderer.color = Color.white;
                    break;
            }
        }
    }

    [ShowNonSerializedField] private RoomColor _oldState;

    List<Effect> _listEffects = new List<Effect>();
    bool _isActive = true;
    int _nbOfUsage = 0;
    int _nbOfUpgrades = 0;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public List<Effect> Effects
    {
        get => _listEffects;
        private set => _listEffects = value;
    }
    public int NbOfUpgrades
    {
        get => _nbOfUpgrades;
        set => _nbOfUpgrades = value;
    }

    public bool IsElementary
    {
        get { return _listEffects.Count > 0 && 
                (_listEffects[0] == Effect.FOUDRE || _listEffects[0] == Effect.FEU || _listEffects[0] == Effect.GLACE); }
    }
    public UIUpgradeButton UpgradeIcon { 
        get => _upgradeIcon;
    }
    public int NbOfUsage { 
        get => _nbOfUsage; 
        set => _nbOfUsage = value; 
    }
    public float IconScale { 
        get => _iconScale;
    }
    public GameObject LayerSelection { 
        get => _layerSelection; 
        set => _layerSelection = value; 
    }

    public void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        RoomColor = RoomColor.NotBuyable;
        _oldState = _roomColor;
        //_layerSelection.SetActive(false);
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        CreateIconObject();
        _oldState = _roomColor;
    }
    
    private void CreateIconObject()
    {
        float offsetZ = 0.10f;

        _icon = new GameObject();
        _icon.name = transform.name + "_Icon";
        _icon.transform.parent = transform;
        _icon.transform.localPosition = new Vector3(0, 0, -offsetZ);
        _iconRenderer = _icon.AddComponent<SpriteRenderer>();
        _iconRenderer.color = new Color(0, 0, 0, 0);
        _iconRenderer.sortingOrder = 3;
    }

    public void SetData(RoomData roomData)
    {
        SetColor(RoomColor.Usable);
        _roomData = roomData;
        //Debug.Log($"RoomName = {transform.name} data = {roomData}");
        if (_trapData == null || _trapData.RoomType != RoomType.BOSS) //set sprite boss room
        {
            SetSprite(_roomData.Sprite);
        }
    }

    public void SetData(TrapData trapData)
    {
        SetColor(RoomColor.Usable);
        _trapData = trapData;
        _listEffects.Clear();
        _listEffects.Add(trapData.Effect);
        StopSelectionAnimation();
        transform.localScale = Vector3.zero;
        SetIcon(trapData.Sprite);
        _tweenFade = transform.DOScale(1f, _scaleFade).SetEase(Ease.OutBounce).
            OnComplete(() => StartSelectionAnimation());
    }

    public void StartSelectionAnimationRoutine()
    {
        _tweenFade = null;
        //yield return new WaitForSeconds(_timeAfterFade);
        StartSelectionAnimation();
    }

    public void SetData(RoomData roomData, TrapData trapData)
    {
        SetColor(RoomColor.Usable);
        _roomData = roomData;
        _trapData = trapData;
        SetIcon(_trapData.Sprite);
        if (trapData.RoomType == RoomType.BOSS) //set sprite boss room
        {
            SetSprite(_generalData.SpriteBossRoom);
        } else
        {
            SetSprite(_roomData.Sprite);
        }
    }

    public void SetData(RoomData roomData, RoomColor roomColor = RoomColor.Unclickable)
    {
        SetColor(roomColor);
        _roomData = roomData;
        SetSprite(_roomData.Sprite);
    }

    public void SetData(Sprite sprite, RoomColor roomColor = RoomColor.Unclickable)
    {
        SetColor(roomColor);
        SetSprite(sprite);
    }

    public void SetData(RoomData roomData, TrapData trapData, RoomColor roomColor = RoomColor.NotBuyable)
    {
        SetColor(roomColor);
        _roomData = roomData;
        _trapData = trapData;
        SetIcon(_trapData.Sprite);
        SetSprite(_roomData.Sprite);
    }

    public void ClearIcon()
    {
        if (_iconRenderer == null)
        {
            _iconRenderer = _icon.GetComponent<SpriteRenderer>();
        }
        if (_trapData != null && _trapData.RoomType != RoomType.ENTRANCE && _trapData.RoomType != RoomType.BOSS)
        {
            //Debug.Log("CLEAR "+ _trapData.RoomType);
            _iconRenderer.color = new Color(255, 255, 255, 0);
            _iconRenderer.sprite = null;
            _icon.transform.localScale = Vector2.one;
            _spriteTopMonster.gameObject.SetActive(false);
        }
    }

    public void SetIconEffect()
    {
        if (_iconRenderer == null)
        {
            _iconRenderer = _icon.GetComponent<SpriteRenderer>();
        }
        if (_trapData != null)
        {
            _iconRenderer.color = new Color(255, 255, 255, 255);
            if (_trapData.Effect != Effect.MONSTRE || _nbOfUpgrades == 0)
            {
                _iconRenderer.sprite = _trapData.RoomEffectImage;
            } else
            {
                _iconRenderer.sprite = _generalData.TrapList.GetSpriteMonsterFromEffect(_listEffects[1]);
            }
            if (!_trapData.IsRoomEffectImageBehindHeroes)
            {
                _iconRenderer.sortingOrder = 41;
            }
        }
    }

    public void SetIconEffectAnimated()
    {
        if (_trapData != null)
        {
            _icon.GetComponent<SpriteRenderer>().DOFade(1, 1f);
            _icon.GetComponent<SpriteRenderer>().sprite = _trapData.RoomEffectImage;
        }
    }

    public void UndoData(TrapData trapData)
    {
        _trapData = trapData;
        if (trapData == null) {
            RoomColor = RoomColor.NotBuyable;
            SetIcon(null);
            SetSprite(GameManager.Instance.GeneralData.Square);
        }
        else {
            RoomColor = RoomColor.Usable;
            SetIcon(trapData.Sprite);
        }
        EnableUpgrade();
    }

    public void UndoData(RoomData roomData, TrapData trapData)
    {
        _roomData = roomData;
        _trapData = trapData;
        if (roomData == null || trapData == null) {
            RoomColor = RoomColor.NotBuyable;
            SetIcon(null);
            SetSprite(GameManager.Instance.GeneralData.Square);
        }
        else {
            RoomColor = RoomColor.Usable;
            SetIcon(trapData.Sprite);
            SetSprite(roomData.Sprite);
        }
        EnableUpgrade();
    }

    public void UndoData(RoomData roomData, TrapData trapData, RoomColor roomColor = RoomColor.NotBuyable)
    {
        _roomData = roomData;
        _trapData = trapData;
        RoomColor = roomColor;
        _oldState = roomColor;
        SetIcon(trapData == null ? null : trapData.Sprite);
        SetSprite(roomData == null ? GameManager.Instance.GeneralData.Square : roomData.Sprite);
        EnableUpgrade();
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetIcon(Sprite sprite)
    {
        _icon.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        _icon.GetComponent<SpriteRenderer>().sprite = sprite;
        if (_trapData != null && (_trapData.RoomType == RoomType.NORMAL || _trapData.RoomType == RoomType.LEVER))
        {
            _icon.transform.localScale = new Vector2(_iconScale, _iconScale);
        }
    }

    public void SetColor(RoomColor color)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        if (color != RoomColor.Selected)
            _oldState = color != _oldState ? _roomColor : _oldState;
        RoomColor = color;
    }

    public void SetColor(RoomColor color, RoomColor oldColor)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        RoomColor = color;
        _oldState = oldColor;
    }

    public bool IsInBound(Vector2 mousePosition)
    {
        Vector3 size = transform.localScale;
        Vector3 position = transform.position;

        float left = position.x - size.x / 2;
        float right = position.x + size.x / 2;
        float top = position.y + size.y / 2;
        float bottom = position.y - size.y / 2;

        return mousePosition.x > left && mousePosition.x < right && mousePosition.y > bottom && mousePosition.y < top;
    }

    public void UnSelect()
    {
        RoomColor = _oldState;
        _upgradeIcon.gameObject.SetActive(false);
        StopSelectionAnimation();
        if (_roomData == null) {
            if (MapManager.Instance.SelectedSlot == null)
                RoomColor = RoomColor.NotBuyable;
            _oldState = RoomColor.NotBuyable;
        }
    }

    public void EnableUpgrade()
    {
        if (NbOfUpgrades == 0 && _trapData != null && MapManager.Instance.IsRoomATrap(this) 
            && MapManager.Instance.BuyableRoomCount > 0 && MapManager.Instance.IsUpgradable && MapManager.Instance.SelectedSlot == this)
        {
            _upgradeIcon.gameObject.SetActive(true);
        } else
        {
            _upgradeIcon.gameObject.SetActive(false);
        }
    }

    public void PlayParticles()
    {
        var module = _particleSystemSpawn.main;
        module.startColor = Color.white;
        _particleSystemSpawn.Play();
    }
    public void UpgradeRoom()
    {
        _nbOfUpgrades++;
        EnableUpgrade();
        _borderRenderer.color = new Color(1f, 1f, 1f, 1f);
        var module = _particleSystemSpawn.main;
        module.startColor = Color.yellow;
        _particleSystemSpawn.Play();
    }

    public void UpgradeRoom(Effect effect)
    {
        UpgradeRoom();
        //Create sprite on top (depending on color of effect chosen)
        _spriteTopMonster.color = _generalData.TrapList.GetColorFromEffect(effect);
        _spriteTopMonster.gameObject.SetActive(true);
    }

    public void UndoUpgrade()
    {
        _nbOfUpgrades--;
        _borderRenderer.color = new Color(1f, 1f, 1f, 0f);
        if (_trapData.Effect == Effect.MONSTRE && _listEffects.Count > 1)
        {
            _listEffects.RemoveAt(_listEffects.Count - 1);
            SetIcon(_trapData.Sprite);
            _spriteTopMonster.gameObject.SetActive(false);

        }
        EnableUpgrade();
    }

    public bool IsBuyable()
    {
        return _roomColor != RoomColor.Usable && _roomColor != RoomColor.Unclickable;
    }

    public bool IsNotBuy()
    {
        return _roomColor == RoomColor.Buyable && _roomColor != RoomColor.Unclickable;
    }

    public bool IsClickable()
    {
        return _roomColor != RoomColor.NotBuyable && _roomColor != RoomColor.Unclickable;
    }

    public bool IsUsable()
    {
        return _roomColor == RoomColor.Usable || _oldState == RoomColor.Usable;
    }

    public void StartLayerSelectionAnimation()
    {
        _layerSelection.SetActive(true);
        _layerSelectionTween = _layerSelection.transform.DOMoveY(_layerSelection.transform.position.y + _offsetMovementLayerSelection,
            _timeMovementLayerSelection).SetLoops(-1,LoopType.Yoyo);
    }

    public void StopLayerSelectionAnimation()
    {
        if (_layerSelectionTween != null)
        {
            _layerSelectionTween.Kill();
            _layerSelection.SetActive(false);
            _layerSelectionTween = null;
        }
    }

    public void StartSelectionAnimation()
    {
        if (_tweenSelection == null)
        {
            _tweenSelection = transform.DOScale(new Vector3(_scaleSelection, _scaleSelection, 1f), 1f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            _tweenSelection.Restart();
        }
    }

    public void StopSelectionAnimation()
    {
        if (_tweenFade != null)
        {
            _tweenFade.Complete();
        }
        if (_tweenSelection != null)
        {
            _tweenSelection.Rewind();
        }
    }

}

public enum RoomColor
{
    Buyable = 0,
    NotBuyable = 1,
    Usable = 2,
    Unclickable = 3,
    Selected = 4
}
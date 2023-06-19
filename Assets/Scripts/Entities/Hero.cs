using DG.Tweening;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour 
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _damageTextMesh;
    Animator _animator;
    private HeroData _heroData;
    private int _health;
    private bool _isDead = false;
    private bool _isinvulnerable = false;
    private int _nbDamageOnElementaryRoom = 0; //Pas de meilleur endroit ou le mettre :/
    private bool _hasDamageReduction = false;

    private bool _canMove;

    public event Action<Hero> OnHeroDeath;
    public event Action<int> OnDamageTaken;
    
    #region Properties
    public string HeroName
    {
        get => _heroData.heroName;
        set => _heroData.heroName = value;
    }
    public int MaxHealth
    {
        get => _heroData.maxHealth;
        set => _heroData.maxHealth = value;
    }

    public Sprite HeadSprite
    {
        get => _heroData.headSprite;
    }

    public Role Role 
    {
        get => _heroData.role;
        set => _heroData.role = Role;
    }
    public bool IsDead { 
        get => _isDead; 
        set => _isDead = value; 
    }
    public int Health {
        get => _health; 
        set => _health = value; 
    }
    public bool IsInvulnerable {
        get => _isinvulnerable; 
        set => _isinvulnerable = value; 
    }
    public int NbDamageOnElementaryRoom { 
        get => _nbDamageOnElementaryRoom; 
        set => _nbDamageOnElementaryRoom = value; 
    }
    public bool HasDamageReduction {
        get => _hasDamageReduction;
        set => _hasDamageReduction = value;
    }
    public bool CanMove { get => _canMove; set => _canMove = value; }
    #endregion
    public void TestDamage()
    {
        UpdateHealth(1);
    }
    public void UpdateHealth(int pv)
    {
        if (IsDead || IsInvulnerable)
        {
            return;
        }

        int realPV = pv;
        if (pv < 0) //DAMAGE
        {
            realPV = Mathf.Max(-_health, realPV);
            if (Role == Role.MAGE && GameManager.Instance.IsCurrentRoomElementary)
            {
                _nbDamageOnElementaryRoom++;
            }
        } else { //HEAL
            realPV = Mathf.Min(MaxHealth - _health, realPV);
        }

        _health = Mathf.Clamp(_health + realPV, 0,MaxHealth);
        if (_health <= 0)
        {
            _isDead = true;
            _canMove = false;
            if (_heroData._soundDeath != "")
            {
                AudioManager.Instance.Play(_heroData._soundDeath);
            }
            if (_animator != null)
            {
                _animator.SetTrigger("IsDead");
            }
            OnHeroDeath?.Invoke(this);
            this.transform.parent = null;
        } else
        {
            if (_heroData._soundDamage != "")
            {
                AudioManager.Instance.Play(_heroData._soundDamage);
            }
            if (_animator != null)
            {
                _animator.SetTrigger("IsHurt");
            }
            OnDamageTaken?.Invoke(realPV);
        }
        //InstantiateDamage(realPV);
        UIUpdatePlayMode.Instance.UpdateHero(this,realPV);
    }

    public void LoadHeroData(HeroData data)
    {
        _heroData = data;
        //_renderer.sprite = _heroData.sprite;
        _health = _heroData.maxHealth;
        GameObject atlas = null;
        if (_heroData.atlas != null)
        {
            atlas = Instantiate(_heroData.atlas, this.transform);
            //Transform atlasTronc = atlas.transform.Find(_heroData.atlasTroncName);
            if (atlas != null)
            {
                _animator = atlas.AddComponent<Animator>();
                _animator.runtimeAnimatorController = _heroData.animatorController;
            } else
            {
                Debug.LogWarning($"{_heroData.atlasTroncName} not found");
            }
        }
    }

    public void IsRunningInAnimator(bool isRunning)
    {
        if (_animator != null)
        {
            _animator.SetBool("IsRunning", isRunning);
        }
    }

    public void InstantiateDamage(int realPV)
    {
        if (_canvas != null)
        {
            GameObject go = Instantiate(_damageTextMesh, _canvas.transform);
            go.transform.parent = _canvas.transform;
            go.transform.localPosition = Vector3.zero;
            TextMeshProUGUI textMesh = go.GetComponent<TextMeshProUGUI>();
            if (textMesh != null)
            {
                textMesh.text = realPV.ToString();
                Vector3 pos = Camera.main.ScreenToViewportPoint(textMesh.transform.position);
                textMesh.transform.position = pos;
                textMesh.transform.DOMoveY(pos.y+10,1f).OnComplete(() => Destroy(textMesh.gameObject));
            }
            /*//then you calculate the position of the UI element
//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

Vector2 ViewportPosition=Cam.WorldToViewportPoint(WorldObject.transform.position);
Vector2 WorldObject_ScreenPosition=new Vector2(
((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));

//now you can set the position of the ui element
UI_Element.anchoredPosition=WorldObject_ScreenPosition;*/
        }
    }
}

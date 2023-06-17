using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class TestDotween : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    void Start()
    {

        _target.transform.DOMove(Vector3.zero, 3).SetAutoKill(false);
    }

    [Button]
    public void Rewind()
    {
        _target.transform.DOSmoothRewind();
    }

    [Button]
    public void StopAll()
    {
        DOTween.RestartAll();
    }
}

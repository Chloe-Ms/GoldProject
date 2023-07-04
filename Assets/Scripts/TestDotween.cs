using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class TestDotween : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    void Start()
    {

        StartCoroutine(Routine());
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

    public IEnumerator Routine()
    {
        Debug.Log("BEGIN ROUTINE");
        yield return StartCoroutine(RoutineWaited());
        Debug.Log("END ROUTINE");
    }

    public IEnumerator RoutineWaited()
    {
        Debug.Log("BEGIN ROUTINE WAITED");
        yield return new WaitForSeconds(2f);
        Debug.Log("END ROUTINE WAITED");
    }
}

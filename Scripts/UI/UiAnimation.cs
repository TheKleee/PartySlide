using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public enum Type
{
    scaleIn,
    scaleLoop,
    moveTo,
    moveLoop
}
public class UiAnimation : MonoBehaviour
{
    [Header("Ui Type:")]
    public Type type;

    [Header("Run On Start?")]
    public bool yes;

    [Header("Rect Transform Target:")]
    public RectTransform target;

    [Header("Chained Objects:")]
    public GameObject[] chain;

    private void Start()
    {
        if(yes)
            switch (type)
            {
                case Type.scaleIn:
                    LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), .25f);
                    Timing.RunCoroutine(_SetScaleBack().CancelWith(gameObject));
                    break;

                case Type.scaleLoop:
                    LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), .65f).setLoopPingPong();
                    break;

                case Type.moveTo:
                    LeanTween.moveLocal(gameObject, target.anchoredPosition, .5f);  //Test...
                    break;

                case Type.moveLoop:
                    LeanTween.moveLocal(gameObject, target.anchoredPosition, 1.2f).setLoopPingPong();
                    break;
            }
    }

    public void CallAction()
    {
        switch (type)
        {
            case Type.scaleIn:
                LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), .25f);
                Timing.RunCoroutine(_SetScaleBack().CancelWith(gameObject));
                break;

            case Type.scaleLoop:
                LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), .65f).setLoopPingPong();
                break;

            case Type.moveTo:
                LeanTween.moveLocal(gameObject, target.anchoredPosition, .5f);  //Test...
                break;

            case Type.moveLoop:
                LeanTween.moveLocal(gameObject, target.anchoredPosition, 1.2f).setLoopPingPong();
                break;
        }
    }

    IEnumerator<float> _SetScaleBack()
    {
        yield return Timing.WaitForSeconds(.25f);
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), .25f);

        if (chain.Length > 0)
            Timing.RunCoroutine(_ActivateLater().CancelWith(gameObject));
    }

    IEnumerator<float> _ActivateLater()
    {
        yield return Timing.WaitForSeconds(.25f);
        chain[0].SetActive(true);

        if (chain.Length > 1)
        {
            yield return Timing.WaitForSeconds(.5f);
            chain[1].SetActive(true);
        }
    }
}

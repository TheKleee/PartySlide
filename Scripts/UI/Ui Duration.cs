using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[RequireComponent(typeof(UiAnimation))]
public class UiDuration : MonoBehaviour
{
    [Header("Life span:")]
    public float lifeSpan;

    private void Start()
    {
        Timing.RunCoroutine(_Life().CancelWith(gameObject));
    }

    IEnumerator<float> _Life()
    {
        yield return Timing.WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }
}

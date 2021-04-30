using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class BoxMove : MonoBehaviour
{
    private GameObject target;
    private int randSummon;

    [Header("Stuff To Summon:")]
    public GameObject[] summons;

    [Header("Vfx:")]
    public GameObject vfx;

    private void Start()
    {
        target = transform.GetChild(0).gameObject;
        randSummon = Random.Range(0, summons.Length);
    }

    private void OnTriggerEnter(Collider player)
    {
        if(player.GetComponent<PlayerMovement>() != null)
        {
            LeanTween.move(target, transform.position, 0.2f);
            Timing.RunCoroutine(_MoveTarget().CancelWith(gameObject));
        }
    }

    IEnumerator<float> _MoveTarget()
    {
        yield return Timing.WaitForSeconds(.2f);
        Instantiate(vfx, transform.position, vfx.transform.rotation);

        yield return Timing.WaitForSeconds(.05f);
        GameObject bonus = Instantiate(summons[randSummon], transform.parent, false);
        bonus.transform.localPosition = transform.localPosition;
        bonus.transform.localEulerAngles = transform.localEulerAngles;
        Destroy(gameObject);
    }
}

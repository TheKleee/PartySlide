using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugFix : MonoBehaviour
{
    private Transform pCont;

    [Header("Vector Position Fix:")]
    public float fixPosY;

    private void OnCollisionEnter(Collision player)
    {
        if (player.transform.GetComponent<PlayerMovement>() != null)
        {
            pCont = player.transform.parent.transform;

            pCont.transform.position = new Vector3(0,
                fixPosY,
                pCont.transform.position.z);
        }
    }
}

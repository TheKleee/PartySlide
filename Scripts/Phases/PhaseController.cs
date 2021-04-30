using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : MonoBehaviour
{
    [Header("Has Won:")]
    public bool won;

    [Header("Next Map Segement:")]
    public GameObject nextMapParent;

    //Only has this function atm : /
    private void OnTriggerEnter(Collider player)
    {
        if (player.GetComponent<PlayerMovement>() != null)
        {
            if (won)
                player.GetComponent<PlayerMovement>().PhaseTwo();
            else
            {
                player.GetComponent<PlayerMovement>().map = nextMapParent;
                player.GetComponent<PlayerMovement>().PhaseMid();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private PlayerMovement pMovement;
    private UiController uiCont;

    [Header("Pts Times:")]
    public int xNum;

    [Header("Confetti:")]
    public GameObject[] confetti = new GameObject[2];   //Get yar own confetti mate! >: O

    private void OnCollisionEnter(Collision player)
    {
        if(player.transform.GetComponent<PlayerMovement>() != null)
        {
            pMovement = player.transform.GetComponent<PlayerMovement>();
            uiCont = pMovement.pCont.GetComponent<UiController>();

            uiCont.multiplayer.text = "X" + xNum;
            uiCont.xNum = xNum;
            uiCont.EndLevel();
            pMovement.CallConfetti();
            pMovement.pCont.RemoveBalloons();

            foreach (GameObject c in confetti)
                c.SetActive(true);
        }
    }
}

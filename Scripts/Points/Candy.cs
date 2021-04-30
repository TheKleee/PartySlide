using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    private PlayerController pCont;
    private UiController uiCont;

    [Header("Vfx:")]
    public GameObject vfx;  //The initial vfx! >:D

    [Header("Ui:")]
    public GameObject uiElement;

    private void OnTriggerEnter(Collider player)
    {
        if(player.GetComponent<PlayerMovement>() != null)
        {
            pCont = player.GetComponent<PlayerMovement>().pCont;
            uiCont = pCont.GetComponent<UiController>();

            //Add points and so on...
            uiCont.AddPts();
            if (vfx != null)
            {
                GameObject candy = Instantiate(vfx, transform.position, vfx.transform.rotation);
                candy.transform.SetParent(pCont.transform);
            }

            Destroy(gameObject);
        }
    }
}

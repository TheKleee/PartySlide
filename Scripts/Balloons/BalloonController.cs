using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BalloonType
{
    red,
    green,
    blue
}
public class BalloonController : MonoBehaviour
{
    #region This is hidden and you cannot see it >:C
    private PlayerController pCont;     //Oh no my eyes! >: C
    private UiController uiCont;        //Oh my :O
    #endregion

    [Header("Type:")]
    public BalloonType type;


    private void OnTriggerEnter(Collider player)
    {
        if (player.GetComponent<PlayerMovement>() != null)
        {
            pCont = player.GetComponent<PlayerMovement>().pCont;
            uiCont = pCont.GetComponent<UiController>();
            pCont.SlowDown();
            //Do what you must... do your worst!!! D:<

            switch (type)
            {
                case BalloonType.red:
                    pCont.Red();
                    uiCont.BonusPts(pCont.balloons[0].transform);
                    break;

                case BalloonType.green:
                    pCont.Green();
                    uiCont.BonusPts(pCont.balloons[1].transform);
                    break;

                case BalloonType.blue:
                    pCont.Blue();
                    uiCont.BonusPts(pCont.balloons[2].transform);
                    break;
            }

            Destroy(gameObject);
        }
    }
}

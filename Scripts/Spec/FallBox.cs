using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBox : MonoBehaviour
{
    [Header("Boxes:")]
    public GameObject BlueBox;
    public GameObject GreenBox;

    private void Start()
    {
        int randInt = Random.Range(0, 10);

        if (randInt < 2)
            BlueBox.SetActive(true);
        else if(randInt < 4)
            GreenBox.SetActive(true);
    }
}

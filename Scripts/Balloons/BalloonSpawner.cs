using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{

    [Header("Balloon List:")]
    public GameObject[] balloonList = new GameObject[5];

    private void Start()
    {
        int spawnId = Random.Range(0, balloonList.Length);
        balloonList[spawnId].SetActive(true);
    }
}

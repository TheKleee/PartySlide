using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialReward : MonoBehaviour
{

    [Header("Ads Manager:")]
    [SerializeField] public AdsManager ads;

    private void Start()
    {
        ads.ShowReward();
    }
}

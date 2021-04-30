using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    void Start()
    {
        if (SaveData.instance.bonusLvl)
            SceneManager.LoadSceneAsync(2);
        else
            SceneManager.LoadSceneAsync(1);
    }
}

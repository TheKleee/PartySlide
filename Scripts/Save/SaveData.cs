using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    #region Singleton

    public static SaveData instance;

    private void Awake()
    {
        //Set screen size for Standalone
#if UNITY_STANDALONE
            Screen.SetResolution(720, 900, false);
            Screen.fullScreen = false;
#endif

        if (instance != null)
            Destroy(gameObject);        //Make sure that there aren't multiple instances of this or we're screwed >xD
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        LoadGame();

        TinySauce.OnGameStarted();
    }

    #endregion

    #region Main Save Information:

    [Header("Main Save Data:")]
    public int points;
    public bool bonusLvl;
    public int lvl;
    public int[] nextTreeLvls = new int[3];

    [Header("Skins Save Data:")]
    public List<int> bodyUnlocked = new List<int>();
    public int bodyId;

    [Header("Remove Ads Info:")]
    public bool noAds;

    //Special:
    public bool changeNumbers;

    #endregion

    #region Save and Load:

    public void SaveGame()              //This should be called whenever the game needs to be saved...
    {
        Save.SaveUser(this);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void LoadGame()
    {
        //Load user data...
        if (File.Exists(Application.persistentDataPath + "/user.dat"))
        {
            UserValuesData player = Save.LoadUser();

            points = player.points;
            bonusLvl = player.bonusLvl;
            nextTreeLvls = player.nextTreeLvls;
            changeNumbers = player.changeNumbers;
            lvl = player.lvl;

            bodyUnlocked = player.bodyUnlocked;
            bodyId = player.bodyId;
            noAds = player.noAds;
        }
    }

    #endregion


    
}

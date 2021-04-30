using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Save : MonoBehaviour
{
    #region Saves:

    public static void SaveUser(SaveData player)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/user.dat", FileMode.Create);

        UserValuesData data = new UserValuesData(player);

        bf.Serialize(stream, data);
        stream.Close();
    }

    #endregion

    #region Loads:

    public static UserValuesData LoadUser()
    {
        if (File.Exists(Application.persistentDataPath + "/user.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/user.dat", FileMode.Open);

            UserValuesData data = (UserValuesData)bf.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Your file has not yet been created!!!");
            return null;
        }
    }

    #endregion
}

[Serializable]
public class UserValuesData
{
    public int points;
    public bool bonusLvl;
    public int[] nextTreeLvls;
    public bool changeNumbers;
    public int lvl;

    //New Stuff
    public List<int> bodyUnlocked = new List<int>();
    public int bodyId;
    public bool noAds;

    public UserValuesData(SaveData player)
    {
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
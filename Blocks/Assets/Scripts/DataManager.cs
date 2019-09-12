using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    #region Variables
    private int maxLevelPlayed;
    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/userData.dat");
        UserData data = new UserData();
        data.maxLevelPLayed = maxLevelPlayed;
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/userData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/userData.dat", FileMode.Open);
            UserData data = (UserData)bf.Deserialize(file);
            file.Close();
            maxLevelPlayed = data.maxLevelPLayed;
        }
        else
        {
            SaveData();
        }
    }

    public void SaveLevel(int level)
    {
        maxLevelPlayed = level;
        SaveData();
    }

    public int MaxLevelPLayed()
    {
        return maxLevelPlayed;
    }
}
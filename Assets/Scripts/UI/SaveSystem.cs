using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void InitializeBase()
    {
        if (!File.Exists(Application.persistentDataPath + "/dataBase.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/dataBase.dat";

            FileStream stream = new FileStream(path, FileMode.Create);

            DataBase initialDataBase = new DataBase();
            initialDataBase.dataBase.Add("admin", new Tuple<string, string>("mesBoules@BordDe.Leau", "admin"));
            
            formatter.Serialize(stream, initialDataBase);
            stream.Close();
        }
    }


    public static void SaveDataBase(DataBase dataBase)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/dataBase.dat";

        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, dataBase);
        stream.Close();
    }

    public static DataBase LoadDataBase()
    {
        string path = Application.persistentDataPath + "/dataBase.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataBase dataBase = formatter.Deserialize(stream) as DataBase;
            stream.Close();
            return dataBase;
        }
        else
        {
            Debug.LogError("Database not found !");
        }
        return null;
    }
}

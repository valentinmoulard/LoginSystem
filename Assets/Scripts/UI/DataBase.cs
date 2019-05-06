using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBase
{
    public Dictionary<string, Tuple<string, string>> dataBase;
    

    public DataBase()
    {
        dataBase = new Dictionary<string, Tuple<string, string>>();
    }
    public DataBase(CanvasController canvasController)
    {
        dataBase = canvasController.dataBase.dataBase;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public static class ExtentionMethods
{
    public static void ClearTMProInputField(this TMPro.TMP_InputField tmPro_InputField)
    {
        tmPro_InputField.text = "";
        
    }
}
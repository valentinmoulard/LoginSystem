using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject loginCanvas;
    public GameObject signUpCanvas;
    public GameObject loggedCanvas;


    [Header("Login Page")]
    public TMPro.TMP_InputField login_loginInput;
    public TMPro.TMP_InputField login_passwordInput;
    //login canvas particular
    public TMPro.TextMeshProUGUI accountCreatedText;
    public TMPro.TextMeshProUGUI wrongLoginOrPasswordText;


    [Header("Sign Up Page")]
    public TMPro.TMP_InputField signUp_loginInput;
    public TMPro.TMP_InputField signUp_mailAddressInput;
    public TMPro.TMP_InputField signUp_passwordInput;
    public TMPro.TMP_InputField signUp_passwordConfirmInput;
    //sign up canvas particular
    public TMPro.TextMeshProUGUI checkTheField;




    public DataBase dataBase;

    void Start()
    {
        login_passwordInput.contentType = TMPro.TMP_InputField.ContentType.Password;
        signUp_passwordInput.contentType = TMPro.TMP_InputField.ContentType.Password;
        signUp_passwordConfirmInput.contentType = TMPro.TMP_InputField.ContentType.Password;

        SaveSystem.InitializeBase();
        try
        {
            dataBase = SaveSystem.LoadDataBase();
            Debug.Log("Data base loaded !");
        }
        catch (Exception)
        {
            Debug.LogError("Data base load failed !");
            throw;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ShowDataBaseContent(dataBase);
        }

    }

    void ShowDataBaseContent(DataBase dataBase)
    {
        Debug.Log("Number of User : " + dataBase.dataBase.Count);

        foreach (KeyValuePair<string, Tuple<string, string>> user in dataBase.dataBase)
        {
            Debug.Log("Username : " + user.Key + " || mail @ : " + user.Value.Item1 + " || password : " + user.Value.Item1);
        }
    }


    #region UI functions

    public void LogIn()
    {
        if (login_loginInput.text != string.Empty && login_passwordInput.text != string.Empty)
        {
            foreach (KeyValuePair<string, Tuple<string, string>> user in dataBase.dataBase)
            {
                if (user.Key == login_loginInput.text)
                {
                    if (user.Value.Item2 == login_passwordInput.text)
                    {
                        Debug.Log("login successful !");
                        ShowLoggedCanvas();
                        return;
                    }
                }
            }
            Debug.Log("Wrong login or password");
        }
    }


    public void SignUp()
    {
        if (signUp_loginInput.text != string.Empty && signUp_mailAddressInput.text != string.Empty
            && signUp_passwordInput.text != string.Empty && signUp_passwordConfirmInput.text != string.Empty
            && signUp_passwordInput.text == signUp_passwordConfirmInput.text && signUp_loginInput.text != signUp_passwordInput.text)
        {
            try
            {
                dataBase.dataBase.Add(signUp_loginInput.text, new Tuple<string, string>(signUp_mailAddressInput.text, signUp_passwordInput.text));
                HideSignUpCanvas();

            }
            catch (Exception)
            {
                Debug.LogError("Save failed !");
                throw;
            }
        }
    }


    public void ShowSignUpCanvas()
    {
        signUpCanvas.SetActive(true);
    }

    public void ShowLoggedCanvas()
    {
        loggedCanvas.SetActive(true);
    }

    public void HideSignUpCanvas()
    {
        signUpCanvas.SetActive(false);
    }

    public void HideLoggedCanvas()
    {
        loggedCanvas.SetActive(false);
    }

    #endregion




}

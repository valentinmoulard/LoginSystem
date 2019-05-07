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
    public GameObject accountCreatedText;
    public GameObject LogInErrorMessage;


    [Header("Sign Up Page")]
    public TMPro.TMP_InputField signUp_loginInput;
    public TMPro.TMP_InputField signUp_mailAddressInput;
    public TMPro.TMP_InputField signUp_passwordInput;
    public TMPro.TMP_InputField signUp_passwordConfirmInput;
    //sign up canvas particular
    public GameObject signUpErrorMessage;
    
    public DataBase dataBase;

    void Start()
    {
        login_passwordInput.contentType = TMPro.TMP_InputField.ContentType.Password;
        signUp_passwordInput.contentType = TMPro.TMP_InputField.ContentType.Password;
        signUp_passwordConfirmInput.contentType = TMPro.TMP_InputField.ContentType.Password;

        signUp_mailAddressInput.characterValidation = TMPro.TMP_InputField.CharacterValidation.EmailAddress;

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

    /// <summary>
    /// show the content of the database
    /// </summary>
    /// <param name="dataBase"></param>
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
                        ShowLoggedCanvas();
                        return;
                    }
                }
            }
            ShowError(LogInErrorMessage, "Invalid inputs...");
        }
    }


    public void SignUp()
    {
        if (signUp_loginInput.text != string.Empty && signUp_mailAddressInput.text != string.Empty
            && signUp_passwordInput.text != string.Empty && signUp_passwordConfirmInput.text != string.Empty
            && signUp_passwordInput.text == signUp_passwordConfirmInput.text && signUp_loginInput.text != signUp_passwordInput.text
            && !dataBase.dataBase.ContainsKey(signUp_loginInput.text))
        {
            try
            {
                dataBase.dataBase.Add(signUp_loginInput.text, new Tuple<string, string>(signUp_mailAddressInput.text, signUp_passwordInput.text));
                SaveSystem.SaveDataBase(dataBase);
                HideSignUpCanvas();
                accountCreatedText.SetActive(true);
            }
            catch (Exception)
            {
                Debug.LogError("Save failed !");
                throw;
            }
        }
        else
        {
            ShowError(signUpErrorMessage, "Invalid fields or user already exists");
        }
    }

    public void LogOut()
    {
        HideLoggedCanvas();
    }


    /// <summary>
    /// for a given text mesh pro GameObject, set it active and show the error message
    /// </summary>
    /// <param name="textError"></param>
    /// <param name="_errorMessage"></param>
    public void ShowError(GameObject textError, string _errorMessage)
    {
        textError.SetActive(true);
        textError.GetComponent<TMPro.TextMeshProUGUI>().text = _errorMessage;
    }
    

    /// <summary>
    /// show / hide functions of the different canvas ===========================================================================================
    /// </summary>
    
    public void ShowSignUpCanvas()
    {
        //clear the fields of the login canvas
        accountCreatedText.SetActive(false);
        LogInErrorMessage.SetActive(false);
        //clear all the input fields
        login_loginInput.ClearTMProInputField();
        login_passwordInput.ClearTMProInputField();

        signUpCanvas.SetActive(true);
    }

    public void ShowLoggedCanvas()
    {
        //clear the fields of the login canvas
        accountCreatedText.SetActive(false);
        LogInErrorMessage.SetActive(false);
        //clear all the input fields
        login_loginInput.ClearTMProInputField();
        login_passwordInput.ClearTMProInputField();

        loggedCanvas.SetActive(true);
    }
    
    public void HideSignUpCanvas()
    {
        signUpErrorMessage.SetActive(false);
        //clear all the input fields
        signUp_loginInput.ClearTMProInputField();
        signUp_mailAddressInput.ClearTMProInputField();
        signUp_passwordInput.ClearTMProInputField();
        signUp_passwordConfirmInput.ClearTMProInputField();

        signUpCanvas.SetActive(false);
    }

    public void HideLoggedCanvas()
    {
        //clear all the fields
        loggedCanvas.SetActive(false);
    }

    #endregion




}

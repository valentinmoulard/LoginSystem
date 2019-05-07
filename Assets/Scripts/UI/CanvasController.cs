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
    //reset password UI
    public GameObject login_passwordResetText;
    public GameObject login_mailPasswordInputField;
    public GameObject login_submitButton;
    public GameObject login_passwordResetMailSent;
    public bool isHiden;
    //login canvas particular
    public GameObject login_accountCreatedText;
    public GameObject login_LogInErrorMessage;


    [Header("Sign Up Page")]
    public TMPro.TMP_InputField signUp_loginInput;
    public TMPro.TMP_InputField signUp_mailAddressInput;
    public TMPro.TMP_InputField signUp_passwordInput;
    public TMPro.TMP_InputField signUp_passwordConfirmInput;
    //sign up canvas particular
    public GameObject signUpErrorMessage;
    

    [Header("Logged Page")]
    public TMPro.TextMeshProUGUI logged_name;
    public TMPro.TextMeshProUGUI logged_mail;


    public DataBase dataBase;

    public string currentUser;


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
                        currentUser = user.Key;
                        logged_name.text = user.Key;
                        logged_mail.text = user.Value.Item1;
                        return;
                    }
                }
            }
        }
        ShowError(login_LogInErrorMessage, "Invalid inputs...");
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
                login_accountCreatedText.SetActive(true);
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

    public void SubmitPasswordResetRequest()
    {
        GameObject destinationMail = login_mailPasswordInputField.transform.GetChild(0).transform.Find("Text").gameObject;
        
        mono_gmail.SendMail("testunity2037@gmail.com", "loginsystem2037",
        destinationMail.GetComponent<TMPro.TextMeshProUGUI>().text,
        "Password reset request", "Normaly in this mail you should find a link to a password reset page but I don't have the skills to do that");

        login_passwordResetMailSent.SetActive(true);
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
        login_accountCreatedText.SetActive(false);
        login_LogInErrorMessage.SetActive(false);
        //clear all the input fields
        login_loginInput.ClearTMProInputField();
        login_passwordInput.ClearTMProInputField();
        //hide password reset content
        login_passwordResetText.SetActive(true);
        login_mailPasswordInputField.SetActive(true);
        login_submitButton.SetActive(true);

        if (isHiden == false)
        {
            ShowResetPasswordUI();
        }

        signUpCanvas.SetActive(true);
    }

    public void ShowLoggedCanvas()
    {
        //clear the fields of the login canvas
        login_accountCreatedText.SetActive(false);
        login_LogInErrorMessage.SetActive(false);
        //clear all the input fields
        login_loginInput.ClearTMProInputField();
        login_passwordInput.ClearTMProInputField();
        //hide password reset content
        login_passwordResetText.SetActive(true);
        login_mailPasswordInputField.SetActive(true);
        login_submitButton.SetActive(true);

        if (isHiden == false)
        {
            ShowResetPasswordUI();
        }

        loggedCanvas.SetActive(true);
    }
    
    public void ShowResetPasswordUI()
    {
        if (isHiden)
        {
            isHiden = !isHiden;
            login_passwordResetText.SetActive(true);
            login_mailPasswordInputField.SetActive(true);
            login_submitButton.SetActive(true);
        }
        else
        {
            isHiden = !isHiden;
            GameObject passwordText = login_mailPasswordInputField.transform.GetChild(0).transform.Find("Text").gameObject;
            passwordText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            login_passwordResetText.SetActive(false);
            login_mailPasswordInputField.SetActive(false);
            login_submitButton.SetActive(false);
        }

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
        logged_name.text = "";
        logged_mail.text = "";
        loggedCanvas.SetActive(false);
    }

    #endregion
    
}

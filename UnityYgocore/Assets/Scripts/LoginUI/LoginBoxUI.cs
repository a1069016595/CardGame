using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginBoxUI : MonoBehaviour
{
    #region 单例
    private static LoginBoxUI instance;

    public LoginBoxUI()
    {
        instance = this;
    }

    public static LoginBoxUI GetInstance()
    {
        return instance;
    }
    #endregion

    private LoginUI loginUI;

    public Button appplyButton;
    public Button closeButton;

    public InputField userInputField;
    public InputField passwordInputField;

    public void Init()
    {
        appplyButton = transform.FindChild("applyButton").GetComponent<Button>();
        closeButton = transform.FindChild("closeButton").GetComponent<Button>();
        userInputField = transform.FindChild("userInputField").GetComponent<InputField>();
        passwordInputField = transform.FindChild("passwordInputField").GetComponent<InputField>();

        appplyButton.onClick.AddListener(OnAppplyButton);
        closeButton.onClick.AddListener(OnCloseButton);

        loginUI = LoginUI.GetInstance();
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        userInputField.text = "";
        passwordInputField.text = "1";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnAppplyButton()
    {
        string userName = userInputField.text;
        string password = passwordInputField.text;
        ComVal.account = userName;
        loginUI.Login(userName,password);
    }

    private void OnCloseButton()
    {
        Hide();
    }
}

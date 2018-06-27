using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Protocol;

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
    //   public InputField passwordInputField;

    ErrorManager errorManager;

    normalDele hideBoxDele;

    public void Init()
    {
        errorManager = ErrorManager.GetInstance();

        appplyButton = transform.FindChild("applyButton").GetComponent<Button>();
        closeButton = transform.FindChild("closeButton").GetComponent<Button>();
        userInputField = transform.FindChild("userInputField").GetComponent<InputField>();
        //       passwordInputField = transform.FindChild("passwordInputField").GetComponent<InputField>();

        appplyButton.onClick.AddListener(OnAppplyButton);
        closeButton.onClick.AddListener(OnCloseButton);

        loginUI = LoginUI.GetInstance();
        Hide();
    }

    public void Show(normalDele dele)
    {
        hideBoxDele = dele;
        gameObject.SetActive(true);
        userInputField.text = "";
        //       passwordInputField.text = "1";
    }

    private void Hide()
    {
        if (hideBoxDele != null)
        {
            hideBoxDele();
        }
        gameObject.SetActive(false);
    }

    private void OnAppplyButton()
    {
        string userName = userInputField.text;
        //        string password = passwordInputField.text;
        ComVal.account = userName;
        Login(userName);
    }

    private void OnCloseButton()
    {
        Hide();
    }


    /// <summary>
    /// 登录
    /// </summary>
    public void Login(string userName)
    {
        if (userName == "")
        {
            //ErrorModel m = new ErrorModel("用户名或密码不能为空");
            errorManager.AddErrorModel("用户名不能为空");
        }
        else
        {
            AccountInfoDTO dto = new AccountInfoDTO();
            dto.account = userName;
            NetWorkScript.Instance.write(TypeProtocol.TYPE_LOGIN_BRQ, 0, LoginProtool.LOGIN_BRQ, dto);
        }
    }
}

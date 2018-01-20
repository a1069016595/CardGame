using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Protocol;

public class LoginUI : BaseUI
{
    #region 单例
    private static LoginUI instance;

    public LoginUI()
    {
        instance = this;
    }

    public static LoginUI GetInstance()
    {
        return instance;
    }
    #endregion

    LoginBoxUI loginBoxUI;
    ErrorManager errorManager;

    public Button loginButton;
    public Button editDeckButton;

    void Awake()
    {
        loginButton = transform.FindChild("loginButton").GetComponent<Button>();
        editDeckButton = transform.FindChild("EditCardBtn").GetComponent<Button>();
        loginButton.onClick.AddListener(OnLoginButton);
        editDeckButton.onClick.AddListener(OnEditButton);
        loginBoxUI = LoginBoxUI.GetInstance();
        errorManager = ErrorManager.GetInstance();

        loginBoxUI.Init();

    }



    void OnLoginButton()
    {
        NetWorkScript.Instance.init();
        if (!NetWorkScript.Instance.isConnect)
        {
            errorManager.AddErrorModel("无法连接到服务器");
        }
        else
            loginBoxUI.Show();
    }

    void OnEditButton()
    {
        UIMgr.GetInstance().LoadUI(ComStr.UI_EditCardUI);
    }

    /// <summary>
    /// 登录
    /// </summary>
    public void Login(string userName, string password)
    {
        if (userName == "" || password == "")
        {
            //ErrorModel m = new ErrorModel("用户名或密码不能为空");
            errorManager.AddErrorModel("用户名或密码不能为空");
        }
        else
        {
            AccountInfoDTO dto = new AccountInfoDTO();
            dto.account = userName;
            dto.password = password;
            NetWorkScript.Instance.write(TypeProtocol.TYPE_LOGIN_BRQ, 0, LoginProtool.LOGIN_BRQ, dto);
        }
    }
}

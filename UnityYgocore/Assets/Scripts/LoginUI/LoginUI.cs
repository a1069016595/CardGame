using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Protocol;
using System;

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

    Button loginButton;
    Button editDeckButton;
    Button watchPlayBackButton;

    void Awake()
    {
        loginButton = transform.GetChild<Button>("loginButton");
        editDeckButton = transform.GetChild<Button>("EditCardBtn");
        watchPlayBackButton = transform.GetChild<Button>("WatchPlayBackBtn");

        loginButton.onClick.AddListener(OnLoginButton);
        editDeckButton.onClick.AddListener(OnEditButton);
        watchPlayBackButton.onClick.AddListener(OnWatchPlayBackButton);

        loginBoxUI = LoginBoxUI.GetInstance();
        errorManager = ErrorManager.GetInstance();

        loginBoxUI.Init();

        AddHandler(DuelEvent.gameEvent_CloseChoosePlayBackPlane, ShowButton);
        AddHandler(DuelEvent.gameEvent_OpenChoosePlayBackPlane, HideButton);
    }

    private void ShowButton(params object[] args)
    {
        ShowButton();
    }

    private void ShowButton()
    {
        loginButton.interactable = true;
        editDeckButton.interactable = true;
        watchPlayBackButton.interactable = true;
    }

    private void HideButton(params object[] args)
    {
        HideButton();
    }

    private void HideButton()
    {
        loginButton.interactable = false;
        editDeckButton.interactable = false;
        watchPlayBackButton.interactable = false;
    }

    private void OnWatchPlayBackButton()
    {
        DuelEventSys.GetInstance.SendEvent(DuelEvent.gameEvent_OpenChoosePlayBackPlane);
    }

    void OnLoginButton()
    {
        NetWorkScript.Instance.init();
        if (!NetWorkScript.Instance.isConnect)
        {
            HideButton();
            errorManager.AddErrorModel("无法连接到服务器", ShowButton);
        }
        else
        {
            HideButton();
            loginBoxUI.Show(ShowButton);
            //AccountInfoDTO dto = new AccountInfoDTO();
            //dto.account = "1";
            //dto.password = "2";
            //NetWorkScript.Instance.write(TypeProtocol.TYPE_LOGIN_BRQ, 0, LoginProtool.LOGIN_BRQ, dto);
        }
    }

    void OnEditButton()
    {
        UIMgr.Instance().LoadUI(ComStr.UI_EditCardUI);
    }
}

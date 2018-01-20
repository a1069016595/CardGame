using UnityEngine;
using System.Collections;
using Protocol;
public class LoginHandler : MonoBehaviour, IHandler
{
    #region 单例
    private static LoginHandler instance;

    public LoginHandler()
    {
        instance = this;
    }

    public static LoginHandler GetInstance()
    {
        return instance;
    }
    #endregion

    ErrorManager errorManager;
    UIMgr uiMgr;
    public void Init()
    {
        errorManager = ErrorManager.GetInstance();
        uiMgr = UIMgr.GetInstance();
    }

    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case LoginProtool.LOGIN_CREQ:
                Login(model.GetMessage<int>());
                break;
            default:
                break;
        }
    }

    private void Login(int i)
    {
        switch (i)
        {
            case Login_Result_Protool.AccountNotExit:
                errorManager.AddErrorModel("账户不存在");
                break;
            case Login_Result_Protool.PasswordError:
                errorManager.AddErrorModel("密码错误");
                break;
            case Login_Result_Protool.UserIsOnLine:
                errorManager.AddErrorModel("用户已登录");
                break;
            case Login_Result_Protool.LoginSuccess:
                uiMgr.LoadUI(ComStr.UI_GameHallUI);
                NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ, 0, GameHallProtocol.GAMEHALL_ENTERGAMEHALL_BRQ, null);
                break;
            default:
                break;
        }
    }
}

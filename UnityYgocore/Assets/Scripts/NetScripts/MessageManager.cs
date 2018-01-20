using UnityEngine;
using System.Collections;
using Protocol;
public class MessageManager : MonoBehaviour
{

    //IHandler login;
    //IHandler user;
    //IHandler match;
    //IHandler select;
    //IHandler fight;
    // Use this for initialization

    GameHallHandler gameHallHandler;
    LoginHandler loginHandler;
    DuelHandler duelHandler;

    ErrorManager errorManager;
    void Awake()
    {
        gameHallHandler = GameHallHandler.GetInstance();
        loginHandler=LoginHandler.GetInstance();
        errorManager = ErrorManager.GetInstance();
        duelHandler = DuelHandler.GetInstance();

        errorManager.Init();
        loginHandler.Init();
        gameHallHandler.Init();
        //AccountInfoDTO dto = new AccountInfoDTO();
        //dto.account = "2";
        //dto.password = "3";
        //NetWorkScript.Instance.write(LoginProtool.LOGIN_CREQ, 1, 2, dto);
    }
   
    void Update()
    {
        while (NetWorkScript.Instance.messageList.Count > 0)
        {
            SocketModel model = NetWorkScript.Instance.messageList[0];
            NetWorkScript.Instance.messageList.RemoveAt(0);
            StartCoroutine("MessageReceive", model);
        }
    }

    void MessageReceive(SocketModel model)
    {
        //Debug.Log("ff");
        switch (model.type)
        {
            case TypeProtocol.TYPE_GAMEHALL_CREQ:
                gameHallHandler.MessageReceive(model);
                break;
            case TypeProtocol.TYPE_LOGIN_CREQ:
                loginHandler.MessageReceive(model);
                break;
            case TypeProtocol.TYPE_DUEL_CREQ:
                duelHandler.MessageReceive(model);
                break;
            default:
                Debug.Log("error");
                break;
        }
    }

    void OnApplicationQuit()
    {
        NetWorkScript.Instance.Close();
    }
}


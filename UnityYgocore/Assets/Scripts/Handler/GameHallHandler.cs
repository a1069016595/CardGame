using UnityEngine;
using System.Collections;
using Protocol;
public class GameHallHandler : MonoBehaviour,IHandler
{
    #region 单例
    private static GameHallHandler instance;

    public GameHallHandler()
    {
        instance = this;
    }

    public static GameHallHandler GetInstance()
    {
        return instance;
    }
    #endregion

    IHandler roomListUI;
    IHandler chatUI;
    UIMgr uiMgr;
    IHandler prepareUI;
    
    public void Init()
    {
        roomListUI = RoomListUI.GetInstance();
        chatUI = ChatUI.GetInstance();
        uiMgr = UIMgr.GetInstance();
        prepareUI = PrepareUI.GetInstance();

        roomListUI.Init();
        chatUI.Init();
    }

    public void MessageReceive(SocketModel model)
    {
        
        switch(model.command)
        {
            case GameHallProtocol.GAMEHALL_ROOMLIST_CREQ:
                roomListUI.MessageReceive(model);
                break;
            case GameHallProtocol.GAMEHALL_CHAT_CREQ:
                chatUI.MessageReceive(model);
                break;
            case GameHallProtocol.GAMEHALL_CREATEROOM_CREQ:
                CreateRoom(model);
                break;
            case GameHallProtocol.GAMEHALL_ENTERGAMEHALL_CREQ:
                roomListUI.MessageReceive(model);
                break;
            case GameHallProtocol.GAMEHALL_LEAVEROOM_CREQ:
                prepareUI.MessageReceive(model);
                break;
            case GameHallProtocol.GAMEHALL_ENTERROOM_CREQ:
                EnterRoom(model);
                break;
            case GameHallProtocol.GAMEHALL_READY_CREQ:
                prepareUI.MessageReceive(model);
                break;
            case GameHallProtocol.GAMEHALL_STARTGAME_CREQ:
                StartGame(model);
                break;
        }
    }

    void CreateRoom(SocketModel mes)
    {
        RoomInfoDTO dto=mes.GetMessage<RoomInfoDTO>();
       uiMgr.LoadUI(ComStr.UI_PerpareUI) ;
       PrepareUI.GetInstance().CreateRoom(dto);
    }

    void EnterRoom(SocketModel mes)
    {
        Debug.Log("进入房间");
        uiMgr.LoadUI(ComStr.UI_PerpareUI);
        prepareUI.MessageReceive(mes);
    }

    void StartGame(SocketModel mes)
    {
        DuelRoomMesDTO dto = mes.GetMessage<DuelRoomMesDTO>();
        Debug.Log("开始游戏");
        uiMgr.LoadUI(ComStr.UI_DuelFieldUI);
        Duel.GetInstance().StartGame();
    }
}

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

    RoomListUI roomListUI;
    ChatUI chatUI;
    UIMgr uiMgr;
    GameHallMesUI gameHallMesUI;
    
    public void Init()
    {
        roomListUI = RoomListUI.Instance();
        chatUI = ChatUI.Instance();
        uiMgr = UIMgr.Instance();
        gameHallMesUI = GameHallMesUI.Instance();

        roomListUI.Init();
        chatUI.Init();
        gameHallMesUI.Init();
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
                PrepareUI.GetInstance().MessageReceive(model);
                break;
            case GameHallProtocol.GAMEHALL_ENTERROOM_CREQ:
                EnterRoom(model);
                break;
            case GameHallProtocol.GAMEHALL_READY_CREQ:
                PrepareUI.GetInstance().MessageReceive(model);
                break;
            case GameHallProtocol.GAMEHALL_STARTGAME_CREQ:
                StartGame(model);
                break;
            case GameHallProtocol.GAMEHALL_GAMEHALLMES_CREQ:
                Debug.Log("jj");
                gameHallMesUI.MessageReceive(model);
                break;
        }
    }

    void CreateRoom(SocketModel mes)
    {
        RoomInfoDTO dto = mes.GetMessage<RoomInfoDTO>();
        uiMgr.LoadPerpareUI();
        PrepareUI.GetInstance().CreateRoom(dto);
    }

    void EnterRoom(SocketModel mes)
    {
        Debug.Log("进入房间");
        uiMgr.LoadPerpareUI();
        PrepareUI.GetInstance().MessageReceive(mes);
    }

    void StartGame(SocketModel mes)
    {
        DuelRoomMesDTO dto = mes.GetMessage<DuelRoomMesDTO>();
        Debug.Log("开始游戏");
        uiMgr.LoadDuelField();
        Duel.GetInstance().StartGame();
    }
}

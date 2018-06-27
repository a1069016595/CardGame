using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine.UI;
public class GameHallUI : BaseUI
{
    #region 单例
    private static GameHallUI instance;

    public GameHallUI()
    {
        instance = this;
    }

    public static GameHallUI GetInstance()
    {
        return instance;
    }
    #endregion

    //  public RoomListUI roomListUI;

    public Button createRoomBtn;

    public CreateRoomPanel createRoomPanel;



    void Awake()
    {
        createRoomBtn = transform.FindChild("CreateRoomBtn").GetComponent<Button>();
        createRoomPanel = transform.FindChild("CreateRoomPanel").GetComponent<CreateRoomPanel>();

        createRoomPanel.Init();
        createRoomBtn.onClick.AddListener(OnCreateRoomBtn);
    }

    public override void Show()
    {
        NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ, 0, GameHallProtocol.GAMEHALL_ENTERGAMEHALL_BRQ, null);
    }


    void OnCreateRoomBtn()
    {
        createRoomPanel.Show(CreateRoomDele);
    }

    /// <summary>
    /// 创建房间委托
    /// </summary>
    /// <param name="mes"></param>
    void CreateRoomDele(string mes)
    {
        NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ, 0, GameHallProtocol.GAMEHALL_CREATEROOM_BRQ, mes);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Protocol;
using System.Collections.Generic;


/// <summary>
/// 房主的准备开关一直为打开状态
/// 
/// 这里房主点开始游戏时 发送房主的卡组信息
/// 不是房主时 改变选项时发送卡组信息
/// </summary>
public class PrepareUI : BaseUI, IHandler
{
    #region 单例
    private static PrepareUI instance;

    public PrepareUI()
    {
        instance = this;
    }

    public static PrepareUI GetInstance()
    {
        return instance;
    }
    #endregion

    public Toggle hostToggle;
    public Toggle playerToggle;

    public Text playerAccount;
    public Text myAccount;

    public Dropdown deckDropDown;

    public Button startButton;
    public Button exitButton;

    /// <summary>
    /// 房主 真为自己 假为他人
    /// </summary>
    public bool theHost;

    UIMgr uiMgr;



    void Awake()
    {
        Debug.Log("jj");
        uiMgr = UIMgr.Instance();

        hostToggle = transform.FindChild("PlayerToggle").GetComponent<Toggle>();
        playerToggle = transform.FindChild("MyToggle").GetComponent<Toggle>();
        playerAccount = transform.FindChild("PlayerAccount").GetComponent<Text>();
        myAccount = transform.FindChild("MyAccount").GetComponent<Text>();
        deckDropDown = transform.FindChild("DeckDropdown").GetComponent<Dropdown>();
        startButton = transform.FindChild("StartButton").GetComponent<Button>();
        exitButton = transform.FindChild("ExitButton").GetComponent<Button>();

        InitMes();

        startButton.onClick.AddListener(OnStartGame);
        exitButton.onClick.AddListener(OnLeaveRoom);

        playerToggle.onValueChanged.AddListener(OnOptionValueChange);
        deckDropDown.onValueChanged.AddListener(OnChangeSelectDeck);

    }




    /// <summary>
    /// 进入他人房间
    /// </summary>
    public void EnterOtherRoom(string mes)
    {
        Debug.Log("进入他人房间");
        theHost = false;
        playerAccount.text = mes;
        myAccount.text = ComVal.account;
        playerToggle.isOn = false;
        playerToggle.enabled = true;
        GetDeckList();
        OnChangeSelectDeck(0);
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreateRoom(RoomInfoDTO mes)
    {
        Debug.Log("创建房间");
        theHost = true;
        playerAccount.text = mes.roomOwner;
        myAccount.text = "";
        playerToggle.enabled = false;
        playerToggle.isOn = false;
        GetDeckList();
    }

    /// <summary>
    /// 离开房间
    /// </summary>
    public void OnLeaveRoom()
    {
        InitMes();
        uiMgr.LoadUI(ComStr.UI_GameHallUI);
        NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ, 0, GameHallProtocol.GAMEHALL_LEAVEROOM_BRQ, null);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void OnStartGame()
    {
        if (theHost)
        {
            DuelMesDTO dto = new DuelMesDTO();
            Deck a = DeckLoad.LoadDeck(deckDropDown.captionText.text);
            dto.deck = new Protocol.Deck();
            dto.deck.mainDeck = a.mainDeck.ToArray();
            dto.deck.extraDeck = a.extraDeck.ToArray();
            dto.account = ComVal.account;
            NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ, 0, GameHallProtocol.GAMEHALL_STARTGAME_BRQ, dto);
        }
    }

    /// <summary>
    /// 其他人进入房间
    /// </summary>
    public void OtherEnterRoom(string mes)
    {
        myAccount.text = mes;
        playerToggle.isOn = false;
        playerToggle.enabled = false;
        Debug.Log("其他人进入房间");
    }
    /// <summary>
    /// 其他人离开房间
    /// </summary>
    public void OtherLeaveRoom()
    {
        myAccount.text = "";
        playerToggle.isOn = false;
        playerToggle.enabled = false;
        Debug.Log("其他人离开房间");
    }
    /// <summary>
    /// 房主离开房间
    /// </summary>
    public void HostLeaveRoom()
    {
        theHost = true;
        playerAccount.text = ComVal.account;
        myAccount.text = "";
        playerToggle.isOn = false;
        playerToggle.enabled = false;
        // startButton.gameObject.SetActive(true);
        Debug.Log("房主离开房间");
    }


    private void OnChangeSelectDeck(int arg0)
    {
        if (!theHost)
        {
            Debug.Log("发送 玩家改变卡组");
            DuelMesDTO dto = new DuelMesDTO();
            Deck a = DeckLoad.LoadDeck(deckDropDown.captionText.text);
            dto.deck = new Protocol.Deck();
            dto.deck.mainDeck = a.mainDeck.ToArray();
            dto.deck.extraDeck = a.extraDeck.ToArray();
            NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ, 0, GameHallProtocol.GAMEHALL_CHANGEDECK_BRQ, dto);
        }
    }

    public void OnOptionValueChange(bool value)
    {
        if (!theHost)
        {
            Debug.Log("发送 玩家准备");
            DuelMesDTO dto = new DuelMesDTO();
            dto.isReady = GetIsReady();
            NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ, 0, GameHallProtocol.GAMEHALL_READY_BRQ, dto);
        }
    }

    void GetDeckList()
    {
        List<string> list = DeckLoad.GetDeckNameList();
        List<Dropdown.OptionData> dropDownList = new List<UnityEngine.UI.Dropdown.OptionData>();
        for (int i = 0; i < list.Count; i++)
        {
            dropDownList.Add(new Dropdown.OptionData(list[i]));
        }
        deckDropDown.options = dropDownList;
    }

    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case GameHallProtocol.GAMEHALL_LEAVEROOM_CREQ:
                if (theHost)
                {
                    OtherLeaveRoom();
                }
                else
                {
                    HostLeaveRoom();
                }
                break;
            case GameHallProtocol.GAMEHALL_ENTERROOM_CREQ:
                if (theHost)
                {
                    string str = model.GetMessage<string>();
                    OtherEnterRoom(str);
                }
                else
                {
                    string str = model.GetMessage<string>();
                    EnterOtherRoom(str);
                }
                break;
            case GameHallProtocol.GAMEHALL_READY_CREQ:
                bool mes = model.GetMessage<bool>();
                playerToggle.isOn = mes;
                if (mes && theHost)
                {
                    startButton.gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    void InitMes()
    {
        playerAccount.text = "";
        myAccount.text = "";
        hostToggle.isOn = true;
        hostToggle.enabled = false;
        playerToggle.isOn = false;
        theHost = false;
        startButton.gameObject.SetActive(false);
    }

    private bool GetIsReady()
    {
        if (theHost)
        {
            return hostToggle.isOn;
        }
        else
        {
            return playerToggle.isOn;
        }
    }
}

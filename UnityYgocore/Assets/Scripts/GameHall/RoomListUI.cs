using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Protocol;

public class RoomListUI : MonoSingleton<RoomListUI>, IHandler
{


    public Button leftButton;
    public Button rightButton;
    public Button applyButton;
    public RectTransform layoutGroup;
    public GameObject roomPrefeb;

    /// <summary>
    /// 当前所有的房间集合
    /// </summary>
    public List<RoomInfoDTO> currentCardList;
    /// <summary>
    /// 当前显示的房间集合
    /// </summary>
    private List<RoomInfoDTO> curShowList;

    public Text text;

    public int curPage;
    public int totalPage;
    public List<Prefeb_Room> roomList;

    public int prefebNum;


    public void Init()
    {

        leftButton = this.transform.FindChild("leftButton").GetComponent<Button>();
        rightButton = this.transform.FindChild("rightButton").GetComponent<Button>();
        text = transform.FindChild("text").GetComponent<Text>();
        layoutGroup = this.transform.FindChild("layoutGroup").GetComponent<RectTransform>();

        roomPrefeb = Resources.Load("Prefebs/prefeb_room") as GameObject;
        roomList = new List<Prefeb_Room>();
        curPage = 0;

        leftButton.onClick.AddListener(LeftButtonClick);
        rightButton.onClick.AddListener(RightButtonClick);

        InitCardGroup();
        UpdateText();
    }


    /// <summary>
    /// 向后翻
    /// </summary>
    private void RightButtonClick()
    {
        if (totalPage == curPage)
        {
            return;
        }
        curPage++;
        curShowList = new List<RoomInfoDTO>();
        for (int i = (curPage - 1) * 9; i < curPage * 9; i++)
        {
            if (i >= currentCardList.Count)
            {
                break;
            }
            curShowList.Add(currentCardList[i]);

        }
        Show(curShowList);
        UpdateText();
    }

    /// <summary>
    /// 向前翻
    /// </summary>
    private void LeftButtonClick()
    {
        if (curPage <= 1)
        {
            return;
        }
        curPage--;
        curShowList = new List<RoomInfoDTO>();
        for (int i = (curPage - 1) * 9; i < curPage * 9; i++)
        {
            curShowList.Add(currentCardList[i]);
        }
        Show(curShowList);
        UpdateText();
    }

    private void InitCardGroup()
    {
        curShowList = new List<RoomInfoDTO>();
        for (int i = 0; i < 9; i++)
        {
            GameObject room = GameObject.Instantiate(roomPrefeb);
            room.transform.SetParent(layoutGroup.transform);
            room.transform.localScale = Vector3.one;
            //room.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            room.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            Prefeb_Room card_select = room.GetComponent<Prefeb_Room>();
            roomList.Add(card_select);
            card_select.SetActive(false);

        }
    }

    /// <summary>
    /// 显示搜索到的卡片
    /// </summary>
    private void ShowSearchRoom(List<RoomInfoDTO> list)
    {

        this.gameObject.SetActive(true);
        if ((list.Count % 9) != 0)
        {
            totalPage = list.Count / 9 + 1;
        }
        else
        {
            totalPage = list.Count / 9;
        }
        curPage = 1;
        currentCardList = list;
        curShowList = new List<RoomInfoDTO>();

        if (currentCardList.Count <= 9)
        {
            for (int i = 0; i < currentCardList.Count; i++)
            {
                RoomInfoDTO dto = currentCardList[i];
                curShowList.Add(dto);
            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                RoomInfoDTO dto = currentCardList[i];
                curShowList.Add(dto);
            }
        }

        Show(curShowList);
        UpdateText();
    }

    /// <summary>
    /// 显示房间
    /// </summary>
    /// <param name="cardList"></param>
    private void Show(List<RoomInfoDTO> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Prefeb_Room card_select = roomList[i];
            RoomInfoDTO dto = list[i];
            card_select.SetText(dto);
            card_select.SetActive(true);
        }
        for (int i = list.Count; i < 9; i++)
        {
            Prefeb_Room card_select = roomList[i];
            card_select.SetActive(false);
        }

    }
    /// <summary>
    /// 隐藏所有房间
    /// </summary>
    public void HideAllRoom()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            roomList[i].gameObject.SetActive(false);
        }
        curPage = 0;
        totalPage = 0;
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = curPage.ToString() + "/" + totalPage.ToString();
    }

    public void MessageReceive(SocketModel model)
    {
        Debug.Log("收到房间列表消息");
        RoomListDTO dto = model.GetMessage<RoomListDTO>();
        if (dto.roomCount == 0)
        {
            Debug.Log("隐藏房间");
            HideAllRoom();
            return;
        }
        List<RoomInfoDTO> list = new List<RoomInfoDTO>();
        foreach (var item in dto.roomList)
        {
            list.Add(item);
        }
        ShowSearchRoom(list);
    }
}


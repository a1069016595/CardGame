using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Protocol;

public class Prefeb_Room : MonoBehaviour
{
    public Text text_room;
    public Text text_room1;

    public Button button;

    public string roomState;
    public int roomID;
    public void SetText(RoomInfoDTO dto)
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButton);
        }
        roomID = dto.roomID;
        string roomName = dto.roomName;
        string roomOwner = dto.roomOwner;
        roomState = dto.roomState;
        text_room.text = roomID.ToString() + "\n" + roomState;
        text_room1.text = roomOwner + "\n" + roomName;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    /// <summary>
    /// 点击房间
    /// </summary>
    void OnButton()
    {
        if(roomState=="正在游戏")
            return;
        NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ, 0, GameHallProtocol.GAMEHALL_ENTERROOM_BRQ, roomID);
    }
}

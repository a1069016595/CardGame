using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Protocol;
using System.Collections.Generic;
public class ChatUI : MonoBehaviour,IHandler
{
       #region 单例
    private static ChatUI instance;

    public ChatUI()
    {
        instance = this;
    }

    public static ChatUI GetInstance()
    {
        return instance;
    }
    #endregion
    
    public InputField inputField;
    public Button okButton;

    ChattingUI chattingUI;

    public void Init()
    {
        inputField = transform.FindChild("InputField").GetComponent<InputField>();
        okButton = transform.FindChild("Button").GetComponent<Button>();
        chattingUI = transform.FindChild("ChattingUI").GetComponent<ChattingUI>();

        chattingUI.Init();

        okButton.onClick.AddListener(OnButtonClick);

    }
    /// <summary>
    /// 发送消息到服务端
    /// </summary>
    public void OnButtonClick()
    {
        string mes = inputField.text;
        ChatMesDTO dto = new ChatMesDTO();
        dto.mes = mes;
        NetWorkScript.Instance.write(TypeProtocol.TYPE_GAMEHALL_BRQ,0,GameHallProtocol.GAMEHALL_CHAT_BRQ,dto);
        inputField.text = "";
    }

    public void MessageReceive(SocketModel model)
    {
        Debug.Log("gg");
        ChatMesDTO dto = model.GetMessage<ChatMesDTO>();
        string mes = dto.userName + ":" + dto.mes;

        chattingUI.AddSentence(mes);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol;

public class GameHallMesUI : MonoSingleton<GameHallMesUI>, IHandler
{

    Text OnLineMes;
    Text PlayGameMes;
    Text StandbyMes;


    public void MessageReceive(SocketModel model)
    {
        GameHallMesDTO dto = model.GetMessage<GameHallMesDTO>();

        OnLineMes.text = "在线人数：" + dto.onlineNum;
        PlayGameMes.text = "游戏中：" + dto.playingNum;
        StandbyMes.text = "待机中：" + dto.standbyNum;
    }

    public void Init()
    {
        OnLineMes = transform.GetChild<Text>("OnLineMes");
        PlayGameMes = transform.GetChild<Text>("PlayGameMes");
        StandbyMes = transform.GetChild<Text>("StandbyMes");
    }
}

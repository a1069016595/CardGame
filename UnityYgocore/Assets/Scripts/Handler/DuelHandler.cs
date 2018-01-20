using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Protocol;

public class DuelHandler : MonoBehaviour, IHandler
{
      #region 单例
    private static DuelHandler instance;

    public DuelHandler()
    {
        instance = this;
    }

    public static DuelHandler GetInstance()
    {
        return instance;
    }
    #endregion


    GuessFirst guessFirstUI; 
    DuelEventSys eventSys;

    void Awake()
    {
        guessFirstUI = transform.FindChild("GuessFirst").GetComponent<GuessFirst>();
         eventSys=DuelEventSys.GetInstance;
    }

    public void MessageReceive(SocketModel model)
    {
        Debug.Log("接收信息  " + model.command);
        switch (model.command)
        {
            case DuelProtocol.GUESS_CREQ:
                guessFirstUI.ReciveMes(model.GetMessage<DuelGuessMesDTO>());
                break;
            case DuelProtocol.STARTGAME_CREQ:
                Duel.GetInstance().StartDuel(model.GetMessage<StartGameDTO>());
                break;
            case DuelProtocol.DialogBoxSelect_CREQ:
                eventSys.SendEvent(DuelEvent.netEvent_ReciveDialogBoxSelect, model.GetMessage<DuelOperateDialogBoxDTO>());
                break;
            case DuelProtocol.OperateCard_CREQ:
                eventSys.SendEvent(DuelEvent.netEvent_ReciveOperateCard, model.GetMessage<DuelOperateCardDTO>());
                break;
            case DuelProtocol.SelectCardGroup_CREQ:
                eventSys.SendEvent(DuelEvent.netEvent_ReciveSelectCardGroup, model.GetMessage<DuelSelectGroupCardDTO>());
                break;
            case DuelProtocol.SelectFieldCard_CREQ:
                eventSys.SendEvent(DuelEvent.netEvent_ReciveSelectFieldCard, model.GetMessage<DuelSelectFieldCardDTO>());
                break;
            case DuelProtocol.SelectGroupCardCon_CREQ:
                eventSys.SendEvent(DuelEvent.netEvent_ReciveSelectGroupCardCon, model.GetMessage<DuelSelectGroupCardConDTO>());
                break;
            case DuelProtocol.ChangePhase_CREQ:
                eventSys.SendEvent(DuelEvent.netEvent_ReciveChangePhase, model.GetMessage<DuelChangePhaseDTO>());
                break;
            case DuelProtocol.SelectPutType_CREQ:
                 eventSys.SendEvent(DuelEvent.netEvent_ReciveSelectPutType, model.GetMessage<DuelSelectPutTypeDto>());
                break;
        }
    }

    public void Init()
    {
      
    }
}


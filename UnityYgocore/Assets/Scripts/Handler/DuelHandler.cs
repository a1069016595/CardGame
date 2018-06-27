using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Protocol;

public class DuelHandler : BaseMonoBehivour, IHandler
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

    void Awake()
    {
        guessFirstUI = transform.FindChild("GuessFirst").GetComponent<GuessFirst>();
        AddHandler(DuelEvent.netEvent_ReciveDuelMes, MessageReceive);
    }


    public void MessageReceive(params object[] args)
    {
        SocketModel model = (SocketModel)args[0];
        MessageReceive(model);
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
                SendEvent(DuelEvent.netEvent_ReciveDialogBoxSelect, model.GetMessage<DuelOperateDialogBoxDTO>());
                break;
            case DuelProtocol.OperateCard_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveOperateCard, model.GetMessage<DuelOperateCardDTO>());
                break;
            case DuelProtocol.SelectCardGroup_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveSelectCardGroup, model.GetMessage<DuelSelectGroupCardDTO>());
                break;
            case DuelProtocol.SelectFieldCard_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveSelectFieldCard, model.GetMessage<DuelSelectFieldCardDTO>());
                break;
            case DuelProtocol.SelectGroupCardCon_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveSelectGroupCardCon, model.GetMessage<DuelSelectGroupCardConDTO>());
                break;
            case DuelProtocol.ChangePhase_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveChangePhase, model.GetMessage<DuelChangePhaseDTO>());
                break;
            case DuelProtocol.SelectPutType_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveSelectPutType, model.GetMessage<DuelSelectPutTypeDto>());
                break;
            case DuelProtocol.ApplySelectEffect_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveApplySelectEffect, model.GetMessage<DuelApplySelectEffectDTO>());
                break;
            case DuelProtocol.ChangeSelectEffect_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveChangeSelectEffect, model.GetMessage<DuelSelectEffectButtonDTO>());
                break;
            case DuelProtocol.GameFinish_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveGameEnd, null);
                break;
            case DuelProtocol.Surrender_CREQ:
                SendEvent(DuelEvent.netEvent_ReciveSurrender, null);
                break;
        }
    }

    public void Init()
    {

    }

}


using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelNetMesHandler : MonoBehaviour
{
    DuelEventSys eventSys;

    void Awake()
    {
        eventSys = DuelEventSys.GetInstance;

        eventSys.AddHandler(DuelEvent.netEvent_SendDialogBoxSelect, SendDialogBoxSelect);
        eventSys.AddHandler(DuelEvent.netEvent_SendOperateCard, SendOperateCard);
        eventSys.AddHandler(DuelEvent.netEvent_SendSelectCardGroup, SendSelectCardGroup);
        eventSys.AddHandler(DuelEvent.netEvent_SendSelectFieldCard, SendSelectFieldCard);
        eventSys.AddHandler(DuelEvent.netEvent_SendSelectGroupCardCon, SendSelectGroupCardCon);
        eventSys.AddHandler(DuelEvent.netEvent_SendChangePhase, SendChangePhase);
        eventSys.AddHandler(DuelEvent.netEvent_SendSelectPutType, SendSelectPutType);
    }

    private void SendSelectPutType(params object[] args)
    {
        Debug.Log("发送信息：选择卡片放置形式");
        DuelSelectPutTypeDto dto = new DuelSelectPutTypeDto();
        dto.isAttack = (bool)args[0];
        Send(dto, DuelProtocol.SelectPutType_BRQ);
    }

    private void SendChangePhase(params object[] args)
    {
        Debug.Log("发送信息：改变回合");
        DuelChangePhaseDTO dto = new DuelChangePhaseDTO();
        dto.phase = (int)args[0];
        Send(dto,DuelProtocol.ChangePhase_BRQ);
    }

    private void SendDialogBoxSelect(params object[] args)
    {
        Debug.Log("发送信息：对话框操作");
        DuelOperateDialogBoxDTO dto = new DuelOperateDialogBoxDTO();
        dto.isTrue = (bool)args[0];
        Send(dto,DuelProtocol.DialogBoxSelect_BRQ);
    }

    private void SendOperateCard(params object[] args)
    {
        Debug.Log("发送信息：操作卡片");
        DuelOperateCardDTO dto = new DuelOperateCardDTO();
        dto.area = (int)args[0];
        dto.rank = (int)args[1];
        dto.str = (string)args[2];
        dto.isMy = (bool)args[3];
        Send(dto,DuelProtocol.OperateCard_BRQ);
    }

    private void SendSelectCardGroup(params object[] args)
    {
        Debug.Log("发送信息：选择卡片组");
        DuelSelectGroupCardDTO dto = new DuelSelectGroupCardDTO();
        dto.isSelect = (bool)args[0];
        dto.rank = (int)args[1];
        Send(dto,DuelProtocol.SelectCardGroup_BRQ);
    }

    private void SendSelectFieldCard(params object[] args)
    {
        Debug.Log("发送信息：选择场地卡片");
        DuelSelectFieldCardDTO dto = new DuelSelectFieldCardDTO();
        dto.isSelect = (bool)args[0];
        dto.area = (int)args[1];
        dto.rank = (int)args[2];
        dto.isMy = (bool)args[3];
        Send(dto,DuelProtocol.SelectFieldCard_BRQ);
    }

    private void SendSelectGroupCardCon(params object[] args)
    {
        Debug.Log("发送信息：选择卡片组操作");
        DuelSelectGroupCardConDTO dto = new DuelSelectGroupCardConDTO();
        dto.CtrType = (int)args[0];
        Send(dto,DuelProtocol.SelectGroupCardCon_BRQ);
    }

    private void Send(object mes,byte type)
    {
        NetWorkScript.Instance.write(TypeProtocol.TYPE_DUEL_BRQ, 0, type, mes);
    }
}

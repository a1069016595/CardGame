using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public delegate void IntEvent(int val);
public delegate void StringEvent(string val);
public delegate void OperateCardEvent(int targetArea,int targetRank,string str,bool isMy);
public delegate List<string> GetOperateListEvent(int targetArea, int targetRank, bool isMy);

public delegate void NormalCallBackEvent(normalDele dele);

public delegate void ChainMesUpdate(Chain chain);

public delegate void DuelEventHandler(params object[] args);

public class DuelEventSys
{
    private static DuelEventSys instance;

    public int val = 0;

    private Dictionary<DuelEventHandler, GameObject> objDic;

    public static DuelEventSys GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new DuelEventSys();
                instance.Init();
            }

            return instance;
        }
    }

    private void Init()
    {
        objDic = new Dictionary<DuelEventHandler, GameObject>();
        handlerList = new HashSet<DuelEventHandler>[(int)DuelEvent.end];
        for (int i = 0; i < handlerList.Length; i++)
        {
            handlerList[i] = new HashSet<DuelEventHandler>();
        }
    }

    HashSet<DuelEventHandler>[] handlerList;

    public void DeleteHandler(DuelEvent e, DuelEventHandler handler)
    {
        val--;
        handlerList[(int)e].Remove(handler);
    }

    public void AddHandler(DuelEvent e, DuelEventHandler handler, GameObject obj)
    {
        val++;
        objDic.Add(handler, obj);
        handlerList[(int)e].Add(handler);
    }

    public void SendEvent(DuelEvent e, params object[] args)
    {
        List<DuelEventHandler> nullList = new List<DuelEventHandler>();
        foreach (var item in handlerList[(int)e])
        {
            if (objDic[item] == null)
            {
                nullList.Add(item);
            }
            else
            {
                item(args);
            }
        }
        foreach (var item in nullList)
        {
            handlerList[(int)e].Remove(item);
        }
    }
}

public enum DuelEvent
{
    gameEvent_OpenChoosePlayBackPlane,
    gameEvent_CloseChoosePlayBackPlane,
    gameEvent_EnterPlayBack,
    gameEvent_ExitPlayBack,

    netEvent_ReciveDuelMes,

    netEvent_ReciveSelectFieldCard,
    netEvent_SendSelectFieldCard,

    netEvent_ReciveDialogBoxSelect,
    netEvent_SendDialogBoxSelect,

    netEvent_ReciveSelectCardGroup,
    netEvent_SendSelectCardGroup,

    netEvent_ReciveOperateCard,
    netEvent_SendOperateCard,

    netEvent_ReciveSelectGroupCardCon,
    netEvent_SendSelectGroupCardCon,

    netEvent_ReciveChangePhase,
    netEvent_SendChangePhase,

    netEvent_ReciveSelectPutType,
    netEvent_SendSelectPutType,

    netEvent_ReciveChangeSelectEffect,
    netEvent_SendChangeSelectEffect,

    netEvent_ReciveApplySelectEffect,
    netEvent_SendApplySelectEffect,

    netEvent_ReciveGameEnd,
    netEvent_SendGameEnd,

    netEvent_ReciveSurrender,
    netEvent_SendSurrender,//投降

    duelEvent_operateCard,
    duelEvent_changePhase,
    duelEvent_UpdateSelectCardShow,
    duelEvent_RecordOperate,
    duelEvent_SavePlayBack,
    duelEvent_ShowSavePlayBackPanel,
    duelEvent_Surrender,

    uiEvent_ShowFieldCardMes,
    uiEvent_HideFieldCardMes,
    uiEvent_ShowOperateTip,
    uiEvent_UpdateSelectCardShow,//鼠标放上时显示卡片放大图及信息
   

    playBackEvent_StartGame,
    playBackEvent_OperateCard,
    playBackEvent_SelectFieldCard,
    playBackEvent_DialogBoxSelect,
    playBackEvent_SelectCardGroup,
    playBackEvent_ChangePhase,
    playBackEvent_SelectPutType,
    playBackEvent_SelectEffect,
    playBackEvent_Surrender,

    playBackEvent_StopPlay,
    playBackEvent_StartPlay,


    end,
}

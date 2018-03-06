using System.Collections;
using System.Collections.Generic;




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
        handlerList = new HashSet<DuelEventHandler>[(int)DuelEvent.end];
        for (int i = 0; i < handlerList.Length; i++)
        {
            handlerList[i] = new HashSet<DuelEventHandler>();
        }
    }

    HashSet<DuelEventHandler>[] handlerList;

    public void DeleteHandler(DuelEvent e, DuelEventHandler handler)
    {
        handlerList[(int)e].Remove(handler);
    }

    public void AddHandler(DuelEvent e,DuelEventHandler handler)
    {
        handlerList[(int)e].Add(handler);
    }

    public void SendEvent(DuelEvent e,params object[] args)
    {
        foreach (var item in handlerList[(int)e])
        {
            item(args);
        }
    }


    public event StringEvent onOver_updateSelectCardShow;
    public event GetOperateListEvent clickButton_GetOperateList;

    public event ChainMesUpdate updateUI_chainUI;

    public event NormalCallBackEvent uiAnim_chainAnim;



    public void UIAnim_ChainAnim(normalDele dele)
    {
        if(uiAnim_chainAnim!=null)
        {
            uiAnim_chainAnim(dele);
        }
    }

    public void UpdateUI_ChainUI(Chain chain)
    {
        if(updateUI_chainUI!=null)
        {
            updateUI_chainUI(chain);
        }
    }


    public List<string> ClickButton_GetOperateList(int targetArea, int targetRank, bool isMy)
    {
        if (clickButton_GetOperateList != null)
        {
            return clickButton_GetOperateList(targetArea, targetRank, isMy);
        }
        else
        {
            return null;
        }
    }

    public void OnOver_updateSelectCardShow(string val)
    {
        if(onOver_updateSelectCardShow!=null)
        {
            onOver_updateSelectCardShow(val);
        }
    }
}

public enum DuelEvent
{
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

    event_operateCard,
    event_changePhase,

    uiEvent_ShowFieldCardMes,
    uiEvent_HideFieldCardMes,
    end,
}

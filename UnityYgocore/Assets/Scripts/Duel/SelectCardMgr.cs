using UnityEngine;
using System.Collections;
using Protocol;

public class SelectCardMgr : DuelUIOpearate
{
    #region 单例
    private static SelectCardMgr instance;

    public SelectCardMgr()
    {
        instance = this;
    }

    public static SelectCardMgr GetInstance()
    {
        return instance;
    }
    #endregion

    public FieldMgr mFieldMgr;
    public FieldMgr oFieldMgr;

    public HandCardUI mHandCardUI;
    public HandCardUI oHandCardUI;

    public GroupCardSelectBack curDele;
    public int curMaxSelectNum;
    public int curSelectNum;
    public int curSelectCardType;
    public bool isInSelect;
    public bool isMax;
    public Group curGroup;
    public Group selectGroup;
    Duel duel;

    public void Init()
    {
        mFieldMgr = transform.FindChild("MField").GetComponent<FieldMgr>();
        oFieldMgr = transform.FindChild("OField").GetComponent<FieldMgr>();
        mHandCardUI = transform.FindChild("MHandCard").GetComponent<HandCardUI>();
        oHandCardUI = transform.FindChild("OHandCard").GetComponent<HandCardUI>();

        curGroup = new Group();
        selectGroup = new Group();

        duel = Duel.GetInstance();

       AddHandler(DuelEvent.netEvent_ReciveSelectFieldCard, ReciveSelectFieldCard);
        AddHandler(DuelEvent.playBackEvent_SelectFieldCard, PlayBackSelectFieldCard);
    }

    private void PlayBackSelectFieldCard(params object[] args)
    {
        HandleSelectCard((int)args[0],(int)args[1], (bool)args[2]);
    }

    private void ReciveSelectFieldCard(params object[] args)
    {
        DuelSelectFieldCardDTO dto = (DuelSelectFieldCardDTO)args[0];
        if (dto.isSelect)
        {
            HandleSelectCard(dto.area, dto.rank, !dto.isMy);
        }
        else
        {
            HandleDisSelectCard(dto.area, dto.rank, !dto.isMy);
        }
    }

    public void SelectCardFromGroup(Group group, GroupCardSelectBack dele, int num, bool isMy, bool isMax)
    {
        Duel.GetInstance().SetSelect();
        curDele = dele;
        curMaxSelectNum = num;
        curSelectNum = 0;
        curGroup = group;
        this.isMax = isMax;
        isInSelect = true;
        selectGroup = new Group();
        Group mg = new Group();
        Group og = new Group();
        Group mhg = new Group();
        Group ohg = new Group();
        isMySelect = isMy;

        for (int i = 0; i < group.GroupNum; i++)
        {
            Card card = group.GetCard(i);
            if (card.isMy)
            {
                if (card.curArea == ComVal.Area_Hand)
                    mhg.AddCard(card);
                else
                    mg.AddCard(card);
            }
            else
            {
                if (card.curArea == ComVal.Area_Hand)
                    ohg.AddCard(card);
                else
                    og.AddCard(card);
            }
        }
        if (mg.GroupNum != 0)
            mFieldMgr.SelectFieldCard(mg, isMy);
        if (og.GroupNum != 0)
            oFieldMgr.SelectFieldCard(og, isMy);
        if (mhg.GroupNum != 0)
            mHandCardUI.SelectFieldCard(mhg.GetRankList(), isMy);
        if (ohg.GroupNum != 0)
            oHandCardUI.SelectFieldCard(ohg.GetRankList(), isMy);

        if (duel.IsNetWork && !isMySelect)
        {
            WaitTip.GetInstance().ShowWaitTip();
        }
    }

    Card GetCard(int area, int rank, bool isMy)
    {
        for (int i = 0; i < curGroup.GroupNum; i++)
        {
            Card card = curGroup.GetCard(i);
            if (card.curArea == area && card.areaRank == rank && card.isMy == isMy)
                return card;
        }
        return Card.Empty();
    }

    public bool SelectCard(int area, int rank, bool isMy)
    {
        if (CanNotControl())
        {
            return false;
        }
        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectFieldCard, true, area, rank, isMy);
        }
        return HandleSelectCard(area, rank, isMy);
    }

    private bool HandleSelectCard(int area, int rank, bool isMy)
    {
        selectGroup.AddCard(GetCard(area, rank, isMy));
        curSelectNum++;
        if (curMaxSelectNum == curSelectNum)
        {
            eventSys.SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_SelectFieldCard, selectGroup);
            if (duel.IsNetWork && !isMySelect)
            {
                WaitTip.GetInstance().HideWaitTip();
            }
            EndSelect();
            Duel.GetInstance().SetNotSelect();
            curDele(selectGroup);
        }
        return true;
    }

    public bool DisSelectCard(int area, int rank, bool isMy)
    {
        if (CanNotControl())
        {
            return false;
        }
        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectFieldCard, false, area, rank, isMy);
        }
        return HandleDisSelectCard(area, rank, isMy);
    }

    private bool HandleDisSelectCard(int area, int rank, bool isMy)
    {
        selectGroup.RemoveCard(GetCard(area, rank, isMy));
        curSelectNum--;
        return true;
    }

    void EndSelect()
    {
        isInSelect = false;
        mFieldMgr.EndSelectCard();
        oFieldMgr.EndSelectCard();
        mHandCardUI.EndSelectCard();
        oHandCardUI.EndSelectCard();
    }

    void Update()
    {
        if (!isMax)
        {
            if (Input.GetMouseButtonDown(1) && curSelectNum > 0)
            {
                EndSelect();
                Duel.GetInstance().SetNotSelect();
                curDele(selectGroup);
            }
        }
    }
}

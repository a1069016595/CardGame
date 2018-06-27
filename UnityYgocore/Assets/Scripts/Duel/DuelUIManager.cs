using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;



public class DuelUIManager : BaseUI
{
    #region 单例
    private static DuelUIManager instance;

    public DuelUIManager()
    {
        instance = this;
    }

    public static DuelUIManager GetInstance()
    {
        return instance;
    }
    #endregion

    IDuel duel;

    public SelectCardShowUI selectCardShowUI;

    public OptionListUI optionListUI;

    GameFieldUI mDeckUI;
    GameFieldUI oDeckUI;

    public HandCardUI mHandCardUI;
    public HandCardUI oHandCardUI;

    public FieldMgr mFieldMgr;
    public FieldMgr oFieldMgr;

    public DialogBoxUI dialogBoxUI;
    public SelectCardUI selectCardUI;

    public LPSliderUI mLpSliderUI;
    public LPSliderUI oLpSliderUI;

    public LPChangeUI lpChangeUI;
    public CardEffectAnim cardEffectAnim;
    public SelectEffectUI selectEffectUI;
    public SelectPutType selectPutType;

    public PhaseButtonMgr phaseButtonMgr;

    public GuessFirst guessFirst;
    public FloatText floatText;
    public SelectCardMgr selectCardMgr;
    public DrawCardAnim mDrawCardAnim;
    public DrawCardAnim oDrawCardAnim;
    public AttactAnim attackAnim;
    public RoundCountUI roundCounterUI;

    public ErrorPlane tipPlane;

    public ChainUICtr chainUICtr;

    ChangeAreaAnim mChangeAreaAnim;
    ChangeAreaAnim oChangeAreaAnim;

    void Awake()
    {
        duel = Duel.GetInstance();
        mDeckUI = transform.FindChild("MDeckUI").GetComponent<GameFieldUI>();
        oDeckUI = transform.FindChild("ODeckUI").GetComponent<GameFieldUI>();

        mHandCardUI = transform.FindChild("MHandCard").GetComponent<HandCardUI>();
        oHandCardUI = transform.FindChild("OHandCard").GetComponent<HandCardUI>();
        mHandCardUI.Init();
        oHandCardUI.Init();

        mFieldMgr = transform.FindChild("MField").GetComponent<FieldMgr>();
        oFieldMgr = transform.FindChild("OField").GetComponent<FieldMgr>();
        mFieldMgr.Init(true);
        oFieldMgr.Init(false);

        mLpSliderUI = transform.FindChild("LPSlider_player1").GetComponent<LPSliderUI>();
        oLpSliderUI = transform.FindChild("LPSlider_player2").GetComponent<LPSliderUI>();

        lpChangeUI = transform.FindChild("lpChangeUI").GetComponent<LPChangeUI>();
        cardEffectAnim = transform.FindChild("CardEffectAnim").GetComponent<CardEffectAnim>();
        selectEffectUI = transform.FindChild("SelectEffectUI").GetComponent<SelectEffectUI>();
        phaseButtonMgr = transform.FindChild("PhaseButton").GetComponent<PhaseButtonMgr>();
        guessFirst = transform.FindChild("GuessFirst").GetComponent<GuessFirst>();
        floatText = transform.FindChild("FloatText").GetComponent<FloatText>();
        selectCardShowUI = transform.FindChild("SelectCardShow").GetComponent<SelectCardShowUI>();
        selectPutType = transform.FindChild("SelectPutType").GetComponent<SelectPutType>();
        mDrawCardAnim = transform.FindChild("MDrawCardAnim").GetComponent<DrawCardAnim>();
        oDrawCardAnim = transform.FindChild("ODrawCardAnim").GetComponent<DrawCardAnim>();
        attackAnim = transform.FindChild("AttackAnim").GetComponent<AttactAnim>();
        roundCounterUI = GetChild<RoundCountUI>("RoundNumText");

        mChangeAreaAnim = GetChild<ChangeAreaAnim>("MAnim");
        oChangeAreaAnim = GetChild<ChangeAreaAnim>("OAnim");

        chainUICtr = GetChild<ChainUICtr>("ChainUIMgr");

        selectCardMgr = SelectCardMgr.GetInstance();
        optionListUI = OptionListUI.GetInstance();
        dialogBoxUI = DialogBoxUI.GetInstance();
        selectCardUI = SelectCardUI.GetInstance();
        tipPlane = ErrorPlane.GetInstance();

        selectCardMgr.Init();
        phaseButtonMgr.Init();
        selectEffectUI.Init();
        cardEffectAnim.Init();
        lpChangeUI.Init();
        optionListUI.Init();
        dialogBoxUI.Init();
        selectCardUI.Init();
        selectPutType.Init();
        attackAnim.Init();
    }

    public T GetChild<T>(string name)
    {
        return transform.FindChild(name).GetComponent<T>();
    }



    #region 对卡组ui的操作
    public void InitBothDeck(int m_mainNum, int m_extraNum, int o_mainNum, int o_extraNum)
    {
        mDeckUI.InitDeck(m_mainNum, m_extraNum);
        oDeckUI.InitDeck(o_mainNum, o_extraNum);
    }

    public void AddCardToDeck(int deckArea, Group group, bool isMy)
    {
        if (isMy)
            mDeckUI.AddCardToDeck(deckArea, group);
        else
            oDeckUI.AddCardToDeck(deckArea, group);
    }

    public void RemoveCardFromDeck(int deckArea, Group group, bool isMy)
    {
        if (isMy)
            mDeckUI.RemoveCardFromDeck(deckArea, group);
        else
            oDeckUI.RemoveCardFromDeck(deckArea, group);
    }

    public void RemoveCardFromDeck(int deckArea, Card card, int num, bool isMy)
    {
        if (isMy)
            mDeckUI.RemoveCardFromDeck(deckArea, card, num);
        else
            oDeckUI.RemoveCardFromDeck(deckArea, card, num);
    }

    public void AddCardToDeck(int deckArea, Card card, int num, bool isMy)
    {
        if (isMy)
            mDeckUI.AddCardToDeck(deckArea, card, num);
        else
            oDeckUI.AddCardToDeck(deckArea, card, num);
    }
    #endregion

    #region 手牌的操作
    public void AddCardToHand(Group group, bool isMy)
    {
        List<string> list = group.GetCardIDList();
        if (isMy)
            mHandCardUI.AddCard(list, isMy);
        else
            oHandCardUI.AddCard(list, isMy);
    }

    public void AddCardToHand(Card card, bool isMy)
    {
        string id = card.cardID;
        if (isMy)
            mHandCardUI.AddCard(id, isMy);
        else
            oHandCardUI.AddCard(id, isMy);
    }

    public void RemoveCardFromHand(int val, bool isMy)
    {
        if (isMy)
            mHandCardUI.RemoveCard(val);
        else
            oHandCardUI.RemoveCard(val);
    }

    public void RemoveCardFromHand(List<int> val, bool isMy)
    {

        if (isMy)
            mHandCardUI.RemoveCard(val);
        else
            oHandCardUI.RemoveCard(val);
    }

    public void ShowHandCardDashAnim(List<int> val, bool isMy)
    {
        if (isMy)
            mHandCardUI.ShowCardDashAnim(val,isMy);
        else
            oHandCardUI.ShowCardDashAnim(val,isMy);
    }
    public void HideHandCardDashAnim(bool isMy)
    {
        if (isMy)
            mHandCardUI.HideCardDashAnim();
        else
            oHandCardUI.HideCardDashAnim();
    }
    #endregion

    #region 场地卡片的加入移除操作

    public int GetAreaRank(bool isInMonsterArea, bool isMy)
    {
        if (isMy)
            return mFieldMgr.GetCardRank(isInMonsterArea);
        else
            return oFieldMgr.GetCardRank(isInMonsterArea);
    }

    public int AddToArea(string cardID, int putType, FieldUIType type, bool isMy)
    {
        if (isMy)
            return mFieldMgr.AddCard(cardID, putType, type);
        else
            return oFieldMgr.AddCard(cardID, putType, type);
    }


    public void RemoveCardFromField(Card card, bool isMy)
    {
        if (isMy)
            mFieldMgr.RemoveCard(card);
        else
            oFieldMgr.RemoveCard(card);
    }

    public void ChangeMonsterPlaseType(int val, int type, bool isMy, string cardID)
    {
        if (isMy)
            mFieldMgr.ChangeMonsterPlaseType(val, type, cardID);
        else
            oFieldMgr.ChangeMonsterPlaseType(val, type, cardID);
    }

    public void ChangeTrapPhaseType(int val, int type, bool isMy, string cardID)
    {
        if (isMy)
            mFieldMgr.ChangeTrapPlaseType(val, type, cardID);
        else
            oFieldMgr.ChangeTrapPlaseType(val, type, cardID);
    }

    #endregion


    public void InitBothLPUI(string mAccount, string oAccount)
    {
        mLpSliderUI.Init(mAccount);
        oLpSliderUI.Init(oAccount);
    }


    public void ShowLpChange(float val, bool isAdd)
    {
        if (val == 0)
        {
            return;
        }
        lpChangeUI.ShowLPChange(val, isAdd);
    }

    public void ShowSummonAnim(Card card, normalDele dele)
    {
        cardEffectAnim.ShowAnim(card, dele);
    }

    public void ShowEffectAnim(Card card, normalDele dele)
    {
        cardEffectAnim.ShowEffectAnim(card, dele);
    }


    public void ChangeToPhase(int phase)
    {
        phaseButtonMgr.ChangeToPhase(phase);
    }


    public void UpdateCardMesShow(FieldCardGroup group, FieldCardGroup group1)
    {
        mFieldMgr.UpdateCardMesShow(group);
        oFieldMgr.UpdateCardMesShow(group1);
    }

    public void UpdateCardAttackAnimShow(FieldCardGroup group, bool isMy)
    {
        if (isMy)
            mFieldMgr.UpdateCardAttackAnimShow(group);
        else
            oFieldMgr.UpdateCardAttackAnimShow(group);
    }

    public void HideCardAttackAnimShow(FieldCardGroup group, bool isMy)
    {
        if (isMy)
            mFieldMgr.HideCardAttackAnim(group);
        else
            oFieldMgr.HideCardAttackAnim(group);
    }



    public void ShowFloatText(string mes, FloatTextDele dele)
    {
        floatText.Show(mes, dele);
    }


    public void StartGuessFirst(GuessDele dele)
    {
        guessFirst.Show(dele);
    }

    public void PhaseCanControl()
    {
        phaseButtonMgr.SetCanControl();
    }

    public void PhaseNotControl()
    {
        phaseButtonMgr.SetNotControl();
    }

    public void ShowSelectPutType(string cardID, BoolDele dele, bool isMy)
    {
        selectPutType.ShowSelectPutType(cardID, dele, isMy);
    }

    public void ShowFieldDashAnim(List<int> list,  int fieldArea,bool isMy)
    {
        if (isMy)
            mFieldMgr.ShowDashAnim(list, (FieldUIType)fieldArea, isMy);
        else
            oFieldMgr.ShowDashAnim(list, (FieldUIType)fieldArea, isMy);
    }

    public void HideFieldDashAnim(bool isMy)
    {
        if (isMy)
            mFieldMgr.HideDashAnim();
        else
            oFieldMgr.HideDashAnim();
    }

    public void ShowActAnim(int area)
    {
        mDeckUI.ShowDeckAct(area);
    }

    public void HideActAnim(int area)
    {
        mDeckUI.HideDeckAct(area);
    }

    public void UpdateXYZMaterial(List<string> cardIDList, int rank,bool isMy)
    {
        if(isMy)
        {
            mFieldMgr.UpdateXYZCard(cardIDList, rank);
        }
        else
        {
            oFieldMgr.UpdateXYZCard(cardIDList, rank);
        }
    }

    #region 动画
    public void ShowDrawAnim(string id, normalDele dele, int count, bool isMy)
    {
        if (isMy)
            mDrawCardAnim.Play(id, dele, count);
        else
            oDrawCardAnim.Play(id, dele, count);
    }

    public void ShowAttackAnim(int Attacker_areaRank, int Attackeder_areaRank, bool isMy, normalDele dele)
    {
        duel.SetIsAnim(true);
        attackAnim.Show(Attacker_areaRank, Attackeder_areaRank, isMy, dele);
    }
    #endregion


    public void ReduceLp(int val, bool isMy)
    {
        if (isMy)
            mLpSliderUI.ReduceLp(val);
        else
            oLpSliderUI.ReduceLp(val);
    }

    public void AddLp(int val, bool isMy)
    {
        if (isMy)
            mLpSliderUI.AddLp(val);
        else
            oLpSliderUI.AddLp(val);
    }

    public void UpdateRoundCount(int val)
    {
        roundCounterUI.SetRoundCount(val);
    }

    public void ShowMes(string title, string mes, normalDele dele)
    {
        tipPlane.Show(mes, dele, title);
    }

    public Vector3 GetCardAreaPos(Card card)
    {
        int area = card.curArea;
        bool isMy = card.ownerPlayer.isMy;
        switch (area)
        {
            case ComVal.Area_Monster:
            case ComVal.Area_NormalTrap:
            case ComVal.Area_FieldSpell:
                if (isMy)
                {
                    return mFieldMgr.GetAreaPos(area, card.areaRank);
                }
                else
                {
                    return oFieldMgr.GetAreaPos(area, card.areaRank);
                }
            case ComVal.Area_Remove:
                if (isMy)
                {
                    return mDeckUI.GetAreaPos(area);
                }
                else
                {
                    return oDeckUI.GetAreaPos(area);
                }
            case ComVal.Area_Graveyard:
                if (isMy)
                {
                    return mDeckUI.GetAreaPos(area);
                }
                else
                {
                    return oDeckUI.GetAreaPos(area);
                }
            case ComVal.Area_Hand:
                if (isMy)
                {
                    return mHandCardUI.GetComponent<RectTransform>().anchoredPosition3D;
                }
                else
                {
                    return oHandCardUI.GetComponent<RectTransform>().anchoredPosition3D;
                }
            default:
                Debug.Log(area);
                Debug.Log("???");
                break;
        }
        return Vector3.zero;
    }


    public void UpdateChainUI(Chain val)
    {
        chainUICtr.UpdateChainUI(val);
    }

    public void PlayChainAnim(normalDele dele)
    {
        chainUICtr.PlayChainAnim(dele);
    }

    #region 回调方法

    public void SelectFieldCard(Group group, int num, GroupCardSelectBack dele, bool isMy,bool isMax)
    {
        selectCardMgr.SelectCardFromGroup(group, dele, num, isMy,isMax);
    }


    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="str"></param>
    /// <param name="dele"></param>
    public void ShowDialogBoxUI(string str, GetMes dele, bool isMy)
    {
        dialogBoxUI.ShowDialogBox(str, dele, isMy);
    }

    /// <summary>
    /// 显示选择卡片组对话框
    /// </summary>
    /// <param name="cardGroup"></param>
    /// <param name="callBack"></param>
    /// <param name="num"></param>
    public void ShowSelectCardUI(Group cardGroup, GroupCardSelectBack callBack, int num, bool isMy,bool isMax)
    {
        selectCardUI.ShowSelectCardUI(cardGroup, callBack, num, isMy,isMax);
    }

    public void ShowSelectEffectUI(IntDele dele, List<string> effectList, bool isMy)
    {
        selectEffectUI.ShowSelectEffectDialogBox(dele, effectList, isMy);
    }

    public void ShowChangeAreaAnim(string cardID, int fromArea, int toArea, int fromType, int toType, bool isMy, int fromRank = -1, int toRank = -1)
    {
        if (isMy)
        {
            mChangeAreaAnim.PlayAnim(cardID, fromArea, toArea, fromType, toType, fromRank, toRank);
        }
        else
        {
            oChangeAreaAnim.PlayAnim(cardID, fromArea, toArea, fromType, toType, fromRank, toRank);
        }
    }
    #endregion


    #region 被ui调用

    public RectTransform GetHandCardRect(int rank, bool isMy)
    {
        if (isMy)
        {
            return mHandCardUI.GetCardRectRank(rank);
        }
        else
        {
            return oHandCardUI.GetCardRectRank(rank);
        }
    }


    public Vector3 GetReturnHandCardPos(int rank, bool isMy)
    {
        if (isMy)
        {
            return mHandCardUI.GetReturnHandCardPos(rank);
        }
        else
        {
            return oHandCardUI.GetReturnHandCardPos(rank);
        }
    }

    public RectTransform GetHandCardUI(bool isMy)
    {
        if(isMy)
        {
            return mHandCardUI.GetComponent<RectTransform>();
        }
        else
        {
            return oHandCardUI.GetComponent<RectTransform>();
        }
    }

    #endregion



}


    


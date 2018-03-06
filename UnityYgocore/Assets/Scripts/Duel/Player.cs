using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用来记录玩家的状态
/// <para>记录限制效果的检测</para>
/// </summary>
public class Player
{

    List<LimitPlayerEffect> limitEffectList;

    bool canSpSummon;

    /// <summary>
    /// 普通召唤次数
    /// </summary>
    int maxSummonTime;
    /// <summary>
    /// 已普通召唤次数
    /// </summary>
    int summonedTime;
    /// <summary>
    /// 生命值
    /// </summary>
    public int LP;

    public bool isMy;

    public Group group_MainDeck;
    public Group group_ExtraDeck;
    public Group group_Graveyard;
    public Group group_Remove;
    public Group group_HandCard;
    public Card fieldSpell;
    public FieldCardGroup group_MonsterCard;
    public FieldCardGroup group_TrapCard;
    public string name;

    Duel duel;

    Player otherPlayer;

    EffectLauchLimitCounter effectLauchLimitCounter;

    List<SpSummonEvent> curRoundSpSummonEventList;


    public int MainDeckNum
    {
        get { return group_MainDeck.GroupNum; }
    }


    public Player(bool _isMy, string _name)
    {
        name = _name;
        isMy = _isMy;
        LP = 8000;
        maxSummonTime = 1;
        summonedTime = 0;
        canSpSummon = true;
        group_ExtraDeck = new Group();
        group_Graveyard = new Group();
        group_MainDeck = new Group();
        group_Remove = new Group();
        group_HandCard = new Group();
        group_MonsterCard = new FieldCardGroup(5);
        group_TrapCard = new FieldCardGroup(5);
        limitEffectList = new List<LimitPlayerEffect>();
        duel = Duel.GetInstance();
        effectLauchLimitCounter = new EffectLauchLimitCounter();
        curRoundSpSummonEventList=new List<SpSummonEvent>();
    }

    public void SetOtherPlayer(Player _player)
    {
        otherPlayer = _player;
    }


    private List<LimitPlayerEffect> GetFiterLimitEffectList(int val, Card targetCard = null)
    {
        limitEffectList = duel.GetLimitEffectList(this);
        List<LimitPlayerEffect> list = new List<LimitPlayerEffect>();
        foreach (var item in limitEffectList)
        {
            if (item.IsBindLimitEffectType(val) && item.IsValid(targetCard))
            {

                list.Add(item);
            }
        }
        return list;
    }

    public void ReduceLP(float val, bool isCheckLost = true)
    {
        LP = (int)(LP - val);
        LP = LP.GetPositive();
        if (isCheckLost)
        {
            CheckLost();
        }
    }

    public void CheckLost()
    {
        if (LP == 0)
        {
            Duel.GetInstance().LostMatch(this);
        }
    }

    public void AddLP(int val)
    {
        LP = LP + val;
    }
    public void SetCanNotSpSummon()
    {
        canSpSummon = false;
    }



    #region  更新玩家数据
    public void AddEffectCounter(string effectID, int maxLuachTime)
    {
        effectLauchLimitCounter.AddCounter(effectID, maxLuachTime);
    }

    public void AddEffectLauchTime(string effectID)
    {
        effectLauchLimitCounter.AddLauchTime(effectID);
    }

    public void AddNormalSummontime()
    {
        summonedTime++;
    }

    public void AddSpSummonEvent(SpSummonEvent val)
    {
        curRoundSpSummonEventList.Add(val);
    }

    public void EndRoundReset()
    {
        maxSummonTime = 1;
        summonedTime = 0;
        foreach (var item in group_Graveyard.cardList)
        {
            item.RoundEndReset();
        }
        foreach (var item in group_Remove.cardList)
        {
            item.RoundEndReset();
        }
        foreach (var item in group_MonsterCard.GetCardList())
        {
            item.RoundEndReset();
        }
        foreach (var item in group_TrapCard.GetCardList())
        {
            item.RoundEndReset();
        }
        if (fieldSpell != null)
        {
            fieldSpell.RoundEndReset();
        }
    }

    public void ResetCounter()
    {
        effectLauchLimitCounter.ResetCounter();
        curRoundSpSummonEventList.Clear();
    }
    #endregion

    #region 是否允许
    public bool CanLauchEffect(LauchEffect e)
    {
        if (e.effectID != null)
        {
            if (!effectLauchLimitCounter.CheckCanLauch(e.effectID))
            {
                return false;
            }
        }

        if (e.category.IsBind(ComVal.category_search) && !CanSearchCard(e.ownerCard))
        {
            return false;
        }

        if(e.category.IsBind(ComVal.category_spSummon|ComVal.category_fusionSummon)&&!CanSpSummon())
        {
            return false;
        }

        if (e.ownerCard.IsTrap())
        {
            return CanLuachTrapEffect();
        }
        else if (e.ownerCard.IsMonster())
        {
            return CanLauchMonsterEffect();
        }
        else if (e.ownerCard.isSpell())
        {
            return CanLauchMagicEffect();
        }
        else
        {
            Debug.Log("error");
            return false;
        }
    }

    private bool CanSearchCard(Card targetCard)
    {
        List<LimitPlayerEffect> list = GetFiterLimitEffectList(ComVal.limitEffectType_unableSearchCardFromMainDeck, targetCard);
        return list.Count == 0;
    }

    private bool CanLauchMonsterEffect()
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_unableLauchMonsterEffect).Count == 0;
    }

    private bool CanLauchMagicEffect()
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_unableLauchMagic).Count == 0;
    }

    private bool CanLuachTrapEffect()
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_unableLauchTrap).Count == 0;
    }

    public bool CanNormalSummon()
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_unableNormalSummon).Count == 0
            && summonedTime < maxSummonTime;
    }

    public bool CanAttack(Card c)
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_unableAttack).Count == 0;
    }

    public bool CanSendToGraveyard()
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_sendToRemove).Count == 0;
    }

    /// <summary>
    /// 是否可以特殊召唤
    /// <para>有一张卡可特殊召唤返回真</para>
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public bool CanSpSummon(Group group)
    {
        if (GetFiterLimitEffectList(ComVal.limitEffectType_unableSpSummon).Count == 0)
        {
            List<LimitPlayerEffect> list = GetFiterLimitEffectList(ComVal.limitEffectType_spSummonLimit);
            for (int i = 0; i < group.GroupNum; i++)
            {
                if (CanSpSummon(group.GetCard(i)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CanSpSummon(Card card)
    {
        if (GetFiterLimitEffectList(ComVal.limitEffectType_unableSpSummon).Count == 0)
        {
            List<LimitPlayerEffect> list = GetFiterLimitEffectList(ComVal.limitEffectType_spSummonLimit);
            for (int i = 0; i < list.Count; i++)
            {
                if ((bool)limitEffectList[i].LimitEffectHandle(card))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    private bool CanSpSummon()
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_unableSpSummon).Count == 0;
    }

    public bool WhetherBeSpSummon()
    {
        return curRoundSpSummonEventList.Count > 0;
    }

    #endregion

    #region 获取数据

    public int GetHandCardNum()
    {
        return group_HandCard.GroupNum;
    }

    public Group GetHandCardGroup()
    {
        return group_HandCard;
    }


    public Card GetSendToRemoveReasonCard()
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_sendToRemove)[0].ownerCard;
    }

    public LimitPlayerEffect GetSendToRemoveReasonEffect()
    {
        return GetFiterLimitEffectList(ComVal.limitEffectType_sendToRemove)[0];
    }

    public Group GetCanAttackMonster()
    {
        List<LimitPlayerEffect> effects = GetFiterLimitEffectList(ComVal.limitEffectType_unableAttackTarget);
        List<Card> cards = otherPlayer.group_MonsterCard.GetCardList();
        List<Card> result = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (!cards[i].CanBeAttack())
            {
                cards.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < effects.Count; i++)
        {
            cards = effects[i].LimitEffectHandle(cards);
        }
        return new Group(cards);
    }


    public Card GetCard(int area, int rank)
    {
        Card card;
        if (area == ComVal.Area_Monster)
        {
            card = group_MonsterCard.GetCard(rank);
            return card;
        }
        else if (area == ComVal.Area_NormalTrap)
        {
            card = group_TrapCard.GetCard(rank);
            return card;
        }
        else if (area == ComVal.Area_Hand)
        {
            card = group_HandCard.GetCard(rank);
            return card;
        }
        else if (area == ComVal.Area_FieldSpell)
        {
            return fieldSpell;
        }
        else
        {
            Debug.Log("error");
            return Card.Empty();
        }
    }

    /// <summary>
    /// 获取可以特殊召唤的group
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public Group GetCanSpSummonGroup(Group group)
    {
        Group g = new Group();
        for (int i = 0; i < group.GroupNum; i++)
        {
            Card card = group.GetCard(i);
            if (CanSpSummon(card))
            {
                g.AddCard(card);
            }
        }
        return g;
    }

    /// <summary>
    /// 魔陷区的剩余格子数量
    /// </summary>
    public int GetLeftTrapAreaNum()
    {
        return 5 - group_TrapCard.GroupNum;
    }

    /// <summary>
    /// 怪兽区的剩余格子数量
    /// </summary>
    public int GetLeftMonsterAreaNum()
    {
        return 5 - group_MonsterCard.GroupNum;
    }
    #endregion
}

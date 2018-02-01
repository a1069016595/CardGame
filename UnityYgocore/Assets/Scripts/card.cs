using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// 游戏中的卡片
/// </summary>
public class Card
{
    public string cardID;
    public string cardName;
    public string cardDescribe;
    public int cardType;
    public int afk;
    public int def;
    public int level;
    public int race;
    public int attribute;

    public List<LauchEffect> lauchEffectList = new List<LauchEffect>();
    public SpSummonEffect spSummonEffect;//只有一个

    List<StateEffect> singleEffectList = new List<StateEffect>();
    List<StateEffect> equipEffectList = new List<StateEffect>();
    List<StateEffect> materialsXYZEffectList = new List<StateEffect>();
    List<StateEffect> materialSynchroEffectList = new List<StateEffect>();


   public  List<Card> materialsXYZCardList = new List<Card>();
   public List<Card> materialSynchroCardList = new List<Card>();
    List<Card> sacrificeCardList = new List<Card>();
    List<Card> equipCardList = new List<Card>();

    int curAfk;
    int curDef;
    int curLevel;
    int curAttribute;

    Duel duel;

    /// <summary>
    /// 已经攻击次数
    /// </summary>
    int cardAttackedTime;

    /// <summary>
    /// 可转换次数
    /// </summary>
    public int cardChangeTypeTime;

    /// <summary>
    /// 已经转换次数
    /// </summary>
    public int cardChangeTypeedTime;


    /// <summary>
    /// 魔法陷阱卡放置的回合数
    /// </summary>
    public int cardSetRound;


    /// <summary>
    /// 所在区域的顺序
    /// <para>仅在怪兽区域，手牌，魔陷区域，墓地区域，除外区域</para>
    /// </summary>
    public int areaRank;

    public int curArea;
    /// <summary>
    /// 当前卡片的放置状态
    /// </summary>
    public int curPlaseState;

    public Player ownerPlayer;
    public Player opponentPlayer;
    public Player controller;

    /// <summary>
    /// 限制特殊召唤的卡片
    /// </summary>
    public string limitSpSummonCardStr;

    public int fusionCount;

    public bool isMy;

    public CardMaterialFitler cardMaterialFitler;
    public Filter xyzFilter;
    public int xyzMaterialNum;
    public int previousArea;


    /// <summary>
    /// 在当前区域的回合数,区域变化后会初始化为0
    /// </summary>
    public int stayturn;

    public Reason changeAreaReason;//所在区域改变的原因

    List<EffectLauchCountLimit> effectLauchCountLimitList = new List<EffectLauchCountLimit>();

    public Card EffectDataCard;

    List<Pointer> curPointerList=new List<Pointer>();

    /// <summary>
    /// 生成卡片信息
    /// </summary>
    public void Init(bool _isMy)
    {
        if (IsMonster())
        {
            curAfk = afk;
            curDef = def;
            curLevel = level;
            curAttribute = attribute;
            cardAttackedTime = 0;
        }
        isMy = _isMy;
        duel = Duel.GetInstance();

        if (ComVal.isInExtra(cardType))
        {
            curArea = ComVal.Area_Extra;
        }
        else
        {
            curArea = ComVal.Area_MainDeck;
        }
    }

    public void AddAttackTime()
    {
        cardAttackedTime++;
    }

    public void AddSingleEffect(StateEffect effect)
    {
        singleEffectList.Add(effect);
    }

    public void AddEquipEffect(StateEffect effect)
    {
        equipEffectList.Add(effect);
        if (effect.targetCard == this)
        {
            equipCardList.Add(effect.equipCard);
        }
    }

    public void AddMaterialXYZEffect(StateEffect effect)
    {
        materialsXYZEffectList.Add(effect);
    }

    public void AddMaterialSynchroEffect(StateEffect effect)
    {
        materialSynchroEffectList.Add(effect);
    }

    public void ClearEffect()//改变区域时清除状态效果
    {
        for (int i = 0; i < singleEffectList.Count; i++)
        {
            if (!singleEffectList[i].IsBindCardEffectType(ComVal.cardEffectType_unableReset))
            {
              
                singleEffectList.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < equipEffectList.Count; i++)
        {
            if (!equipEffectList[i].IsBindCardEffectType(ComVal.cardEffectType_unableReset))
            {
                equipEffectList.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < materialsXYZEffectList.Count; i++)
        {
            if (!materialsXYZEffectList[i].IsBindCardEffectType(ComVal.cardEffectType_unableReset))
            {
                materialsXYZEffectList.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < materialSynchroEffectList.Count; i++)
        {
            if (!materialSynchroEffectList[i].IsBindCardEffectType(ComVal.cardEffectType_unableReset))
            {
                materialSynchroEffectList.RemoveAt(i);
                i--;
            }
        }
    }

    private List<StateEffect> GetFiterStateEffect(int val, int area)
    {
        List<StateEffect> result = new List<StateEffect>();

        foreach (var item in singleEffectList)
        {
            if (item.IsBindStateEffectType(val) && item.IsValid(this))
            {
                result.Add(item);
            }
        }
        foreach (var item in equipEffectList)
        {
            if (item.targetCard == this && item.IsBindStateEffectType(val) && item.IsValid(this))
            {
                if (item.equipCard.curArea.IsBind(item.rangeArea))
                {
                    result.Add(item);
                }
            }
        }
        foreach (var item in materialsXYZEffectList)
        {
            if (item.IsBindStateEffectType(val) && item.IsValid(this))
            {
                result.Add(item);
            }
        }
        foreach (var item in materialSynchroEffectList)
        {
            if (item.IsBindStateEffectType(val) && item.IsValid(this))
            {
                result.Add(item);
            }
        }
        return result;
    }

    #region 获取卡片属性
    public int GetBaseAfk()
    {
        //TODO :有些问题,多个效果时如何计算
        int result = afk;
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_ChangeAttack, curArea);

        foreach (var item in list)
        {
            result = item.GetStateEffectVal(this);
        }
        list = GetFiterStateEffect(ComVal.stateEffectType_ExchangeAfkDef, curArea);
        for (int i = 0; i < list.Count; i++)
        {
            result = def;
        }
        return result.GetPositive();
    }

    public int GetCurAfk()
    {
        int val = GetBaseAfk();
        List<StateEffect> addAttacklist = GetFiterStateEffect(ComVal.stateEffectType_addAfkVal, curArea);
        List<StateEffect> reduceAttackList = GetFiterStateEffect(ComVal.stateEffectType_reduceAfkVal, curArea);

        foreach (var item in addAttacklist)
        {
            val += item.GetStateEffectVal(this);
        }

        foreach (var item in reduceAttackList)
        {
            val -= item.GetStateEffectVal(this);
        }
        return val.GetPositive();
    }

    public int GetBaseDef()
    {
        int result = def;
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_ChangeDef, curArea);
        foreach (var item in list)
        {
            result = item.GetStateEffectVal(this);
        }
        list = GetFiterStateEffect(ComVal.stateEffectType_ExchangeAfkDef, curArea);
        for (int i = 0; i < list.Count; i++)
        {
            result = afk;
        }
        return result.GetPositive();
    }

    public int GetCurDef()
    {
        int val = GetBaseDef();
        List<StateEffect> addDefList = GetFiterStateEffect(ComVal.stateEffectType_addDefVal, curArea);
        List<StateEffect> reduceDefList = GetFiterStateEffect(ComVal.stateEffectType_reduceDefVal, curArea);

        foreach (var item in addDefList)
        {
            val += item.GetStateEffectVal(this);
        }
        foreach (var item in reduceDefList)
        {
            val -= item.GetStateEffectVal(this);
        }
        return val.GetPositive();
    }

    public int GetBaseLevel()
    {
        int val = level;
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_ChangeLevel, curArea);
        for (int i = 0; i < list.Count; i++)
        {
            val = list[i].GetStateEffectVal(this);
        }
        return val.GetPositive();
    }

    public int GetCurLevel()
    {
        int val = GetBaseLevel();
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_AddLevel, curArea);
        foreach (var item in list)
        {
            val += item.GetStateEffectVal(this);
        }
        return val.GetPositive();
    }

    public int GetCurAttribute()
    {
        int val = attribute;
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_ChangeAttribute, curArea);
        foreach (var item in list)
        {
            val = item.GetStateEffectVal(this);
        }
        return val;
    }

    #endregion

    #region 卡片状态

    public bool CanDestroy()
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_resistDestroy, curArea);
        if (list.Count != 0)
        {
            return false;
        }
        return true;
    }

    public bool CanRemove()
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_resistRemove, curArea);
        if (list.Count != 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 是否可以特殊召唤
    /// </summary>
    /// <returns></returns>
    public bool CanSpSummon()
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_unableSpSummon, curArea);
        if (list.Count != 0)
        {
            return false;
        }
        return true;
    }

    public bool CanNormalSummon()
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_unableNormalSummon, curArea);
        if (list.Count != 0)
        {
            return false;
        }
        return true;
    }

    public int GetSacrificeNum()//祭品数
    {
        int num;

        int level = GetCurLevel();
        if (level > 4 && level <= 6)
        {
            num = 1;
        }
        else if (level > 6 && level <= 8)
        {
            num = 2;
        }
        else if (level > 8)
        {
            num = 3;
        }
        else
        {
            num = 0;
        }

        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_addSacrifice, curArea);
        for (int i = 0; i < list.Count; i++)
        {
            num += list[i].GetStateEffectVal(this);
        }

        List<StateEffect> list1 = GetFiterStateEffect(ComVal.stateEffectType_changeSacrifice, curArea);
        if (list1.Count != 0)
        {
            num = list1[0].GetStateEffectVal(this);
        }
        return num;
    }

    public bool CanBeSacrifice()//能被解放
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_unableSacrifice | ComVal.stateEffectType_unableRelease, curArea);
        return list.Count > 0;
    }

    public bool CanAttack()
    {
        if (duel.roundCount == 1)
        {
            return false;
        }
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_unableAttack, curArea);
        if (list.Count != 0)
        {
            return false;
        }
        if (GetCurAttackTime() > 0 && curPlaseState == ComVal.CardPutType_UpRightFront && controller.CanAttack(this))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsViliadAttack()
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_unableAttack, curArea);
        if (list.Count != 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private int GetCurAttackTime()
    {
        int val = 1;
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_addAttackTime, curArea);
        foreach (var item in list)
        {
            val += item.GetStateEffectVal(this);
        }
        return val - cardAttackedTime;
    }

    public bool CanPierce()//穿透
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_Pierce, curArea);
        return list.Count != 0;
    }

    public bool CanChangeType()
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_unableChangeType, curArea);
        return list.Count == 0;
    }

    public bool CanBeAttack()
    {
        List<StateEffect> list = GetFiterStateEffect(ComVal.stateEffectType_unableBeAttack, curArea);
        return list.Count == 0;
    }

    #endregion

    public void SetCardCountLimit(LauchEffect[] list, int val)
    {
        List<LauchEffect> l = new List<LauchEffect>(list);
        EffectLauchCountLimit limit = new EffectLauchCountLimit(l, val);
        effectLauchCountLimitList.Add(limit);
    }

    public void AddCardEffectLauchTime(LauchEffect effect)
    {
        foreach (var item in effectLauchCountLimitList)
        {
            item.AddLauchCount(effect);
        }
    }

    public bool CanLauchEffect(LauchEffect e)
    {
        foreach (var item in effectLauchCountLimitList)
        {
            if (!item.CanLauchEffect(e))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 设置2个融合素材条件
    /// </summary>
    /// <param name="f"></param>
    /// <param name="f1"></param>
    public void SetMaterialFilter2(Filter f, Filter f1)
    {
        cardMaterialFitler = new CardMaterialFitler(f, f1);
        fusionCount = 2;
    }

    public void SetXYZMaterialFiter(Filter filter,int num)
    {
        xyzFilter = filter;
        xyzMaterialNum = num;
    }


    public bool CanFusion(Group group)
    {
        if (cardMaterialFitler == null)
        {
            return false;
        }
        List<Card> list1 = new List<Card>();
        List<Card> list2 = new List<Card>();
        for (int i = 0; i < group.GroupNum; i++)
        {
            Card card = group.GetCard(i);
            if (cardMaterialFitler.cardFilter1(card))
            {
                list1.Add(card);
            }
        }
        for (int i = 0; i < group.GroupNum; i++)
        {
            Card card = group.GetCard(i);
            if (cardMaterialFitler.cardFilter2(card))
            {
                list2.Add(card);
            }
        }
        if (list1.Count >= 1 && list2.Count >= 1)
        {
            if (list1.Count == 1 && list2.Count == 1)
            {
                if (list2[0] == list1[0])
                    return false;
            }
            return true;
        }
        else
            return false;
    }

    public void SetLimitSpSummonCardStr(string str)
    {
        limitSpSummonCardStr = str;
    }

    public void SetPlaseState(int val)
    {
        curPlaseState = val;
    }

    public Card(string _cardID, string _cardName, string _cardDescribe, int _cardType)
    {
        cardID = _cardID;
        cardName = _cardName;
        cardDescribe = _cardDescribe;
        cardType = _cardType;
    }

    /// <summary>
    /// 设置怪兽所有属性
    /// </summary>
    /// <param name="_attribute"></param>
    public void SetMonsterAttribute(int _attribute, int _race, int _level, int _afk, int _def)
    {
        attribute = _attribute;
        level = _level;
        race = _race;
        afk = _afk;
        def = _def;
    }


    public static Card Empty()
    {
        Card card = new Card(null, null, null, 0);
        return card;
    }

    public static bool IsEmpty(Card card)
    {
        if (card.cardID == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsMonster()
    {
        if (ComVal.isBind(cardType, ComVal.cardType_Monster))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsTrap()
    {
        if (ComVal.isBind(cardType, ComVal.cardType_Trap))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsForverSpellTrapCard()
    {
        return cardType.IsBind(ComVal.CardType_Spell_Continuous | ComVal.CardType_Spell_Equit | ComVal.CardType_Spell_Field
                                | ComVal.CardType_Trap_Continuous);
    }


    public bool isSpell()
    {
        if (ComVal.isBind(cardType, ComVal.cardType_Spell))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddEffect(BaseEffect _effect)
    {
        if (_effect.IsBindType(EffectType.LauchEffect))
        {
            lauchEffectList.Add((LauchEffect)_effect);
        }
        else if (_effect.IsBindType(EffectType.SpsummonEffect))
        {
            spSummonEffect = (SpSummonEffect)_effect;
        }
    }

    /// <summary>
    /// 只要有一个效果可以发动 返回真
    /// <para>且不存在于连锁当中</para>
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private bool MCanLauchEffect(Code code, Chain chain, int cardEffectType)
    {
        if (IsTrap())
        {
            if (curArea == ComVal.Area_NormalTrap && curPlaseState == ComVal.CardPutType_UpRightBack)
            {
                if (cardSetRound == 0)
                {
                    return false;
                }
            }
        }
        else if (isSpell())
        {
            if (curArea == ComVal.Area_NormalTrap && curPlaseState == ComVal.CardPutType_UpRightBack && cardType == ComVal.CardType_Spell_Quick)
            {
                if (cardSetRound == 0)
                {
                    return false;
                }
            }
        }
        int effectType = cardEffectType;
        foreach (var item in lauchEffectList)
        {
            if (ComVal.isBind(code.code, item.code))
            {
                if (item.CanBeLaunch(code) && item.IsBindCardEffectType(effectType))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CanLauchEffect(Code code, Chain chain, int cardEffectType)
    {
        return MCanLauchEffect(code, chain, cardEffectType);
    }
    public bool CanLauchEffect(Code code, Chain chain)
    {
        return MCanLauchEffect(code, chain, ComVal.cardEffectType_lauchEffect);
    }
    /// <summary>
    /// 获取可以发动的效果
    /// <para>不存在于连锁之中</para>
    /// </summary>
    /// <returns></returns>
    public List<LauchEffect> GetCanLauchEffectList(Code code, Chain chain)
    {
        List<LauchEffect> list = new List<LauchEffect>();
        foreach (var item in lauchEffectList)
        {
            if (chain.isDelayCode)
            {
                if ((ComVal.isBind(item.code, chain.codeVal) && item.IsBindCardEffectType(ComVal.cardEffectType_chooseLauch)))
                {
                    continue;
                }
            }
            if (item.CanBeLaunch(code) && !chain.ContainEffect(item.code, this, item))
            {
                list.Add(item);
            }
        }
        return list;
    }

    public bool CanLauchTriggerEffect(Code code, Chain chain, bool isDelayode = false)
    {
        int effectType;
        effectType = ComVal.cardEffectType_triggerEffect;
        if (IsHideInTrapArea())
        {
            if (cardSetRound == 0)
            {
                return false;
            }
        }
        if (isSpell() && curArea == ComVal.Area_Hand)
        {
            if (duel.curPlayer != ownerPlayer)
            {
                return false;
            }
        }
        if (IsTrap() && curArea == ComVal.Area_Hand)
        {
            return false;
        }
        foreach (var item in lauchEffectList)
        {
            if (isDelayode)
            {
                if ((ComVal.isBind(item.code, chain.codeVal) && item.IsBindCardEffectType(ComVal.cardEffectType_chooseLauch)))
                {
                    continue;
                }
            }
            if (ComVal.isBind(code.code, item.code))
            {

                if (item.CanBeLaunch(code) && !chain.ContainEffect(item.code, this, item) && item.IsBindCardEffectType(effectType))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 检测是否可以特殊召唤
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public bool CanSpSummon(Code code)
    {
        if(spSummonEffect!=null)
        {
            if(spSummonEffect.CheckLauch(duel,this,null,code))
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 获取必须发动的效果
    /// </summary>
    /// <param name="code"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    public List<LauchEffect> GetMustLauchEffectList(Code code, Chain chain)
    {
        List<LauchEffect> list = new List<LauchEffect>();
        foreach (var item in lauchEffectList)
        {
            if (item.cardEffectType.IsBind(ComVal.cardEffectType_mustLauch))
            {
                if (item.CanBeLaunch(code) && !chain.ContainEffect(item.code, this, item))
                {
                    list.Add(item);
                }
            }
        }
        return list;
    }

    public bool ContainName(string val)
    {
        return cardName.Contains(val);
    }
    /// <summary>
    /// 设置区域 初始化stayTurn的值 清除状态效果
    /// </summary>
    /// <param name="_curArea"></param>
    public void SetArea(int _curArea, Reason reason)
    {
        previousArea = curArea;
        curArea = _curArea;
        stayturn = 0;
        changeAreaReason = reason;
        ClearEffect();
    }

    public void SetAreaRank(int _areaRank)
    {
        areaRank = _areaRank;
    }

    /// <summary>
    /// 初始化卡片设置回合数
    /// </summary>
    public void SetCardSetRound()
    {
        cardSetRound = 0;
    }

    public void SetPlayer(Player thePlayer)
    {
        ownerPlayer = thePlayer;
        controller = ownerPlayer;
    }

    public void SetOpponentPlayer(Player player)
    {
        opponentPlayer = player;
    }


    public bool IsFaceUp()
    {
        return curPlaseState == ComVal.CardPutType_layFront || curPlaseState == ComVal.CardPutType_UpRightFront;
    }

    public bool IsFaceUpInMonsterArea()
    {
        return IsFaceUp() && IsInField();
    }

    public bool IsAttack()
    {
        return curPlaseState == ComVal.CardPutType_UpRightFront;
    }

    public void RoundEndReset()
    {
        foreach (var item in effectLauchCountLimitList)
        {
            item.ResetLauchCount();
        }
        stayturn++;
        if (ComVal.isBind(curArea, ComVal.Area_Field))
        {
            cardSetRound++;
        }
        else
        {
            cardSetRound = 0;
        }
        if (IsMonster())
        {
            cardChangeTypeTime = 1;
            cardChangeTypeedTime = 0;
            cardAttackedTime = 0;
            cardAttackedTime = 0;
        }
    }

    public bool IsHideInTrapArea()
    {
        return curArea == ComVal.Area_NormalTrap && (IsTrap() || isSpell()) && curPlaseState == ComVal.CardPutType_UpRightBack;
    }

    public bool IsInField()
    {
        return curArea.IsBind(ComVal.Area_Field);
    }


    public bool IsAdjust()
    {
        return cardType.IsBind(ComVal.CardType_Monster_Adjust);
    }

    public bool IsAdjustOutSide()
    {
        return !cardType.IsBind(ComVal.CardType_Monster_Adjust | ComVal.CardType_Monster_XYZ);
    }

    public bool CanBeXYZMaterial()
    {
        return !cardType.IsBind( ComVal.CardType_Monster_XYZ);
    }

    public void SetXYZMaterial(Group g)
    {
        materialsXYZCardList = g.cardList;
    }

    public void SetSynchroMaterial(Group g)
    {
        materialSynchroCardList = g.cardList;
    }

    public int XyzMaterialNum()
    {
        return materialsXYZCardList.Count;
    }

    #region  装备卡相关 

    public void ClearEquipCard()
    {
        equipCardList.Clear();
    }

    public Group GetEquipGroup()
    {
        Group result = new Group(equipCardList);
        return result;
    }

    public bool IsEquipCard()
    {
        for (int i = 0; i < equipEffectList.Count; i++)
        {
            if (equipEffectList[i].equipCard == this && equipEffectList[i].equipCard.curArea.IsBind(ComVal.Area_NormalTrap))
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region 指示物相关

    public void AddPointer(string pointerName,int num,int max)
    {
        foreach (var item in curPointerList)
        {
            if(item.name==pointerName)
            {
                item.AddVal(num);
                return;
            }
        }
        curPointerList.Add(new Pointer(pointerName,max));
        AddPointer(pointerName, num,max);
    }

    public void RemovePoint(string pointerName, int num)
    {
        foreach (var item in curPointerList)
        {
            if (item.name == pointerName)
            {
                item.RemoveVal(num);
                return;
            }
        }
    }

    public bool HavePointer(string pointerName, int num)
    {
        foreach (var item in curPointerList)
        {
            if (item.name == pointerName)
            {
                 if(item.num >= num)
                 {
                     return true;
                 }
            }
        }
        return false;
    }

    #endregion
}

/// <summary>
/// 卡片发动限制，回合结束重置
/// </summary>
public class EffectLauchCountLimit
{
    List<LauchEffect> list;
    int count;

    int lauchCount;

    public EffectLauchCountLimit(List<LauchEffect> _list, int _count)
    {
        list = _list;
        count = _count;
    }

    public bool CanLauchEffect(LauchEffect e)
    {
        if (list.Contains(e))
        {
            return lauchCount < count;
        }
        return true;
    }

    public void AddLauchCount(LauchEffect e)
    {
        if (list.Contains(e))
        {
            lauchCount++;
        }
    }

    public void ResetLauchCount()
    {
        lauchCount = 0;
    }
}

public class Pointer
{
    public string name;
    public int num;
    public int max;

    public Pointer(string name,int max)
    {
        this.name = name;
        num = 0;
        this.max = max;
    }

    public void AddVal(int val)
    {
        num += val;
        num = Math.Min(max, num);
    }

    public void RemoveVal(int val)
    {
        num -= val;
        num = Math.Max(0, num);
    }
}



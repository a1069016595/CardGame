using System;

//统计代码行数 b*[^:b#/]+.*$
public static class ComVal
{
    #region 卡片类型

    public const int CardType_Monster_Normal = 0x1;
    public const int CardType_Monster_Effect = 0x2;
    public const int CardType_Monster_Adjust = 0x4;
    /// <summary>
    /// 融合怪兽
    /// </summary>
    public const int CardType_Monster_Fusion = 0x8;
    /// <summary>
    /// 同调怪兽
    /// </summary>
    public const int CardType_Monster_Synchro = 0x10;
    /// <summary>
    /// 超量怪兽
    /// </summary>
    public const int CardType_Monster_XYZ = 0x20;
    public const int CardType_Monster_Double = 0x2000;


    public const int CardType_Spell_Normal = 0x40;
    public const int CardType_Spell_Quick = 0x80;
    public const int CardType_Spell_Continuous = 0x100;
    public const int CardType_Spell_Equit = 0x200;
    public const int CardType_Trap_Normal = 0x400;
    public const int CardType_Trap_StrikeBack = 0x800;
    public const int CardType_Trap_Continuous = 0x1000;//永续陷阱
    public const int CardType_Spell_Field = 0x4000;//场地魔法

    public const int cardType_Monster = CardType_Monster_Adjust | CardType_Monster_Effect | CardType_Monster_Fusion | CardType_Monster_Normal
      | CardType_Monster_Synchro | CardType_Monster_XYZ | CardType_Monster_Double;
    public const int cardType_Spell = CardType_Spell_Continuous | CardType_Spell_Equit | CardType_Spell_Normal |
        CardType_Spell_Quick | CardType_Spell_Field;
    public const int cardType_Trap = CardType_Trap_Continuous | CardType_Trap_Normal | CardType_Trap_StrikeBack;

    public const int cardType_Extra = CardType_Monster_Synchro | CardType_Monster_XYZ | CardType_Monster_Fusion;

    #endregion

    #region 卡片种族
    public const int CardRace_Dragon = 0x1;
    public const int CardRace_Zombie = 0x2;
    public const int CardRace_Fiend = 0x4;
    public const int CardRace_Pyro = 0x8;
    public const int CardRace_SeaSerpent = 0x10;
    public const int CardRace_Rock = 0x20;
    public const int CardRace_Machine = 0x40;
    public const int CardRace_Fish = 0x80;
    public const int CardRace_Dinosaur = 0x100;
    public const int CardRace_Insect = 0x200;
    public const int CardRace_Beast = 0x400;
    public const int CardRace_BeastWarrior = 0x800;
    public const int CardRace_Plant = 0x1000;
    public const int CardRace_Aqua = 0x2000;
    public const int CardRace_Warrior = 0x4000;//战士
    public const int CardRace_WingedBeast = 0x8000;
    public const int CardRace_Fairy = 0x10000;
    public const int CardRace_Spellcaster = 0x20000;
    public const int CardRace_Thunder = 0x40000;
    public const int CardRace_Reptile = 0x80000;
    #endregion

    #region 卡片属性
    public const int CardAttr_Light = 0x01;
    public const int CardAttr_Dark = 0x02;
    public const int CardAttr_Fire = 0x04;
    public const int CardAttr_Water = 0x08;
    public const int CardAttr_Wind = 0x10;
    public const int CardAttr_Earth = 0x20;
    #endregion

    #region 卡片摆放形态
    /// <summary>
    /// 正面竖放
    /// </summary>
    public const int CardPutType_UpRightFront = 1;
    /// <summary>
    /// 正面横放
    /// </summary>
    public const int CardPutType_layFront = 2;

    /// <summary>
    /// 背面横放
    /// </summary>
    public const int CardPutType_layBack = 3;
    /// <summary>
    /// 背面竖放
    /// </summary>
    public const int CardPutType_UpRightBack = 4;
    #endregion

    #region 游戏阶段
    public const int Phase_Drawphase = 0x1;
    public const int Phase_Standbyphase = 0x2;
    public const int Phase_Mainphase1 = 0x4;
    public const int Phase_Battlephase = 0x8;
    public const int Phase_Mainphase2 = 0x10;
    public const int Phase_Endphase = 0x20;

    public const int Phase_Mainphase = Phase_Mainphase1 | Phase_Mainphase2;
    #endregion

    #region 卡片位置

    public const int Area_Trap = Area_FieldSpell | Area_NormalTrap;
    public const int Area_Hand = 0x2;
    public const int Area_Graveyard = 0x4;
    public const int Area_Remove = 0x8;
    public const int Area_MainDeck = 0x10;
    public const int Area_Extra = 0x20;
    public const int Area_Monster = 0x40;

    public const int Area_AllArea = Area_Extra | Area_Graveyard | Area_Hand | Area_MainDeck | Area_Monster | Area_Remove | Area_Trap;

    public const int Area_Field = Area_Monster | Area_Trap;
    public const int Area_XYZMaterial = 0x80;
    public const int Area_InSummon = 0x100;//召唤宣言时
    public const int Area_FieldSpell = 0x200;
    public const int Area_NormalTrap = 0x400;
    #endregion

    #region 时点
    public const Int64 code_EnterDrawPhase = 0x1;
    public const Int64 code_EnterStandByPhase = 0x2;
    public const Int64 code_EnterMainPhase1 = 0x4;
    public const Int64 code_EnterBattlePhase = 0x8;
    public const Int64 code_EnterMainPhase2 = 0x10;
    public const Int64 code_EnterEndPhase = 0x20;
    public const Int64 code_NormalSummon = 0x40;//普通召唤
    public const Int64 code_ToGraveyard = 0x80;//送去墓地
    public const Int64 code_SpecialSummon = 0x100;//特殊召唤
    public const Int64 code_Destroy = 0x200;//破坏
    public const Int64 code_Remove = 0x400;//除外
    public const Int64 code_TurnBack = 0x800;//翻转
    public const Int64 code_EffectLanch = 0x1000;//效果发动
    public const Int64 code_TurnBackSummon = 0x2000;//翻转召唤
    public const Int64 code_CalculateHarm = 0x4000;//计算伤害
    public const Int64 code_CauseHarm = 0x8000;//造成伤害
    public const Int64 code_GetHarm = 0x10000;//受到伤害
    public const Int64 code_Attack = 0x20000;//攻击时
    public const Int64 code_EliminateMonster = 0x40000;//消灭怪兽 
    public const Int64 code_AttackToDefence = 0x80000;//怪兽由攻击变为防守
    public const Int64 code_DefenceToAttack = 0x100000;//怪兽由防守变为攻击
    public const Int64 code_SpDeclaration = 0x200000;//特殊召唤宣言
    public const Int64 code_ReturnToHand = 0x400000;//返回手牌
    public const Int64 code_HighSummon = 0x800000;//上级召唤
    public const Int64 code_DirectAttack = 0x1000000;//直接攻击
    public const Int64 code_AddCardToHand = 0x2000000;//将卡片加入到手牌中
    public const Int64 code_DrawCard = 0x4000000;//抽卡
    public const Int64 code_NoCode = 0x8000000;
    public const Int64 code_AttackDeclaration = 0x10000000;//攻击宣言
    public const Int64 code_AddLp = 0x20000000;//增加lp
    public const Int64 code_ReduceLp = 0x40000000;//减少lp
    public const Int64 code_NSDeclaration = 0x80000000;//普通召唤宣言
    public const Int64 code_HSDeclaration = 0x100000000;//高级召唤宣言
    public const Int64 code_BeforeReckonAttack = 0x200000000;//伤害计算前
    public const Int64 code_AfterReckonAttack = 0x400000000;//伤害计算后
    public const Int64 code_ToMainDeck = 0x800000000;//伤害计算后
    //public const Int64 code_LeaveStandByPhase = 0x100000000;
    //public const Int64 code_LeaveMainPhase1 = 0x200000000;
    //public const Int64 code_LeaveBattlePhase = 0x400000000;
    //public const Int64 code_LeaveMainPhase2 = 0x800000000;
    //public const Int64 code_LeaveEndPhase = 0x1000000000;


    //public const Int64 code_FreeCode = code_NoCode | code_EnterDrawPhase | code_EnterStandByPhase | code_EnterMainPhase1 | code_EnterBattlePhase |
    //    code_EnterMainPhase2 | code_EnterEndPhase | code_EffectLanch;
    public const Int64 code_FreeCode = code_NoCode | code_EnterMainPhase1 | code_EffectLanch | code_AttackDeclaration;
    //普通召唤和上级召唤
    public const Int64 code_NotSpSummon = code_NormalSummon | code_HighSummon;

    public const Int64 code_LeaveField = code_ToGraveyard | code_Remove | code_ReturnToHand;//离场
    #endregion

    #region resetcode
    public const int resetEvent_LeaveDrawPhase = 0x1;
    public const int resetEvent_LeaveStandByPhase = 0x2;
    public const int resetEvent_LeaveMainPhase1 = 0x4;
    public const int resetEvent_LeaveBattlePhase = 0x8;
    public const int resetEvent_LeaveMainPhase2 = 0x10;
    public const int resetEvent_LeaveEndPhase = 0x20;
    public const int resetEvent_EndCaculateDamage = 0x40;//伤判结束
    //public const int resetEvent_ = 0x80;

    public const int resetEvent_Phase = resetEvent_LeaveBattlePhase | resetEvent_LeaveDrawPhase | resetEvent_LeaveEndPhase
        | resetEvent_LeaveMainPhase1 | resetEvent_LeaveMainPhase2 | resetEvent_LeaveStandByPhase;
    #endregion

    #region 卡牌效果类型
    public const int category_spSummon = 0x1;
    public const int category_toHand = 0x2;
    public const int category_search = 0x4;
    public const int category_position = 0x8;
    public const int category_destroy = 0x10;
    public const int category_toGrave = 0x20;
    public const int category_remove = 0x40;
    public const int category_draw = 0x80;

    public const int category_stateEffect = 0x100;//状态效果 改变卡片属性
    public const int category_fusionSummon = 0x200;
    public const int category_limitEffect = 0x400;//限制效果 用于限制玩家行为,如强欲而谦虚之壶
    public const int category_forever = 0x800;//永续效果
    public const int category_limitTime = 0x1000;//有限时间

    public const int category_disCard = 0x2000;
    public const int category_revived = 0x4000;//苏生
    public const int category_addAttackVal = 0x8000;//增加攻击力
    public const int category_drawCard = 0x10000;//增加攻击力
    public const int category_toMainDeck = 0x20000;//返回卡组
    public const int category_disAbleEffect = 0x40000;
    public const int category_disAbleSpSummon = 0x80000;
    public const int category_equipCard = 0x100000;
    #endregion

    #region 原因
    public const int reason_Effect = 0x1;
    public const int reason_AttackDestroy = 0x2;
    public const int reason_EffectDestroy = 0x4;
    public const int reason_SendToGraveyard = 0x8;
    public const int reason_SacrificeSummon = 0x10;
    public const int reason_Cost = 0x20;
    public const int reason_FusionMaterial = 0x40;
    public const int reason_FusionSummon = 0x80;
    public const int reason_LauchFinish = 0x100;   // 魔陷卡发动完效果后便送去墓地
    public const int reason_NormalDrawCard = 0x200;
    public const int reason_SynchroSummon = 0x400;
    public const int reason_InviadSpSummon = 0x800;
    public const int reason_Realease = 0x1000;
    public const int reason_Destroy = reason_EffectDestroy | reason_AttackDestroy;

    #endregion

    #region 卡牌效果发动类型
    public const int cardEffectType_mustLauch = 0x1;
    public const int cardEffectType_mustToChooseLauch = 0x2;//xxx的场合,延迟处理，不会被卡时点
    public const int cardEffectType_chooseLauch = 0x4;//xxx时,会被卡时点
    public const int cardEffectType_normalLauch = 0x8;

    public const int cardEffectType_normalStateEffect = 0x10;//通常状态效果
    public const int cardEffectType_equip = 0x20;//装备
    public const int cardEffectType_materialXYZ = 0x40;//xyz素材
    public const int cardEffectType_materialSynchro = 0x80;//同调素材

    public const int cardEffectType_Single = 0x100;//单体效果
    public const int cardEffectType_Multiple = 0x200;//群体效果
    public const int cardEffectType_field = 0x400;//场地

    //不加入连锁
    public const int cardEffectType_notInChain = 0x1000;

    public const int cardEffectType_unableReset = 0x2000;//改变区域会重置 用于状态效果

    public const int cardEffectType_lauchEffect = cardEffectType_mustLauch | cardEffectType_mustToChooseLauch | cardEffectType_chooseLauch
                                                | cardEffectType_normalLauch;

    public const int cardEffectType_triggerEffect = cardEffectType_mustLauch | cardEffectType_mustToChooseLauch | cardEffectType_chooseLauch;//触发效果
    #endregion

    #region 状态效果种类
    public const int stateEffectType_unableRelease = 1;//不能解放
    public const int stateEffectType_addAfkVal = 2;
    public const int stateEffectType_addDefVal = 3;
    public const int stateEffectType_reduceAfkVal = 4;
    public const int stateEffectType_reduceDefVal = 5;
    public const int stateEffectType_unableAttack = 6;//不能攻击
    public const int stateEffectType_unableSpSummon = 7;//不能特殊召唤
    public const int stateEffectType_limitSpSummon = 8;//只能由指定的卡特殊召唤
    public const int stateEffectType_unableNormalSummon = 9;//不能普通召唤
    public const int stateEffectType_ChangeAttribute = 10;//改变属性
    public const int stateEffectType_ChangeAttack = 11;//改变afk
    public const int stateEffectType_ChangeDef = 12;//改变def
    public const int stateEffectType_AddLevel = 13;//加减星级
    public const int stateEffectType_ChangeLevel = 14;//改变星级
    public const int stateEffectType_resistDestroy = 15;//破坏抗性
    public const int stateEffectType_resistRemove = 16;//除外抗性
    public const int stateEffectType_addAttackTime = 17;//增加攻击次数
    public const int stateEffectType_AddAttribute = 18;//光与暗之龙 增加属性
    public const int stateEffectType_Pierce = 19;//穿透效果
    public const int stateEffectType_ExchangeAfkDef = 20;//交换攻防
    public const int stateEffectType_unableChangeType = 21;//不能改变攻防形态
    public const int stateEffectType_addSacrifice = 22;//
    public const int stateEffectType_changeSacrifice = 23;//
    public const int stateEffectType_unableSacrifice = 24;//不能作为祭品
    public const int stateEffectType_unableLauchEffect = 25;
    public const int stateEffectType_unableTurnBack = 26;//不能翻转
    public const int stateEffectType_unableBeAttack = 27;//不会成为攻击目标
    #endregion

    #region 限制效果种类
    public const int limitEffectType_unableSpSummon = 1;//不能特殊召唤
    public const int limitEffectType_unableNormalSummon = 2;//不能召唤
    public const int limitEffectType_spSummonLimit = 3;//特殊召唤限制
    public const int limitEffectType_unableLauchMagic = 4;
    public const int limitEffectType_unableLauchTrap = 5;
    public const int limitEffectType_unableLauchMonsterEffect = 6;
    public const int limitEffectType_unableAttackTarget = 7;
    public const int limitEffectType_unableAttack = 8;//不能攻击

    public const int limitEffectType_sendToRemove = 9;//不去墓地除外卡片
    public const int limitEffectType_unableSearchCardFromMainDeck = 10;//不能从卡组检索 如雷王

    #endregion

    #region 玩家操作事件

    public const int playerOperateType_SpSummon = 1;

    #endregion

    #region 判断卡片的属性
    public const int fiter_containName = 0x1;
    public const int fiter_isAttribute = 0x2;
    public const int fiter_isLevel = 0x4;
    public const int fiter_isCardType = 0x8;
    public const int fiter_isArea = 0x10;
    #endregion

    #region 卡片关键字
    public const int KeyWord_Hero = 0x1;
    #endregion

    /// <summary>
    /// 判断是否为怪物
    /// </summary>
    /// <param name="cardType"></param>
    /// <returns></returns>
    public static bool isMonster(int cardType)
    {
        if (cardType > 0 || cardType < CardType_Spell_Normal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 判断是否为额外卡
    /// </summary>
    /// <returns></returns>
    public static bool isInExtra(int cardType)
    {
        if (cardType == ComVal.CardType_Monster_Fusion || cardType == ComVal.CardType_Monster_Synchro ||
               cardType == ComVal.CardType_Monster_XYZ)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 检测a是否属于b
    /// </summary>
    /// <returns></returns>
    public static bool isBind(Int64 a, Int64 b)
    {
        if ((a & b) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// a且b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int Add(int a, int b)
    {
        return a | b;
    }

    /// <summary>
    /// 取正
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int GetPositive(this int val)
    {
        if (val < 0)
        {
            return 0;
        }
        else
        {
            return val;
        }
    }

    public static int ChangePhaseToVal(int phase)
    {
        switch (phase)
        {
            case Phase_Drawphase:
                return 1;
            case Phase_Standbyphase:
                return 2;
            case Phase_Mainphase1:
                return 3;
            case Phase_Battlephase:
                return 4;
            case Phase_Mainphase2:
                return 5;
            case Phase_Endphase:
                return 6;
        }
        return -1;
    }

    /// <summary>
    /// 用户名
    /// </summary>
    public static string account;
}

public class ComStr
{
    /// <summary>
    /// 空id
    /// </summary>
    public const string CardId_Null = "0";

    #region 卡片类型
    public const string CardType_Monster_Effect = "效果怪兽";
    public const string CardType_Monster_Double = "二重怪兽";
    public const string CardType_Monster_Normal = "通常怪兽";
    public const string CardType_Monster_Synchro = "同调怪兽";
    public const string CardType_Monster_XYZ = "超量怪兽";
    public const string CardType_Monster_Fusion = "融合怪兽";
    public const string CardType_Monster_Adjust = "调整怪兽";
    public const string CardType_Spell_Normal = "通常魔法";
    public const string CardType_Spell_Quick = "速攻魔法";
    public const string CardType_Spell_Continuous = "永续魔法";
    public const string CardType_Spell_Equit = "装备魔法";

    public const string CardType_Spell_Field = "场地魔法";
    public const string CardType_Trap_Normal = "通常陷阱";
    public const string CardType_Trap_StrikeBack = "反击陷阱";
    public const string CardType_Trap_Continuous = "永续陷阱";
   
    #endregion

    #region 卡片种族
    public const string CardRace_Dragon = "龙";
    public const string CardRace_Zombie = "不死";
    public const string CardRace_Fiend = "恶魔";
    public const string CardRace_Pyro = "炎";
    public const string CardRace_SeaSerpent = "海龙";
    public const string CardRace_Rock = "岩石";
    public const string CardRace_Machine = "机械";
    public const string CardRace_Fish = "鱼";
    public const string CardRace_Dinosaur = "恐龙";
    public const string CardRace_Insect = "虫";
    public const string CardRace_Beast = "兽";
    public const string CardRace_BeastWarrior = "兽战士";
    public const string CardRace_Plant = "植物";
    public const string CardRace_Aqua = "水";
    public const string CardRace_Warrior = "战士";
    public const string CardRace_WingedBeast = "鸟兽";
    public const string CardRace_Fairy = "天使";
    public const string CardRace_Spellcaster = "魔法师";
    public const string CardRace_Thunder = "雷";
    public const string CardRace_Reptile = "爬虫";
    #endregion

    #region 卡片属性
    public const string CardAttr_Light = "光";
    public const string CardAttr_Dark = "暗";
    public const string CardAttr_Fire = "炎";
    public const string CardAttr_Water = "水";
    public const string CardAttr_Wind = "风";
    public const string CardAttr_Earth = "地";

    #endregion

    #region 卡片操作选项
    public const string Operate_NormalSummon = "普通召唤";
    public const string Operate_SpecialSummon = "特殊召唤";
    public const string Operate_Set = "设置";
    public const string Operate_CheckList = "查看列表";
    public const string Operate_Attack = "攻击";
    public const string Operate_launch = "发动";
    public const string Operate_ChangeType = "转变";
    #endregion

    #region 卡片位置
    public const string Area_monster = "怪兽区";
    public const string Area_NromalTrap = "魔陷区";
    public const string Area_SpellField = "地形区";
    public const string Area_Hand = "手牌";
    public const string Area_Graveyard = "墓地";
    public const string Area_Remove = "除外";
    public const string Area_Extra = "额外";
    #endregion

    #region 时点信息
    public const string codeMes_NormalSummon = "通常召唤";
    public const string codeMes_HighSummon = "通常召唤";
    public const string codeMes_ToGravery = "送去墓地";
    public const string codeMes_SpSummon = "特殊召唤";
    public const string codeMes_Destroy = "被破坏";
    public const string codeMes_Remove = "被除外";
    public const string codeMes_LauchEffect = "效果发动";
    public const string codeMes_TurnBack = "翻转";
    public const string codeMes_DestroyMonster = "破坏怪兽";
    public const string codeMes_DirectAttack = "直接攻击";
    public const string codeMes_EliminateMonster = "消灭怪兽";
    public const string codeMes_NoCode = "无时点";
    #endregion

    public const string EffectDescribe_40044918_1 = "选最多有这张卡以外的自己场上的名字带有「英雄」的怪兽数量的场上的魔法·陷阱卡破坏。";
    public const string EffectDescribe_40044918_2 = "从卡组把1只名字带有「英雄」的怪兽加入手卡。";

    public const string UI_DuelFieldUI = "DuelFieldUI";
    public const string UI_EditCardUI = "EditCardUI";
    public const string UI_LoginUI = "LoginUI";
    public const string UI_GameHallUI = "GameHallUI";
    public const string UI_PerpareUI = "PrepareUI";

    #region 关键字
    public const string KeyWord_Hero = "英雄";
    public const string KeyWord_Change = "变化";
    public const string KeyWord_MaskChange = "假面变化";
    public const string KeyWord_ElementalHERO = "元素英雄";
    public const string KeyWord_MaskHERO = "假面英雄";
    public const string KeyWord_Lightsworn = "光道";
    public const string KeyWord_DragUnity = "龙骑兵团";
    public const string KeyWord_SixSamurai = "六武众";
    #endregion

    #region 

    public const string Pointer_Samurai = "武士道指示物";

    #endregion

    #region 提示文字
    public const string Text_SelectPutType = "请选择表示形式";
    #endregion

    /// <summary>
    /// 获取时点的mes 用于显示消息框
    /// </summary>
    /// <returns></returns>
    public static string GetCodeMes(Int64 val)
    {
        if (val == ComVal.code_NormalSummon)
            return codeMes_NormalSummon;
        else if (val == ComVal.code_HighSummon)
            return codeMes_HighSummon;
        else if (val == ComVal.code_ToGraveyard)
            return codeMes_ToGravery;
        else if (val == ComVal.code_SpecialSummon)
            return codeMes_SpSummon;
        else if (val == ComVal.code_Destroy)
            return codeMes_Destroy;
        else if (val == ComVal.code_Remove)
            return codeMes_Remove;
        else if (val == ComVal.code_EffectLanch)
            return codeMes_LauchEffect;
        else if (val == ComVal.code_TurnBackSummon)
            return codeMes_TurnBack;
        else if (val == ComVal.code_DirectAttack)
            return codeMes_DirectAttack;
        else if (val == ComVal.code_EliminateMonster)
            return codeMes_EliminateMonster;
        else if (val == ComVal.code_NoCode)
            return codeMes_NoCode;
        return "";
    }

    public static string GetKeyWord(int val)
    {
        string result = "";
        switch (val)
        {
            case ComVal.KeyWord_Hero:
                result = KeyWord_Hero;
                break;
        }
        return result;
    }
}

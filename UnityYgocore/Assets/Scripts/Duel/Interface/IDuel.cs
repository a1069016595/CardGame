using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// duel的对外接口
/// </summary>
public interface IDuel
{
    /// <summary>
    /// 获取当前时点
    /// </summary>
    /// <returns></returns>
    Code GetCurCode();


    void SelectCardFromGroup(Group group, GroupCardSelectBack dele, int num, Player player,bool isMax=true);
    /// <summary>
    /// 选择融合卡片的素材
    /// </summary>
    /// <param name="group"></param>
    /// <param name="dele"></param>
    /// <param name="fusionCard"></param>
    void SelectFusionMaterialFromGroup(Group group, GroupCardSelectBack dele, Card fusionCard, Group targetGroup = null);
    /// <summary>
    /// 将卡片加入手牌
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    /// <param name="reasonCard"></param>
    /// <param name="reasonEffect"></param>
    void AddCardToHandFromMainDeck(Card card, Player player, Card reasonCard, BaseEffect reasonEffect);

    void AddCardToHandFromArea(int area, Group g, Player player, Card reasonCard, BaseEffect reasonEffect);

    void AddCardToHandFromArea(int p, Card c, Player player, Card card, LauchEffect effect);

    /// <summary>
    /// 注册效果 
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="card"></param>
    /// <param name="player"></param>
    void ResignEffect(BaseEffect effect, Card card, Player player);

    Chain GetCurChain();

    /// <summary>
    /// 送去墓地
    /// <para>会筛选出不在指定区域的卡片</para>
    ///  <para>当卡片不在指定区域时，不执行此操作</para>
    /// </summary>
    /// <param name="area"></param>
    /// <param name="group"></param>
    /// <param name="player"></param>
    /// <param name="reasonCard"></param>
    /// <param name="theReason"></param>
    /// <param name="effect"></param>
    Group SendToGraveyard(int area, Group group, Card reasonCard, int theReason, BaseEffect effect = null);

    /// <summary>
    /// 获取相关卡片
    /// <para>当keyword为卡片id时 会选择指定卡片</para>
    /// </summary>
    /// <param name="keyWord"></param>
    /// <param name="isBoth"></param>
    /// <param name="player"></param>
    /// <param name="cardType"></param>
    /// <param name="area"></param>
    /// <param name="isExcept"></param>
    /// <param name="exceptCard"></param>
    /// <param name="cardID"></param>
    /// <returns></returns>
    Group GetIncludeNameCardFromArea(string keyWord, bool isBoth, Player player, int cardType, int area, Filter filter = null, bool isExcept = false, Card exceptCard = null, string cardID = null);
    /// <summary>
    /// 获取相关卡片的数目
    /// <para>当keyword为卡片id时 会选择指定卡片</para>
    /// </summary>
    /// <param name="keyWord"></param>
    /// <param name="isBoth"></param>
    /// <param name="player"></param>
    /// <param name="cardType"></param>
    /// <param name="area"></param>
    /// <param name="isExcept"></param>
    /// <param name="exceptCard"></param>
    /// <param name="cardID"></param>
    /// <returns></returns>
    int GetIncludeNameCardNumFromArea(string keyWord, bool isBoth, Player player, int cardType, int area, Filter filter = null, bool isExcept = false, Card exceptCard = null, string cardID = null);

    /// <summary>
    /// 除外卡片
    /// </summary>
    /// <param name="area"></param>
    /// <param name="group"></param>
    /// <param name="reasonCard"></param>
    /// <param name="theReason"></param>
    /// <param name="effect"></param>
    void SendToRemove(int area, Group group, Card reasonCard, int theReason, BaseEffect effect);

    void SendToMainDeck(int fromArea, Group g, Card reasonCard, int theReason, BaseEffect effect);

    /// <summary>
    /// 特殊召唤
    /// <para>putType为0则由玩家选择摆放形式</para>
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    /// <param name="reasonCard"></param>
    /// <param name="reason"></param>
    /// <param name="reasonEffect"></param>
    /// <param name="putType"></param>
    void SpeicalSummon(int area, Card card, Player player, Card reasonCard, int reason, BaseEffect reasonEffect, int putType, normalDele dele);

    void ChangeMonsterType(int putType, Card card);
    /// <summary>
    /// 抽卡
    /// </summary>
    /// <param name="player"></param>
    /// <param name="num">卡片数目</param>
    /// <param name="reasonCard"></param>
    /// <param name="reasonEffect"></param>
    void DrawCard(Player player, int num, Card reasonCard = null, BaseEffect reasonEffect = null);
    /// <summary>
    /// 怪兽区是否满了
    /// </summary>
    /// <returns></returns>
    bool MonsterAreaIsFull(Player player);
    /// <summary>
    /// 魔陷区是否满了
    /// </summary>
    /// <returns></returns>
    bool SpellAreaIsFull(Player player);
    /// <summary>
    /// 减少lp
    /// </summary>
    /// <param name="val"></param>
    /// <param name="player"></param>
    void ReduceLP(int val, Player player, int reason, Card reasonCard, BaseEffect reasonEffect = null);
    /// <summary>
    /// 增加lp
    /// </summary>
    /// <param name="val"></param>
    /// <param name="player"></param>
    void AddLP(int val, Player player, int reason, Card reasonCard, BaseEffect reasonEffect = null);

    void RemoveXYZMaterial(Card c, int num, int reason, Card reasonCard, BaseEffect reasonEffect);

    void ShuffleDeck(Player player);

    void FinishHandle();

    Player GetOpsitePlayer(Player player);

    void ResignEffectLauchLimitCounter(Player p, string effectID, int maxLuachTime);


    void AddFinishHandle();

    void AddDelegate(normalDele dele, bool isAction = false);

    AttackEvent GetCurAttackEvent();

    Group DiscardFromDeck(int cardNum, Card reasonCard, BaseEffect effect, Player player);

    void ShowDialogBox(Card c, GetMes dele, bool isMy);

    bool IsPlayerRound(Player p);

    int GetCurPhase();

    void NegateSummon(Card c, BaseEffect e);

    void EquipCardFromArea(int area, Card equipCard, Player targetPlayer, Card reasonCard, BaseEffect e);

    void AddDelayAction(normalDele dele, int code, int lauchPhaseCount);

    void SpeicalSummon(int area, Group g, Player player, Card reasonCard, int reason, BaseEffect reasonEffect, int putType, GroupCardSelectBack theDele = null);
}


using UnityEngine;
using System.Collections;

public static class Duel_ChangeCardArea
{
    /// <summary>
    /// 移除不确定卡片时，需要填写area参数
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    /// <param name="area"></param>
    /// <param name="duel"></param>
    /// <returns></returns>
    public static Card RemoveCardFromArea(Card card, Player player, int area)
    {
        int curArea = card.curArea;
        if (curArea.IsBind(ComVal.Area_Field))//场地
        {
            RemoveCardFromField(card, player);
            return card;
        }
        else if (ComVal.isBind(curArea, ComVal.Area_Hand))//手牌
        {
            RemoveCardFromHand(card, player);
            return card;
        }
        else
        {
            if (area != 0)
                return RemoveCardFromDeck(area, card, player);
            else
                return RemoveCardFromDeck(curArea, card, player);
        }
    }

    /// <summary>
    /// 添加卡片到区域
    /// <para>当需要洗牌时，需要传入iduel参数</para>
    /// </summary>
    /// <param name="area"></param>
    /// <param name="card"></param>
    /// <param name="player"></param>
    /// <param name="putType"></param>
    /// <param name="playAnim">是否进行动画 默认为真</param>
    public static void AddCardToArea(int area, Card card, Player player, Reason reason, int putType = 0, bool playAnim = true)
    {
        if (area.IsBind(ComVal.Area_Field))
        {
            AddCardToField(card, putType, player, (FieldUIType)area, reason);
        }
        else if (area == ComVal.Area_Hand)
        {
            AddCardToHand(card, player, reason, playAnim);
        }
        else
        {
            AddCardToDeck(area, card, player, reason);
        }
    }

    /// <summary>
    /// 将卡片移除出手牌
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    static void RemoveCardFromHand(Card card, Player player)
    {
        player.group_HandCard.RemoveCard(card.areaRank);
        DuelUIManager.GetInstance().RemoveCardFromHand(card.areaRank, player.isMy);
    }

    /// <summary>
    /// 对墓地 除外 主卡组 额外卡组移除卡片
    /// <para>如果card为空，则返回主卡组的第一张卡 用于抽卡</para>
    /// </summary>
    /// <param name="card"></param>
    static Card RemoveCardFromDeck(int area, Card card, Player player)
    {

        Card result = Card.Empty();
        switch (area)
        {
            case ComVal.Area_MainDeck:
                if (!Card.IsEmpty(card))
                {
                    player.group_MainDeck.RemoveCard(card);
                }
                else
                    result = player.group_MainDeck.RemoveFirstCard();
                break;
            case ComVal.Area_Extra:
                player.group_ExtraDeck.RemoveCard(card);
                break;
            case ComVal.Area_Graveyard:
                player.group_Graveyard.RemoveCard(card.areaRank);
                break;
            case ComVal.Area_Remove:
                player.group_Remove.RemoveCard(card.areaRank);
                break;
            default:
                break;
        }
        if (ComVal.isBind(area, ComVal.Area_MainDeck) || ComVal.isBind(area, ComVal.Area_Extra))
        {
            DuelUIManager.GetInstance().RemoveCardFromDeck(area, Card.Empty(), 1, player.isMy);
        }
        else
        {
            DuelUIManager.GetInstance().RemoveCardFromDeck(area, card, 1, player.isMy);
        }
        if (area == ComVal.Area_MainDeck)
        {
            Duel.GetInstance().ShuffleDeck(player);
        }

        return result;
    }
    /// <summary>
    /// 将卡片移除出场地区域
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    static void RemoveCardFromField(Card card, Player player)
    {
        int area = card.curArea;
        DuelUIManager.GetInstance().RemoveCardFromField(card, player.isMy);
        if (area == ComVal.Area_Monster)
        {
            player.group_MonsterCard.RemoveCard(card.areaRank);
        }
        else if (area == ComVal.Area_NormalTrap)
        {
            player.group_TrapCard.RemoveCard(card.areaRank);
        }
        else if(area==ComVal.Area_FieldSpell)
        {
            player.fieldSpell = null;
        }
    }



    /// <summary>
    /// 将卡片加入到场地区域
    /// <para>会设置放置类型和区域序号和区域类型</para>
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    /// <param name="isInMonster">是否在怪兽区域</param>
    static void AddCardToField(Card card, int putType, Player player, FieldUIType type, Reason reason)
    {
        int rank = DuelUIManager.GetInstance().AddToArea(card.cardID, putType, type, player.isMy);

        switch (type)
        {
            case FieldUIType.monster:
                card.SetArea(ComVal.Area_Monster, reason);
                player.group_MonsterCard.AddCard(rank, card);
                break;
            case FieldUIType.trap:
                card.SetArea(ComVal.Area_NormalTrap, reason);
                player.group_TrapCard.AddCard(rank, card);
                break;
            case FieldUIType.areaSpell:
                card.SetArea(ComVal.Area_FieldSpell, reason);
                player.fieldSpell=card;
                break;
            default:
                break;
        }
        card.SetAreaRank(rank);
        card.SetPlaseState(putType);
    }

    /// <summary>
    /// 将卡片加入到手牌 
    /// </summary>
    public static void AddCardToHand(Card card, Player player, Reason reason, bool playAnim = false)
    {
        normalDele dele = delegate
        {
            player.group_HandCard.AddCard(card);
            DuelUIManager.GetInstance().AddCardToHand(card, player.isMy);
            int i = player.group_HandCard.GroupNum;
            card.SetArea(ComVal.Area_Hand, reason);
            card.SetAreaRank(i - 1);
            if (playAnim == true)
            {
                Duel.GetInstance().FinishHandle();
            }
        };
        if (card.curArea == ComVal.Area_MainDeck && playAnim)
        {
            DuelUIManager.GetInstance().ShowDrawAnim(card.cardID, dele, player.group_HandCard.GroupNum, player.isMy);
        }
        else
        {
            dele();
        }
    }


    /// <summary>
    /// 对墓地 除外 主卡组 额外卡组添加卡片
    /// <para>可以指定区域序号</para>
    /// </summary>
    /// <param name="area"></param>
    /// <param name="card"></param>
    /// <param name="player"></param>
    static void AddCardToDeck(int area, Card card, Player player, Reason reason)
    {
        switch (area)
        {
            case ComVal.Area_MainDeck:
                player.group_MainDeck.AddCard(card);
                card.SetArea(ComVal.Area_MainDeck, reason);
                break;
            case ComVal.Area_Extra:
                player.group_ExtraDeck.AddAndSetCard(card);
                card.SetArea(ComVal.Area_Extra, reason);
                break;
            case ComVal.Area_Graveyard:
                player.group_Graveyard.AddAndSetCard(card);
                card.SetArea(ComVal.Area_Graveyard, reason);
                break;
            case ComVal.Area_Remove:
                player.group_Remove.AddAndSetCard(card);
                card.SetArea(ComVal.Area_Remove, reason);
                break;
            default:
                break;
        }

        DuelUIManager.GetInstance().AddCardToDeck(area, card, 1, player.isMy);
        if (area == ComVal.Area_MainDeck)
        {
            Duel.GetInstance().ShuffleDeck(player);
        }
    }
}

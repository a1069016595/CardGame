using System;
using System.Collections.Generic;
using UnityEngine;

public class Chain
{
    public bool isHandle;
    public Int64 codeVal;
    public int chainNum;
    public bool isDelayCode;

    int maxChainNum;
    List<Card> cardList;
    List<LauchEffect> effectList;

    List<int> disableList; 

    public Chain(bool isDelay=false)
    {
        isHandle = false;
        effectList = new List<LauchEffect>();
        cardList = new List<Card>();
        disableList = new List<int>();
        chainNum = 0;
        maxChainNum = 0;
        isDelayCode = isDelay;
    }

    public void AddToChain(LauchEffect effect)
    {

        effectList.Add(effect);
        chainNum++;
        maxChainNum++;
        if(!cardList.Contains(effect.ownerCard))
        {
            cardList.Add(effect.ownerCard);
        }
    }

    /// <summary>
    /// 移除最后
    /// </summary>
    public void RemoveEffect()
    {
        effectList.RemoveAt(effectList.Count - 1);
        chainNum--;
    }
    public LauchEffect GetEffect()
    {
        return effectList[effectList.Count - 1];
    }

    public LauchEffect GetEffect(int val)
    {
        return effectList[val];
    }


    public void SetHandle(bool val)
    {
        isHandle = val;
    }

    public Card RemoveCard()
    {
        Card c = cardList[cardList.Count - 1];
        cardList.RemoveAt(cardList.Count - 1);
        return c;
    }


    public bool ContainEffect(Int64 code,Card card, LauchEffect e)
    {
        return effectList.Contains(e) || ContainCodeEffect(code,card);
    }

    public bool ContainCodeEffect(Int64 code, Card card)
    {
        foreach (var item in effectList)
        {
            if (item.ownerCard == card && item.code.IsBind(code))
            {
                return true;
            }
        }
        return false;
    }

    public int GetCardNum()
    {
        return cardList.Count;
    }

    public int GetMaxChainNum()
    {
        return maxChainNum;
    }

    public LauchEffect GetLastEffect()
    {
        if(effectList.Count==0)
        {
            return null;
        }
        return effectList[effectList.Count - 1];
    }

    public void SetCodeVal(Int64 val)
    {
        codeVal = val;
    }

    public void DisableLastEffect()
    {
        disableList.Add(effectList.Count - 1);
    }

    public bool IsEffectDisable(LauchEffect e)
    {
        if(disableList.Contains( effectList.IndexOf(e)))
        {
            return true;
        }
        return false;
    }
}
    



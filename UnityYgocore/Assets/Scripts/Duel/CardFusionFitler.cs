using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class CardMaterialFitler
{
    public Filter cardFilter1;
    public Filter cardFilter2;

    List<Filter> list;

    public CardMaterialFitler(Filter f1, Filter f2)
    {
        list = new List<Filter>();
        cardFilter1 = f1;
        cardFilter2 = f2;
        list.Add(f1);
        list.Add(f2);
    }

    public Group GetGroupList(Group group, Card card)
    {
        if (cardFilter1(card) && cardFilter2(card))
        {
            return group;
        }
        else if (cardFilter1(card))
        {
            Group a = new Group();
            for (int i = 0; i < group.GroupNum; i++)
            {
                Card c = group.GetCard(i);
                if (cardFilter2(c))
                {
                    a.AddCard(c);
                }
            }
            return a;
        }
        else if (cardFilter2(card))
        {
            Group a = new Group();
            for (int i = 0; i < group.GroupNum; i++)
            {
                Card c = group.GetCard(i);
                if (cardFilter1(c))
                {
                    a.AddCard(c);
                }
            }
            return a;
        }
        else
        {
            Debug.Log("奇怪的错误");
            return Group.Empty();
        }
    }

    public Group GetGroup(Group group)
    {
        Group result = new Group();
        for (int i = 0; i < group.GroupNum; i++)
        {
            Card card = group.GetCard(i);
            if(cardFilter1(card)||cardFilter2(card))
            {
                result.AddCard(card);
            }
        }
        return result;
    }
}

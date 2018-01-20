using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


/// <summary>
/// 加载xml中的卡片信息
/// <para>所有方法都要加载了xml后才可调用</para>
/// </summary>
public class LoadXml : MonoBehaviour
{
    /// <summary>
    /// 卡片字典
    /// </summary>
    public static Dictionary<string, Card> CardDic;

    /// <summary>
    /// xml中的卡片字典
    /// </summary>
    public static Dictionary<string, XmlCard> XmlCardDic;

    private static bool isLoad = false;

    /// <summary>
    /// 加载xml 并将其保存在卡片字典中
    /// </summary>
    /// 
    public static void LoadTheXml()
    {
        if (isLoad)
        {
            return;
        }

        CardDic = new Dictionary<string, Card>();
        XmlCardDic = new Dictionary<string, XmlCard>();
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load("Card");
        xmlDoc.LoadXml(textAsset.text);
        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("Cards").ChildNodes;

        foreach (XmlNode node in xmlNodeList)
        {
            string cardID = node.Attributes["cardID"].Value;
            string cardName = node.Attributes["cardName"].Value;
            string cardDescribe = node.Attributes["cardDescribe"].Value;
            string str_cardType = node.Attributes["cardType"].Value;

            int int_cardType = StaticMethod.ChangeCardType(str_cardType);
            Card card = new Card(cardID, cardName, cardDescribe, int_cardType);
            XmlCard xmlCard = new XmlCard(cardID, cardName, cardDescribe, str_cardType);

            if (card.IsMonster())
            {
               
                int afk = int.Parse(node.Attributes["afk"].Value);
                int def = int.Parse(node.Attributes["def"].Value);
                int level = int.Parse(node.Attributes["level"].Value);
                string str_attribute = node.Attributes["attribute"].Value;
                int int_attribute = StaticMethod.ChangeCardAttr(str_attribute,cardID);
                string str_race = node.Attributes["race"].Value;
                int int_race = StaticMethod.ChangeCardRace(str_race);
                card.SetMonsterAttribute(int_attribute, int_race, level, afk, def);
                xmlCard.SetMonsterAttribute(str_attribute, str_race, level, afk, def);
            }
            CardDic.Add(cardID, card);
            XmlCardDic.Add(cardID, xmlCard);
            // Debug.Log(card.cardName + " " + card.cardType + " " + card.cardDescribe + " " + card.afk + " " + card.level + " " + card.race);
            //Debug.Log(cardID + cardName);
            isLoad = true;
        }
    }

    public static Card GetCard(string cardID)
    {

        if (CardDic.ContainsKey(cardID) == false)
        {
            Debug.Log(cardID);
            Debug.Log("error");
        }
        return CardDic[cardID];
    }

    public static XmlCard GetXmlCard(string cardID)
    {

        if (XmlCardDic.ContainsKey(cardID) == false)
        {
            Debug.Log(cardID);
            Debug.Log("error");
        }
        return XmlCardDic[cardID];
    }

    /// <summary>
    /// 返回搜索到的list
    /// </summary>
    /// <param name="cardType">卡片种类</param>
    /// <param name="cardAttr">怪兽属性</param>
    /// <param name="cardRace">怪兽种族</param>
    /// <param name="cardLevel">怪兽星阶</param>
    /// <param name="keyWord">关键字</param>
    /// <returns></returns>
    public static List<string> SearchCard(int cardType, int cardAttr, int cardRace, int cardLevel, string keyWord)
    {
        List<string> result = new List<string>();
        bool checkCardAttr=false;
        bool checkCardRace=false;
        bool checkCardLevel=false;
        bool checkCardType = false;

        if(cardType!=0)
        {
            checkCardType = true;
        }
        if(cardAttr!=0)
        {
            checkCardAttr = true;
        }
        if (cardRace != 0)
        {
            checkCardRace = true;
        }
        if (cardLevel != 0)
        {
            checkCardLevel = true;
        }
        foreach (var item in CardDic)
        {
            Card card = item.Value;


            if (card.cardName.Contains(keyWord) || card.cardDescribe.Contains(keyWord))
            {
                if (checkCardType)
                {
                    if (card.cardType != cardType)
                    {
                        continue;
                    }
                }
                if (checkCardAttr)
                {
                    if (card.attribute != cardAttr)
                    {
                        continue;
                    }
                }
                if (checkCardRace)
                {
                    if (card.race != cardRace)
                    {
                        continue;
                    }
                }
                if (checkCardLevel)
                {
                    if (card.level != cardLevel)
                    {
                        continue;
                    }
                }

                result.Add(card.cardID);
            }

        }

        return result;
    }

   
}


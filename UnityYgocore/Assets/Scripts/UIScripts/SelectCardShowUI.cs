
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectCardShowUI : BaseMonoBehivour
{

    public RawImage rawImage;
    public Text effectText;
    public Text nameText;

    void Start()
    {
        AddHandler(DuelEvent.uiEvent_UpdateSelectCardShow, UpdateCardDisplay);
    }

    public void UpdateCardDisplay(params object[] args)
    {
        string cardID = (string)args[0];
        if(cardID=="0"||cardID=="")
        {
            return;
        }
        XmlCard card = LoadXml.GetXmlCard(cardID);
        string star = "";
        rawImage.texture = StaticMethod.GetCardPics(cardID, false);
        effectText.text = card.cardDescribe;
        nameText.text = "[" + card.cardName.ToString() + "]" + "\n";

        nameText.text += "[" + card.cardType + "]";
        if (card.IsMonster())
        {
            for (int i = 0; i < card.level; i++)
            {
                star = star + "★";
            }
            nameText.text += "  " + card.race + "/" + card.attribute + "\n";
            nameText.text += "[" + star + "]" + "   " + card.afk + "/" + card.def;
        }
    }
}

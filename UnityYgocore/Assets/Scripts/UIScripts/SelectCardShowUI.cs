
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectCardShowUI : MonoBehaviour
{

    public RawImage rawImage;
    public Text effectText;
    public Text nameText;

    void Start()
    {
        DuelEventSys.GetInstance.onOver_updateSelectCardShow += UpdateCardDisplay;
    }

    public void UpdateCardDisplay(string cardID)
    {
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

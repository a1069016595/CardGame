using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Card_SearchResult : BaseCard, IDragHandler, IEndDragHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler
{

    EditShowCardUI editShowCardUI;

    private Text theText;
    private RawImage bgImage;
    private EditUI editUI;
    DragCardUI dragCardUI;

    public void Init()
    {
        editShowCardUI = EditShowCardUI.GetInstance();
        theText = transform.FindChild("Text").GetComponent<Text>();
        image = this.GetComponent<RawImage>();
        bgImage = transform.FindChild("BG").GetComponent<RawImage>();
        editUI = EditUI.GetInstance();
        dragCardUI = DragCardUI.GetInstance();
    }

    public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        editShowCardUI.UpdateCardDisplay(this.id);
        bgImage.color = new Color(0, 0, 0, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bgImage.color = Color.clear;
    }


    /// <summary>
    /// 卡片描述 \n ★
    /// </summary>
    public void SetText(string id)
    {
        XmlCard card = LoadXml.GetXmlCard(id);

        theText.text = "";
        theText.text += card.cardName + "\n";
        if (card.IsMonster())
        {
            theText.text += card.afk.ToString() + "/" + card.def.ToString() + "\n";
            theText.text += card.attribute + "/" + card.race + "  " + "★" + card.level.ToString();
        }
        else
        {
            theText.text += card.cardType;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        dragCardUI.SetPosition(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        editUI.ShowDragCardUI(this.id);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bgImage.color = Color.clear;
        dragCardUI.EndDrag();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       if(eventData.button==PointerEventData.InputButton.Right)
       {
           editUI.AddCardToDeck(this.id);
       }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// edit中用于显示卡组中的卡片
/// </summary>
public class Card_Edit : BaseCard, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    EditShowCardUI editShowCardUI;
    EditUI editUI;
    DragCardUI dragCardUI;
    EditDeckUI editDeckUI;
    public void Init()
    {
        editShowCardUI = EditShowCardUI.GetInstance();
        image = this.GetComponent<RawImage>();
        editUI = EditUI.GetInstance();
        dragCardUI = DragCardUI.GetInstance();
        editDeckUI = EditDeckUI.GetInstance();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            editUI.RemoveCardFromDeck(gameObject);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        editShowCardUI.UpdateCardDisplay(this.id);
        image.color = new Color(0.7f, 0.7f, 0.7f, 1);
        editDeckUI.SetCurCard(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
        editDeckUI.SetCurCard(null);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        editUI.ShowDragCardUI(this.id);
        editUI.RemoveCardFromDeck(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragCardUI.SetPosition(eventData);
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragCardUI.EndDrag();
    }
}
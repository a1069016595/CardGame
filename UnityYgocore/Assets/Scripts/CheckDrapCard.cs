using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 检测是否有卡牌拖拽进区域
/// </summary>
public class CheckDrapCard : MonoBehaviour,IDropHandler
{
    private DragCardUI dragCardUI;

    EditUI editUI;

    void Awake()
    {
        dragCardUI = DragCardUI.GetInstance();
        editUI = EditUI.GetInstance();
    }

    public void OnDrop(PointerEventData eventData)
    {
        editUI.AddCardToDeck(dragCardUI.id);
    }
}
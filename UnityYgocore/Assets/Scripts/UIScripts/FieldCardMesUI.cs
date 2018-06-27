using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldCardMesUI : BaseMonoBehivour
{

    Duel duel;
    RectTransform rectTransform;
    Text text;

   

    void Start()
    {
        AddHandler(DuelEvent.uiEvent_ShowFieldCardMes, ShowMesHandler);
        AddHandler(DuelEvent.uiEvent_HideFieldCardMes, HideMesHandler);
        duel = Duel.GetInstance();
        rectTransform = GetComponent<RectTransform>();
        text = transform.GetChild<Text>("Text");
        gameObject.SetActive(false);
    }

    private void HideMesHandler(params object[] args)
    {
        gameObject.SetActive(false);

    }

    void Update()
    {
        Vector2 pos=GetMousePos();
        rectTransform.anchoredPosition = new Vector2(pos.x + rectTransform.sizeDelta.x / 2, pos.y + rectTransform.sizeDelta.y / 2);
    }

    private void ShowMesHandler(params object[] args)
    {
        gameObject.SetActive(true);
        bool isMy = (bool)args[0];
        int area = (int)args[1];
        int rank = (int)args[2];
        Vector2 pos = (Vector2)args[3];

        Card card = duel.GetCard(isMy, area, rank);

        
        //rectTransform.anchoredPosition3D = new Vector3(pos.x, pos.y + 30,0);

        text.text = card.cardName + "\n";
        List<Pointer> list = card.GetPointerList();
        for (int i = 0; i < list.Count; i++)
        {
            Pointer p = list[i];
            if (p.num > 0)
            {
                text.text += p.name + ":" + p.num + "\n";
            }
        }
    }

    private Vector3 GetMousePos()
    {
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.transform.parent as RectTransform,
            Input.mousePosition,
            Camera.main,
            out result);
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurrenderUI : BaseMonoBehivour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickButton);

        AddHandler(DuelEvent.playBackEvent_StartGame, HideButton);
    }

    private void HideButton(params object[] args)
    {
        gameObject.SetActive(false);
    }

    private void ClickButton()
    {
        SendEvent(DuelEvent.duelEvent_Surrender);
    }
}

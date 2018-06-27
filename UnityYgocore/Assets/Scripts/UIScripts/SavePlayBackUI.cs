using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePlayBackUI : BasePanelUI
{

    InputField inputField;

    void Awake()
    {
        gameObject.SetActive(false);
        InitPanle();

        inputField=transform.GetChild<InputField>("InputField");

        AddHandler(DuelEvent.duelEvent_ShowSavePlayBackPanel, Show);
    }

    private void Show(params object[] args)
    {
        gameObject.SetActive(true);
    }


    public override void ClickCanelButton()
    {
        gameObject.SetActive(false);
        TurnToGameHall();
    }

    public override void ClickApplyButton()
    {
        if(inputField.text=="")
        {
            return;
        }
        gameObject.SetActive(false);
        eventSys.SendEvent(DuelEvent.duelEvent_SavePlayBack, inputField.text);
        ErrorPlane.GetInstance().Show("保存成功",TurnToGameHall);
    }

    private void TurnToGameHall()
    {
        uiMgr.LoadUI(ComStr.UI_GameHallUI);
    }
}

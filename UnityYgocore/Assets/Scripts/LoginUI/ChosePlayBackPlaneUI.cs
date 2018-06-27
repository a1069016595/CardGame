using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChosePlayBackPlaneUI : BasePanelUI
{
    Text describeText;
    Dropdown playBackDropdown;


    void Awake()
    {
        InitPanle();

        describeText = transform.GetChild<Text>("DescribeText");
        playBackDropdown = transform.GetChild<Dropdown>("PlayBackDropdown");

        AddHandler(DuelEvent.gameEvent_OpenChoosePlayBackPlane, ShowPlane);

        playBackDropdown.onValueChanged.AddListener(ChangeSelectVal);
        gameObject.SetActive(false);
    }


    private void ShowPlane(params object[] args)
    {
        gameObject.SetActive(true);
        List<string> list = PlayBackLoad.GetPlayBackList();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (var item in list)
        {
            options.Add(new Dropdown.OptionData(item));
        }
        playBackDropdown.options = options;
        ChangeDescribeText();
    }


    private void ChangeSelectVal(int arg0)
    {
        ChangeDescribeText();
    }

    private void ChangeDescribeText()
    {
        PlayBackMes mes = PlayBackLoad.GetPlayBackMes(playBackDropdown.captionText.text);
        describeText.text = mes.date + "\n" + mes.p1 + "\n" + "===vs===" +"\n"+ mes.p2;
    }

    public override void ClickCanelButton()
    {
        ClosePanel();
    }

    public override void ClickApplyButton()
    {
        ClosePanel();
        uiMgr.LoadDuelField();
        eventSys.SendEvent(DuelEvent.gameEvent_EnterPlayBack, playBackDropdown.captionText.text);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
        eventSys.SendEvent(DuelEvent.gameEvent_CloseChoosePlayBackPlane);
    }
}

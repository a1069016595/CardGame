using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class BasePanelUI : BaseMonoBehivour
{
    protected Button applyButton;
    protected Button cancelButton;

    protected UIMgr uiMgr;
    protected DuelEventSys eventSys;

    protected void InitPanle()
    {
        eventSys = DuelEventSys.GetInstance;
        uiMgr = UIMgr.Instance();
        applyButton = transform.GetChild<Button>("ApplyButton");
        cancelButton = transform.GetChild<Button>("CanelButton");

        applyButton.onClick.AddListener(ClickApplyButton);
        cancelButton.onClick.AddListener(ClickCanelButton);
    }

    abstract public void ClickCanelButton();
    abstract public void ClickApplyButton();
}

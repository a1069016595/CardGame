using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public delegate  void  PhaseButtonClick(PhaseButton button) ;

public class PhaseButtonMgr : MonoBehaviour
{
    #region 单例
    private static PhaseButtonMgr instance;

    public PhaseButtonMgr()
    {
        instance = this;
    }

    public static PhaseButtonMgr GetInstance()
    {
        return instance;
    }
    #endregion


    public List<PhaseButton> buttonList;

    public PhaseButton currentPhase;

    public bool canControl;

    public PhaseButton Drawphase;
    public PhaseButton Standbyphase;
    public PhaseButton Mainphase1;
    public PhaseButton Battlephase;
    public PhaseButton Mainphase2;
    public PhaseButton Endphase;

    Duel duel;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        buttonList = new List<PhaseButton>();

        Drawphase = transform.FindChild("Drawphase").GetComponent<PhaseButton>();
        Standbyphase = transform.FindChild("Standbyphase").GetComponent<PhaseButton>();
        Mainphase1 = transform.FindChild("Mainphase1").GetComponent<PhaseButton>();
        Battlephase = transform.FindChild("Battlephase").GetComponent<PhaseButton>();
        Mainphase2 = transform.FindChild("Mainphase2").GetComponent<PhaseButton>();
        Endphase = transform.FindChild("Endphase").GetComponent<PhaseButton>();

        buttonList.Add(Drawphase);
        buttonList.Add(Standbyphase);
        buttonList.Add(Mainphase1);
        buttonList.Add(Battlephase);
        buttonList.Add(Mainphase2);
        buttonList.Add(Endphase);
        foreach (var item in buttonList)
        {
            item.Init();
            item.click += PhaseButtonMgr_click;
        }
        Drawphase.SetCanNotControl();
        Standbyphase.SetCanNotControl();
        Mainphase1.SetCanNotControl();

        duel = Duel.GetInstance();
    }

    void PhaseButtonMgr_click(PhaseButton button)
    {
        if (currentPhase == Drawphase || currentPhase == Standbyphase)
        {
            Debug.Log("error");
            return;
        }
        if (duel.IsNetWork && duel.isMyRound)
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendChangePhase, GetPhase(button));
        }
        DuelEventSys.GetInstance.SendEvent(DuelEvent.event_changePhase, GetPhase(button));
    }

    public void SetCanControl()
    {
        if (currentPhase == Mainphase1)
        {
            Battlephase.SetCanBeControl();
            Mainphase2.SetCanBeControl();
            Endphase.SetCanBeControl();
        }
        else if (currentPhase == Battlephase)
        {
            Battlephase.SetCanNotControl();
            Mainphase2.SetCanBeControl();
            Endphase.SetCanBeControl();
        }
        else if (currentPhase == Mainphase2)
        {
            Battlephase.SetCanNotControl();
            Mainphase2.SetCanNotControl();
            Endphase.SetCanBeControl();
        }
        else
        {
            //Debug.Log(currentPhase);
            SetNotControl();
        }
    }
    int GetPhase(PhaseButton button)
    {
        if (button == Battlephase)
        {
            return ComVal.Phase_Battlephase;
        }
        else if (button == Mainphase2)
        {
            return ComVal.Phase_Mainphase2;
        }
        else if (button == Endphase)
        {
            return ComVal.Phase_Endphase;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 由外部调用
    /// </summary>
    /// <param name="i"></param>
    public void ChangeToPhase(int i)
    {
        currentPhase = buttonList[ComVal.ChangePhaseToVal(i)-1];
        foreach (var item in buttonList)
        {
            item.ChangeToNormalColor();
        }
        currentPhase.ChangeTextColor();
        SetCanControl();
    }

    public void SetNotControl()
    {
        Battlephase.SetCanNotControl();
        Mainphase2.SetCanNotControl();
        Endphase.SetCanNotControl();
    }
}

    


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionListUI : DuelUIOpearate, IDeselectHandler, IPointerExitHandler
{

    #region 单例
    private static OptionListUI instance;

    public OptionListUI()
    {
        instance = this;

    }

    public static OptionListUI GetInstance()
    {
        return instance;
    }
    #endregion


    public List<OptionButton> buttonList;


    public int targetArea;
    public int targetRank;


    public void Init()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].Init();
            buttonList[i].SetActive(false);
        }

    }

    public void OperateCard(OptionButton button)
    {
        if (CanNotControl())
        {
            return;
        }
        string str = button.GetText();
        if (IsSendMes())
        {
            if (str != ComStr.Operate_CheckList)
            {
                eventSys.SendEvent(DuelEvent.netEvent_SendOperateCard, targetArea, targetRank, str, isMySelect);
            }
        }

        eventSys.SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_OperateCard, targetArea, targetRank, str, isMySelect);
        if(str!=ComStr.Operate_CheckList)
        {
            eventSys.SendEvent(DuelEvent.duelEvent_operateCard, targetArea, targetRank, str, isMySelect);
        }
        HideOptionList();
    }

    /// <summary>
    /// 显示选项列表
    /// </summary>
    /// <param name="area">所在区域</param>
    /// <param name="rank">顺序</param>
    public void ShowOptionList(int area, int rank, Vector3 pos, bool isMy)
    {
        if(Duel.GetInstance().isFinishGame)
        {
            return;
        }
        gameObject.SetActive(true);
        targetArea = area;
        targetRank = rank;
        List<string> list = new List<string>();
        list = Duel.GetInstance().GetCardOption(targetArea, targetRank, isMy);
        if (list.Count == 0)
        {
            return;
        }
        if (list.Count > 5)
        {
            Debug.Log("too long");
            return;
        }
        GetComponent<RawImage>().raycastTarget = true;
        isMySelect = isMy;
        this.GetComponent<RectTransform>().position = pos;
     
      

        for (int i = 0; i < list.Count; i++)
        {
            buttonList[i].SetActive(true);
            buttonList[i].SetText(list[i]);
        }
        for (int i = list.Count; i < buttonList.Count; i++)
        {
              buttonList[i].SetActive(false);
        }
    }
    public void HideOptionList()
    {
        targetRank = 0;
        targetArea = 0;
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("取消选择");
        HideOptionList();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RawImage>().raycastTarget = false;
        HideOptionList();
    }
}

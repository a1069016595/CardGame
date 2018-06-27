using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PhaseButton : BaseMonoBehivour, IPointerClickHandler
{
    public Text text;
    public Button button;

    public event PhaseButtonClick click;

    public bool canClick = false;

    public void Init()
    {
        text = transform.FindChild("Text").GetComponent<Text>();
        button = this.GetComponent<Button>();
    }

    /// <summary>
    /// 为当前阶段时 改变字体颜色
    /// </summary>
    public void ChangeTextColor()
    {
        text.color = Color.blue;
    }

    /// <summary>
    /// 不为当前阶段 恢复为初始颜色
    /// </summary>
    public void ChangeToNormalColor()
    {
        text.color = Color.black;
    }

    public void SetCanNotControl()
    {
        button.enabled = false;
        canClick = false;
    }

    public void SetCanBeControl()
    {
        button.enabled = true;
        canClick = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
            click(this);
        else
            return;
    }
}

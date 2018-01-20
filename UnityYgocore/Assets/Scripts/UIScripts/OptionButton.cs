using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour,IPointerClickHandler
{
    private OptionListUI optionListUI;

    public Text text;

    public void Init()
    {
        optionListUI=OptionListUI.GetInstance();
        text = this.transform.FindChild("Text").GetComponent<Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        optionListUI.OperateCard(this);
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void SetText(string str)
    {
        text.text = str;
    }

    public string GetText()
    {
        return text.text;
    }
}

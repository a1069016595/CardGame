using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void SelectCard(int i,Card_Select obj);
public delegate void DisSelectCard(int i, Card_Select obj);
/// <summary>
/// duel中卡片组选择的prefeb
/// </summary>
public class Card_Select : BaseCard
{
    public Text positionText;
    public RawImage bgImage;

    public bool isChoose;
    public Color initialColor;

    public event SelectCard event_selectCard;
    public event DisSelectCard event_disSelectCard;

    /// <summary>
    /// 序号
    /// </summary>
    public int num;

    public void Init(int _num)
    {
        image = this.transform.FindChild("cardImage").GetComponent<RawImage>();
        positionText = this.transform.FindChild("positionText").GetComponent<Text>();
        bgImage = this.GetComponent<RawImage>();
        isChoose = false;
        initialColor = bgImage.color;
        num = _num;
    }

    /// <summary>
    /// 设置位置信息
    /// </summary>
    public void SetText(int pos, int val)
    {
        string str = "";
        if (pos == ComVal.Area_Hand)
        {
            str = str + ComStr.Area_Hand;
        }
        else if (pos == ComVal.Area_Graveyard)
        {
            str = str + ComStr.Area_Graveyard;
        }
        else if (pos == ComVal.Area_Monster)
        {
            str = str + ComStr.Area_monster;
        }
        else if (pos == ComVal.Area_Remove)
        {
            str = str + ComStr.Area_Remove;
        }
        else if (pos == ComVal.Area_NormalTrap)
        {
            str = str + ComStr.Area_NromalTrap;
        }
        else if (pos == ComVal.Area_FieldSpell)
        {
            str = str + ComStr.Area_SpellField;
        }
        else if (pos == ComVal.Area_Extra)
        {
            str = str + ComStr.Area_Extra;
        }
        str = str + "(" + val.ToString() + ")";
        positionText.text = str;
    }


    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!isChoose)
        {
            event_selectCard(num, this);
        }
        else
        {
            event_disSelectCard(num, this);
        }
    }

    public void SetSelect()
    {
        bgImage.color = Color.yellow;
        isChoose = true;
    }

    public void SetDisSelect()
    {
        bgImage.color = initialColor;
        isChoose = false;
    }
}

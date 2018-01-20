using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SearchCardUI : MonoBehaviour
{
    #region 单例
    private static SearchCardUI instance;

    public SearchCardUI()
    {
        instance = this;
    }

    public static SearchCardUI GetInstance()
    {
        return instance;
    }
    #endregion

    public Dropdown cardTypeDropdown;
    public Dropdown cardLevelDropdown;
    public Dropdown cardAttrDropdown;
    public Dropdown cardRaceDropdown;
    public Button findButton;
    public InputField keyWordInputField;

    public EditUI editUI;

    public void Init()
    {
        cardTypeDropdown = transform.FindChild("CardTypeDropdown").GetComponent<Dropdown>();
        cardLevelDropdown = transform.FindChild("CardLevelDropdown").GetComponent<Dropdown>();
        cardAttrDropdown = transform.FindChild("CardAttrDropdown").GetComponent<Dropdown>();
        cardRaceDropdown = transform.FindChild("CardRaceDropdown").GetComponent<Dropdown>();
        findButton = transform.FindChild("FindButton").GetComponent<Button>();
        keyWordInputField = transform.FindChild("KeyWordInputField").GetComponent<InputField>();

        findButton.onClick.AddListener(OnFindButtonClick);

        InitDropDownOptionList();

        editUI = EditUI.GetInstance();


    }

    private void InitDropDownOptionList()
    {
        List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();

        list.Add(new Dropdown.OptionData("N/A"));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Monster_Normal));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Monster_Effect));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Monster_Adjust));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Monster_Fusion));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Monster_Synchro));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Monster_XYZ));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Spell_Normal));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Spell_Quick));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Spell_Continuous));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Spell_Equit));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Trap_Normal));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Trap_StrikeBack));
        list.Add(new Dropdown.OptionData(ComStr.CardType_Trap_Continuous));

        cardTypeDropdown.options = list;

        list = new List<Dropdown.OptionData>();
        list.Add(new Dropdown.OptionData("N/A"));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Dragon));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Zombie));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Fiend));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Pyro));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_SeaSerpent));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Rock));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Machine));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Fish));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Dinosaur));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Insect));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Beast));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_BeastWarrior));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Plant));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Aqua));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Warrior));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_WingedBeast));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Fairy));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Spellcaster));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Thunder));
        list.Add(new Dropdown.OptionData(ComStr.CardRace_Reptile));

        cardRaceDropdown.options = list;

        list = new List<Dropdown.OptionData>();
        list.Add(new Dropdown.OptionData("N/A"));
        for (int i = 1; i < 13; i++)
        {
            list.Add(new Dropdown.OptionData(i.ToString() + "星"));

        }
        cardLevelDropdown.options = list;

        list = new List<Dropdown.OptionData>();

        list.Add(new Dropdown.OptionData("N/A"));
        list.Add(new Dropdown.OptionData(ComStr.CardAttr_Light));
        list.Add(new Dropdown.OptionData(ComStr.CardAttr_Dark));
        list.Add(new Dropdown.OptionData(ComStr.CardAttr_Fire));
        list.Add(new Dropdown.OptionData(ComStr.CardAttr_Water));
        list.Add(new Dropdown.OptionData(ComStr.CardAttr_Wind));
        list.Add(new Dropdown.OptionData(ComStr.CardAttr_Earth));

        cardAttrDropdown.options = list;
    }

    public void OnFindButtonClick()
    {
        string keyWord = keyWordInputField.text;
        int cardType = StaticMethod.ChangeCardType(cardTypeDropdown.captionText.text);
        int cardRace = StaticMethod.ChangeCardRace(cardRaceDropdown.captionText.text);
        int cardAttr = StaticMethod.ChangeCardAttr(cardAttrDropdown.captionText.text);

        string str = cardLevelDropdown.captionText.text;
         int cardLevel=0;
       
        if (str != "N/A")
        {
            str = str.Replace("星", "");
            
            cardLevel = int.Parse(str);
        }
        if (!ComVal.isMonster(cardType))
        {
            if (cardRace != 0 || cardAttr != 0 || cardLevel != 0)
            {
                return;
            }
        }
        editUI.FindCard(cardType, cardAttr, cardRace, cardLevel, keyWord);
    }
}

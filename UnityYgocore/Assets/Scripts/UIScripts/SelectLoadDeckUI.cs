using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectLoadDeckUI : MonoBehaviour
{
    #region 单例
    private static SelectLoadDeckUI instance;

    public SelectLoadDeckUI()
    {
        instance = this;
    }

    public static SelectLoadDeckUI GetInstance()
    {
        return instance;
    }
    #endregion

    public Dropdown deckNameDropDown;
    public Button sortDeckButton;
    public Button saveAsButton;
    public Button saveButton;
    public Button exitButton;
    public Button deleteDeckButton;
    public Button clearDeckButton;
    public InputField deckNameInputField;


    public List<string> deckNameList;

    public EditUI editUI;

    ErrorPlane errorPlane;
    public void Init()
    {
        deckNameDropDown = this.transform.FindChild("DeckNameDropdown").GetComponent<Dropdown>();
        sortDeckButton = this.transform.FindChild("SortDeckButton").GetComponent<Button>();
        saveAsButton = this.transform.FindChild("SaveDeckButton").GetComponent<Button>();
        saveButton = this.transform.FindChild("SaveButton").GetComponent<Button>();
        exitButton = this.transform.FindChild("ExitToMainMenu").GetComponent<Button>();
        deleteDeckButton = this.transform.FindChild("DeleteDeckButton").GetComponent<Button>();
        clearDeckButton = this.transform.FindChild("ClearDeckMenu").GetComponent<Button>();
        deckNameInputField = this.transform.FindChild("DeckNameInputField").GetComponent<InputField>();


        sortDeckButton.onClick.AddListener(OnSortButtonClick);
        saveAsButton.onClick.AddListener(OnSaveAsButtonClick);
        saveButton.onClick.AddListener(OnSaveButtonClick);
        exitButton.onClick.AddListener(OnExitButton);
        deleteDeckButton.onClick.AddListener(OnDeleteDeckButton);
        clearDeckButton.onClick.AddListener(OnClearDeckButton);
        deckNameDropDown.onValueChanged.AddListener(OnSelectDeck);

        deckNameDropDown.options = new List<Dropdown.OptionData>();

        deckNameList = new List<string>();

        editUI = EditUI.GetInstance();
        errorPlane = ErrorPlane.GetInstance();

        InitDeckNameDropDown();

        if (deckNameDropDown.options.Count != 0)
            editUI.ShowDeck(deckNameDropDown.captionText.text);
    }

    void InitDeckNameDropDown()
    {
        deckNameList = DeckLoad.GetDeckNameList();
        List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
        for (int i = 0; i < deckNameList.Count; i++)
        {
            string str = deckNameList[i];
            Dropdown.OptionData data = new Dropdown.OptionData(str);
            list.Add(data);
        }
        deckNameDropDown.options = list;
    }

    void OnSaveButtonClick()
    {
        string str = deckNameDropDown.captionText.text;
        if(!editUI.SaveDeck(str))
        {
            errorPlane.Show("卡组不能为空卡组");
            return;
        }
        InitDeckNameDropDown();
        errorPlane.Show("保存成功");
    }

    void OnSaveAsButtonClick()
    {
        string deckName=deckNameInputField.text;
        if (deckName== "")
        {
            errorPlane.Show("请输入卡组名字");
            return;
        }
        foreach (var item in deckNameList)
        {
            if(deckName==item)
            {
                errorPlane.Show("已存在该卡组");
                return;
            }
        }
        string str = deckNameInputField.text;
        if(!editUI.SaveDeck(str))
        {
            errorPlane.Show("卡组不能为空卡组");
            return;
        }
        InitDeckNameDropDown();
        errorPlane.Show("另保存成功");
    }

    void OnClearDeckButton()
    {
        editUI.ClearDeck();
    }

    void OnExitButton()
    {
        UIMgr.Instance().LoadUI(ComStr.UI_LoginUI);
    }

    void OnDeleteDeckButton()
    {
        string str = deckNameDropDown.captionText.text;
        if (str == "")
        {
            errorPlane.Show("请选择要删除的卡组");
            return;
        }
        editUI.DeleteDeck(str);
        InitDeckNameDropDown();
        errorPlane.Show("删除成功");
        editUI.ShowDeck(deckNameDropDown.captionText.text);
    }

    /// <summary>
    /// 排序
    /// </summary>
    public void OnSortButtonClick()
    {
        editUI.SortDeck();
    }
    /// <summary>
    /// 选择卡组
    /// </summary>
    /// <param name="val"></param>
    public void OnSelectDeck(int val)
    {
        string str = deckNameDropDown.captionText.text;
        editUI.ShowDeck(str);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Protocol;
public class UIMgr : MonoBehaviour
{
    #region 单例
    private static UIMgr instance;

    public UIMgr()
    {
        instance = this;
    }

    public static UIMgr GetInstance()
    {
        return instance;
    }
    #endregion

    public List<BaseUI> uiList;


    private BaseUI duelUI;
    private BaseUI editCardUI;
    private BaseUI loginUI;
    private BaseUI gameHallUI;
    private BaseUI prepareUI;
   // public BaseUI errorPlane;
    void Awake()
    {
        LoadXml.LoadTheXml();
        uiList = new List<BaseUI>();

        //duelFieldUI = GameFieldUI.GetInstance();
        duelUI = DuelUIManager.GetInstance();
        duelUI.SetName(ComStr.UI_DuelFieldUI);
        editCardUI = EditUI.GetInstance();
        editCardUI.SetName(ComStr.UI_EditCardUI);
        loginUI = LoginUI.GetInstance();
        loginUI.SetName(ComStr.UI_LoginUI);
        gameHallUI = GameHallUI.GetInstance();
        gameHallUI.SetName(ComStr.UI_GameHallUI);
        prepareUI = PrepareUI.GetInstance();
        prepareUI.SetName(ComStr.UI_PerpareUI);

        //editCardUI.Init();
        prepareUI.Init();

        uiList.Add(duelUI);
        uiList.Add(editCardUI);
        uiList.Add(loginUI);
        uiList.Add(gameHallUI);
        uiList.Add(prepareUI);
    }


    public void LoadUI(string str)
    {

        for (int i = 0; i < uiList.Count; i++)
        {
            if (uiList[i].name == str)
            {
                for (int j = 0; j < uiList.Count; j++)
                {
                    uiList[j].SetUIActive(false);
                }
                if (uiList[i] == editCardUI)
                {
                    editCardUI.Init();
                }
                uiList[i].SetUIActive(true);
            }
        }
    }

    public void HideAllUI()
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            uiList[i].SetUIActive(false);
        }
    }
}

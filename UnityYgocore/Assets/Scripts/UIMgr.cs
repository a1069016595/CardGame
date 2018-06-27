using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Protocol;
public class UIMgr : MonoSingleton<UIMgr>
{

    private GameObject duelFieldUI;
    public GameObject prepareUI;

    public List<BaseUI> uiList;


    private BaseUI editCardUI;
    private BaseUI loginUI;
    private BaseUI gameHallUI;


    private GameObject curUI;


    void Awake()
    {
        LoadXml.LoadTheXml();
        uiList = new List<BaseUI>();

        duelFieldUI = Resources.Load("Prefebs/DuelFieldUI") as GameObject;
        prepareUI = Resources.Load("Prefebs/PrepareUI") as GameObject;

        editCardUI = EditUI.GetInstance();
        editCardUI.SetName(ComStr.UI_EditCardUI);
        loginUI = LoginUI.GetInstance();
        loginUI.SetName(ComStr.UI_LoginUI);
        gameHallUI = GameHallUI.GetInstance();
        gameHallUI.SetName(ComStr.UI_GameHallUI);


        uiList.Add(editCardUI);
        uiList.Add(loginUI);
        uiList.Add(gameHallUI);
    }


    public void LoadUI(string str)
    {
        DestroyCurUI();

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
                uiList[i].Show();
            }
        }
    }

    private void DestroyCurUI()
    {
        if (curUI != null)
        {
            GameObject.Destroy(curUI);
            curUI = null;
        }
    }

    public void HideAllUI()
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            uiList[i].SetUIActive(false);
        }
    }

    public void LoadDuelField()
    {
        if (curUI != null && curUI.name.Contains("DuelFieldUI"))
        {
            return;
        }
        LoadUIObj(duelFieldUI);
    }

    public void LoadPerpareUI()
    {
        if (curUI != null && curUI.name.Contains("PrepareUI"))
        {
            return;
        }
        LoadUIObj(prepareUI);
    }

    private void LoadUIObj(GameObject val)
    {
        DestroyCurUI();
        HideAllUI();
        GameObject obj = Instantiate(val, transform);
        obj.GetComponent<RectTransform>().localPosition = duelFieldUI.GetComponent<RectTransform>().position;
        obj.GetComponent<RectTransform>().localScale = duelFieldUI.GetComponent<RectTransform>().localScale;

        obj.transform.SetAsFirstSibling();
        curUI = obj;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 聊天框组件
/// </summary>
public class ChattingUI : MonoBehaviour
{
   
    //初始点与终点数值
    public float maxY;
    public float minY;

    private Scrollbar scrollbar;
    public Text text;


    private List<string> chatMesList;

    RectTransform rectTransform;


    public void Init()
    {
        chatMesList = new List<string>();
       
        rectTransform = text.GetComponent<RectTransform>();

        scrollbar = transform.FindChild("Scrollbar").GetComponent<Scrollbar>();

        scrollbar.onValueChanged.AddListener(OnScrollbarValueChange);

        text.text = "";
        scrollbar.gameObject.SetActive(false);
    }

    void OnScrollbarValueChange(float value)
    {
        rectTransform.anchoredPosition =new Vector2(rectTransform.anchoredPosition.x, (float)((rectTransform.sizeDelta.y - 142.0) *(1.0- value)));
    }

    /// <summary>
    /// 添加语句
    /// </summary>
    public void AddSentence(string str)
    {
        chatMesList.Add(str);
        scrollbar.size = (float)(142.0 / (chatMesList.Count * (440.0 / 20.0)));
        if(scrollbar.size==1)
        {
            scrollbar.gameObject.SetActive(false);//当size为1时 隐藏滑动条
            UpdateText();
            return;
        }
        else
        {
            if(!scrollbar.gameObject.activeSelf)
                scrollbar.gameObject.SetActive(true);
        }
        while(chatMesList.Count>20)
        {
            chatMesList.RemoveAt(0);
        }
        UpdateText();
      
        //Debug.Log((float)(chatMesList.Count * (float)(440 / 20)));
        //Debug.Log((float)(chatMesList.Count * (440.0 / 20.0)));
        rectTransform.sizeDelta =new Vector2(rectTransform.sizeDelta.x, (float) (chatMesList.Count * (440.0 / 20.0)));
        scrollbar.value = 0;
        OnScrollbarValueChange(0);
    }

    void UpdateText()
    {
        text.text = "";
        for (int i = 0; i < chatMesList.Count; i++)
        {
            text.text = text.text + chatMesList[i] + "\n";
        }
    }

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LPSliderUI : MonoBehaviour
{

    private Image LPimage;
    private Text LPText;
    private Text playerNameText;

    bool isPlay = false;

    int curLp;

    public float animTime;

    float timer=0;

    public void Init(string playerName)
    {
        LPimage = transform.FindChild("LPimage").GetComponent<Image>();
        LPText = transform.FindChild("LPText").GetComponent<Text>();
        playerNameText = transform.FindChild("playerName").GetComponent<Text>();

        playerNameText.text = playerName;
        curLp = 8000;
    }


    public void AddLp(int val)
    {
        curLp += val;
        LPimage.fillAmount = (float)curLp/8000;
        LPText.text = curLp.ToString();
    }

    public void ReduceLp(int val)
    {
        curLp -= val;
        curLp = curLp.GetPositive();
        LPimage.fillAmount =(float) curLp / 8000;
        LPText.text = curLp.ToString();
    }

}

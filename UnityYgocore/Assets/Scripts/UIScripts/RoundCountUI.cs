using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCountUI : MonoBehaviour
{
    Text roundText;    

    void Awake()
    {
        roundText = GetComponent<Text>();
    }

    public void SetRoundCount(int val)
    {
        roundText.text = val.ToString();
    }
}

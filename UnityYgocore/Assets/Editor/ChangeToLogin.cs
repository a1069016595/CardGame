using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangeToLogin : MonoBehaviour
{

    [MenuItem("Tool/转换模式")]
    public static void ChangeLogin()
    {
        GameObject parent = GameObject.Find("BG");
        GameObject duel =parent.transform.Find("DuelFieldUI").gameObject;
        GameObject login = parent.transform.Find("LoginUI").gameObject;
        if(duel.activeSelf)
        {
            duel.SetActive(false);
            login.SetActive(true);
        }
        else
        {
            duel.SetActive(true);
            login.SetActive(false);
        }
    }
}

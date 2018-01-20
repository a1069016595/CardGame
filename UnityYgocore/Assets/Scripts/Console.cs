using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public bool isLog = true;

    public void Log(object mes)
    {
        if (isLog)
        {
            Debug.Log(mes);
        }
    }
}

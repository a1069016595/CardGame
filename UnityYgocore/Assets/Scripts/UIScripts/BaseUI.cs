﻿using UnityEngine;
using System.Collections;

public class BaseUI : MonoBehaviour
{
    public string name;

    public void SetName(string str)
    {
        this.name = str;
    }

    public void SetUIActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public virtual void Init()
    {

    }
}

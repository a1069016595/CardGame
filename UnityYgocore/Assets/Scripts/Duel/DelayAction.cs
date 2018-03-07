using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAction 
{
    public normalDele action;
    public int lauchCode;
    public int lauchRoundCount;

    public int startRound;

    public DelayAction(normalDele action, int lauchCode, int lauchRoundCount)
    {
        this.action = action;
        this.lauchCode = lauchCode;
        this.lauchRoundCount = lauchRoundCount;
    }
}

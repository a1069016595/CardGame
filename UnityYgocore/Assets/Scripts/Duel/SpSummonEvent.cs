using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpSummonEvent
{
    public Card invalidReasonCard;
    public BaseEffect invalidReasonEffect;

    public Group spSummonGroup;

    public bool IsInvalid
    {
        get { return isInvalid; }
    }

    private bool isInvalid;

    public SpSummonEvent(Group g)
    {
        this.spSummonGroup = g;
        isInvalid = false;
    }

    public void SetInvalid(Card invalidReasonCard, BaseEffect invalidReasonEffect)
    {
        this.invalidReasonCard = invalidReasonCard;
        this.invalidReasonEffect = invalidReasonEffect;
        isInvalid = true;
    }
}

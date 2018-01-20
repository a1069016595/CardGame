using System;


public class Reason
{
    public int reason;
    public Card card;
    public BaseEffect effect;

    public Reason(int _reason, Card _reasonCard, BaseEffect _reasonEffect)
    {
        reason = _reason;
        card = _reasonCard;
        effect = _reasonEffect;
    }

}


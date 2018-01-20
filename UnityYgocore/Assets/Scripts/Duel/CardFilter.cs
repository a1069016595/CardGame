/// <summary>
/// 用于卡片属性的检测
/// </summary>
public class CardFilter
{
    int attribute;
    object val;
    int attribute1;
    object val1;

    public CardFilter(int _attribute, object _val)
    {
        attribute = _attribute;
        val = _val;
    }

    public CardFilter(int _attribute, int _val, int _attribute1, int _val1)
    {
        attribute = _attribute;
        val = _val;
        attribute1 = _attribute1;
        val1 = _val1;
    }

    public bool isFit(Card card)
    {
        bool result = false;
        switch (attribute)
        {
            case ComVal.fiter_containName:
                result = card.ContainName((string)val);
                break;
            case ComVal.fiter_isAttribute:
                result = ComVal.isBind((int)val,card.GetCurAttribute());
                break;
            case ComVal.fiter_isLevel:
                result = card.level == (int)val;
                break;
            case ComVal.fiter_isCardType:
                result = ComVal.isBind((int)val, card.cardType);
                break;
            case ComVal.fiter_isArea:
                result = ComVal.isBind((int)val, card.curArea);
                break;
        }
        return result;
    }
}

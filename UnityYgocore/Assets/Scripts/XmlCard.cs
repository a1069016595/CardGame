

/// <summary>
/// 用于记录卡片的属性
/// </summary>
public class XmlCard
{
    public string cardID;
    public string cardName;
    public string cardDescribe;
    public string cardType;
    public int afk;
    public int def;
    public int level;
    public string race;
    public string attribute;



    public XmlCard(string _cardID, string _cardName, string _cardDescribe, string _cardType)
    {
        cardID = _cardID;
        cardName = _cardName;
        cardDescribe = _cardDescribe;
        cardType = _cardType;
    }

    public void SetMonsterAttribute(string _attribute, string _race, int _level, int _afk, int _def)
    {
        attribute = _attribute;
        level = _level;
        race = _race;
        afk = _afk;
        def = _def;
    }

    public bool IsMonster()
    {
        if (cardType.Contains("怪兽"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

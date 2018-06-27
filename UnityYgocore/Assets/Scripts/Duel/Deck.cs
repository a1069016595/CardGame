
public class Deck
{
    public Group mainDeck;
    public Group extraDeck;

    public Deck()
    {
        mainDeck = new Group();
        extraDeck = new Group();
    }

    public void ClearDeck()
    {
        mainDeck.cardList.Clear();
        extraDeck.cardList.Clear();
    }

    /// <summary>
    /// 生成卡组
    /// </summary>
    /// <param name="main"></param>
    /// <param name="extra"></param>
    public Deck(string[] main, string[] extra)
    {
        mainDeck = new Group();
        extraDeck = new Group();
        for (int i = 0; i < main.Length; i++)
        {
            Card card = CardFactroy.GenerateCard(main[i]);
            mainDeck.AddCard(card);
        }
        for (int i = 0; i < extra.Length; i++)
        {
            Card card = CardFactroy.GenerateCard(extra[i]);
            extraDeck.AddCard(card);
        }
    }

    public void AddCardToMain(Card card)
    {
        mainDeck.AddCard(card);
    }


    public void AddCardToMain(string id)
    {
        Card card = CardFactroy.GenerateCard(id);
        mainDeck.AddCard(card);
    }

    public void AddCardToExtra(string id)
    {
        Card card = CardFactroy.GenerateCard(id);
        extraDeck.AddCard(card);
    }

    public void AddCardToExtra(Card card)
    {
        extraDeck.AddCard(card);
    }

    public bool isNull()
    {
        if (mainDeck.cardList.Count == 0 && extraDeck.cardList.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 排序卡组
    /// </summary>
    public void SortDeck()
    {
        mainDeck.SortGroup();
        extraDeck.SortGroup();
    }

    public string[] GetMainDeck()
    {
        return mainDeck.GetCardIDList().ToArray();
    }

    public string[] GetExtraDeck()
    {
        return extraDeck.GetCardIDList().ToArray();
    }
}


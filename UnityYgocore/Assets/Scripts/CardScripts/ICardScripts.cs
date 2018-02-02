
public interface ICardScripts
{
    void InitialEffect(Card card, Player player, IDuel duel);

    void Operation(IDuel duel,Card card, LauchEffect effect,Group group=null);

    bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code);


}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DeckLoad : MonoBehaviour
{
    /// <summary>
    /// 加载卡组
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Deck LoadDeck(string name)
    {
        bool readMain = false;
        bool readExtra = false;
        // Debug.Log(Application.persistentDataPath + "/" + name);
        Deck deck = new Deck();
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + name + ".ydk");
       // Debug.Log(Application.persistentDataPath);
        string line;

        for (int i = 0; i < 200; i++)
        {
            line = sr.ReadLine();
            if (line == "#main")
            {
                readMain = true;
                continue;
            }
            if (line == "#extra")
            {
                readMain = false;
                readExtra = true;
                continue;
            }
            if (line == "#end")
            {
                break;
            }
            if (readMain == true)
            {

                Card card = CardFactroy.GenerateCard(line);
                if (card.IsMonster())
                {
                    card.SetMonsterAttribute(card.attribute, card.race, card.level, card.afk, card.def);
                }
                deck.AddCardToMain(card);
            }

            if (readExtra == true)
            {
                Card card = CardFactroy.GenerateCard(line);
             
                if (card.IsMonster())
                {
                    card.SetMonsterAttribute(card.attribute, card.race, card.level, card.afk, card.def);
                }
                deck.AddCardToExtra(card);
            }

        }
        sr.Close();
        return deck;
    }

    /// <summary>
    /// 获取所有卡组名
    /// </summary>
    public static List<string> GetDeckNameList()
    {
        List<string> deckNmae = new List<string>();
        DirectoryInfo info = new DirectoryInfo(Application.streamingAssetsPath);

        FileInfo[] infos = info.GetFiles();
        foreach (var item in infos)
        {

            if (item.Name.Contains("meta"))
            {
                continue;
            }
            if (item.Name.Contains("ydk"))
            {
                string name = item.Name.Replace(".ydk", "");
                deckNmae.Add(name);


            }
        }
        return deckNmae;
    }

    /// <summary>
    /// 保存卡组
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="deckName"></param>
    public static void SaveDeck(Deck deck, string deckName)
    {
        Group mainDeck = deck.mainDeck;
        Group extraDeck = deck.extraDeck;
        FileStream fs = new FileStream(Application.streamingAssetsPath + "/" + deckName + ".ydk", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine("#main");

        foreach (var card in mainDeck.cardList)
        {
            sw.WriteLine(card.cardID);
        }
        sw.WriteLine("#extra");
        foreach (var card in extraDeck.cardList)
        {
            sw.WriteLine(card.cardID);
        }
        sw.WriteLine("#end");
        sw.Flush();
        sw.Close();
    }

    public static void DeleteDeck(string deckName)
    {
        string path = Application.streamingAssetsPath + "/" + deckName + ".ydk";
        if (File.Exists(path))
            File.Delete(path);
        else
            Debug.Log("文件不存在");
    }
}

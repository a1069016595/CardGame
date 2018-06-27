using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayBackLoad
{
    public static PlayBackMes GetPlayBackMes(string name)
    {
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + name + ".PB");
        string date = sr.ReadLine();
        string player1Name = sr.ReadLine();
        string player2Name = sr.ReadLine();

        return new PlayBackMes(player1Name, player2Name, date);
    }

    public static List<string> GetPlayBackList()
    {
        List<string> result = new List<string>();

        DirectoryInfo info = new DirectoryInfo(Application.streamingAssetsPath);

        FileInfo[] infos = info.GetFiles();
        foreach (var item in infos)
        {
            if (item.Name.Contains("meta"))
            {
                continue;
            }
            if (item.Name.Contains("PB"))
            {
                string name = item.Name.Replace(".PB", "");
                result.Add(name);
            }
        }
        return result;
    }
}

public class PlayBackMes
{
    public string p1;
    public string p2;
    public string date;

    public PlayBackMes(string p1,string p2,string date)
    {
        this.p1=p1;
        this.p2=p2;
        this.date=date;
    }
}
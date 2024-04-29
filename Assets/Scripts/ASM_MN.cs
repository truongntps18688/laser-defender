using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading;

public class ASM_MN : Singleton<ASM_MN>
{
    public List<Region> listRegion = new List<Region>();
    public List<Players> listPlayer = new List<Players>();

    private void Awake()
    {
        createRegion();
    }

    public void createRegion()
    {
        listRegion.Add(new Region(0, "VN"));
        listRegion.Add(new Region(1, "VN1"));
        listRegion.Add(new Region(2, "VN2"));
        listRegion.Add(new Region(3, "JS"));
        listRegion.Add(new Region(4, "VS"));
    }

    public string calculate_rank(int score)
    {
        if (score < 100)
            return "đồng";
        else if (score <= 500)
            return "bạc";
        else if (score <= 1000)
            return "vàng";
        else
            return "kim cương";
    }

    public void YC1()
    {
        int ID = ScoreKeeper.Instance.GetID();
        string name = ScoreKeeper.Instance.GetUserName();
        int score = ScoreKeeper.Instance.GetScore();
        int IDregion = ScoreKeeper.Instance.GetIDregion();
        Players newPlayer = new Players(ID, name, score, listRegion[IDregion]);
        listPlayer.Add(newPlayer);
    }
    public void YC2()
    {
        for (int i = 0; i < listPlayer.Count; i++)
        {
            Players item = listPlayer[i];
            Debug.Log("ID: " + item.ID);
            Debug.Log("name: " + item.name);
            Debug.Log("score: " + item.score);
            Debug.Log("name region: " + item.region.Name);
            Debug.Log("rank: " + calculate_rank(item.score));
        }
    }
    public void YC3()
    {
        for (int i = 0; i < listPlayer.Count; i++)
        {
            Players item = listPlayer[i];
            if (item.score < ScoreKeeper.Instance.GetScore())
                continue;
            Debug.Log("ID: " + item.ID);
            Debug.Log("name: " + item.name);
            Debug.Log("score: " + item.score);
            Debug.Log("name region: " + item.region.Name);
            Debug.Log("rank: " + calculate_rank(item.score));
        }
    }
    public void YC4()
    {
        Players foundPlayer = listPlayer.FirstOrDefault(player => player.ID == ScoreKeeper.Instance.GetID());

        if (foundPlayer != null)
        {
            Debug.Log($"Tìm thấy người chơi với ID {ScoreKeeper.Instance.GetID()}: {foundPlayer.name}");
            foundPlayer.name = "?????????";
            YC2(); // test
        }
        else
        {
            Debug.Log($"Không tìm thấy người chơi với ID {ScoreKeeper.Instance.GetID()}");
        }
    }
    public void YC5()
    {
        var sortedPlayers = listPlayer.OrderByDescending(player => player.score);

        foreach (var player in sortedPlayers)
        {
            Debug.Log(player);
        }
    }
    public void YC6()
    {
        if (listPlayer.Count < 5)
        {
            foreach (var player in listPlayer)
            {
                Debug.Log(player);
            }
            return;
        }
        var sortedPlayers = listPlayer.OrderBy(player => player.score);
        var lowestScorePlayers = sortedPlayers.Take(5);


        YC2();
    }
    public void YC7()
    {
        Thread thread = new Thread(CalculateAndSaveAverageScoreByRegion);
        thread.Name = "BXH";
        thread.Start();
    }
    void CalculateAndSaveAverageScoreByRegion()
    {
        Dictionary<int, List<int>> regionScores = new Dictionary<int, List<int>>();
        foreach (var player in listPlayer)
        {
            if (!regionScores.ContainsKey(player.region.ID))
            {
                regionScores[player.region.ID] = new List<int>();
                Debug.Log(player.region.ID);
            }

            regionScores[player.region.ID].Add(player.score);
        }

        using (StreamWriter writer = new StreamWriter("bxhRegion.txt"))
        {
            foreach (var kvp in regionScores)
            {
                int regionID = kvp.Key;
                List<int> scores = kvp.Value;

                float averageScore = (float)scores.Sum() / scores.Count;

                writer.WriteLine($"Region ID: {regionID}, Average Score: {averageScore}");
            }
        }
    }

}

[SerializeField]
public class Region
{
    public int ID;
    public string Name;
    public Region(int ID, string Name)
    {
        this.ID = ID;
        this.Name = Name;
    }
}

[SerializeField]
public class Players
{
    public int ID;
    public string name;
    public int score;
    public Region region;
    public Players(int ID, string name, int score, Region region)
    {
        this.ID = ID;
        this.name = name;
        this.score = score;
        this.region = region;
    }
}
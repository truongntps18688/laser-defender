using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.IO;

public class ScoreKeeper : MonoBehaviour
{
    public int ID;
    public int score;
    public string userName;
    public int IDregion;

    static ScoreKeeper instance;

    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if(instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ModifyScore(int value)
    {
        score += value;
        Mathf.Clamp(score, 0, int.MaxValue);
    }

    public void ResetScore(string userName, int IDregion)
    {
        score = 0;
        this.ID = Random.Range(0,19999);
        this.userName = userName;
        this.IDregion = IDregion;
    }
    
}

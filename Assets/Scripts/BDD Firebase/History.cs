using System;
using UnityEngine;

[Serializable]
public class History
{
    [HideInInspector]
    public string ExerciceId;
    public string UserId;
    public int Score;
    public TimeSpan Time;
    public DateTime CreationDate;

    public History() 
    {    
        CreationDate = DateTime.Now.ToString();
    }

    public History(string exerciceId, string userId, int score, TimeSpan time, DateTime creationDate = null)
    {
        ExerciceId = exerciceId;
        UserId = userId;
        Score = score;
        Time = time;
        if (creationDate == null)
            CreationDate = DateTime.Now;
        else
            CreationDate = creationDate;
    }
}
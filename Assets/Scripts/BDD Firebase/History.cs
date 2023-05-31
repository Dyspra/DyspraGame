using System;
using UnityEngine;

[Serializable]
public class History
{
    [HideInInspector]
    public string ExerciseId;
    public string UserId;
    public int Score;
    public string CreationDate;

    public History() 
    {    
        CreationDate = DateTime.Now.ToString();
    }

    public History(string exerciseId, string userId, int score, DateTime creationDate = default(DateTime))
    {
        ExerciseId = exerciseId;
        UserId = userId;
        Score = score;
        if (creationDate == default(DateTime))
            CreationDate = DateTime.Now.ToString();
        else
            CreationDate = creationDate.ToString();
    }
}
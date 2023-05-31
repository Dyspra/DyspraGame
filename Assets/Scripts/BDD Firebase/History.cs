using System;
using UnityEngine;

[Serializable]
public class History
{
    [HideInInspector]
    public string ExerciseId;
    public string UserId;
    public int Score;
    public DateTime CreationDate;

    public History() 
    {    
        CreationDate = DateTime.Now;
    }

    public History(string exerciseId, string userId, int score, DateTime creationDate = default(DateTime))
    {
        ExerciseId = exerciseId;
        UserId = userId;
        Score = score;
        if (creationDate == default(DateTime))
            CreationDate = DateTime.Now;
        else
            CreationDate = creationDate;
    }
}
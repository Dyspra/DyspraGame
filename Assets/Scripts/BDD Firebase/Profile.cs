using System;
using UnityEngine;

[Serializable]
public class Profile
{
    [HideInInspector]
    public string UserId;
    public string Username;
    public string FirstName;
    public string SurName;
    public string CreationDate;

    public Profile() 
    {    
        CreationDate = DateTime.Now.ToString();
    }

    public Profile(string userId, string username, string firstName, string surName, string creationDate)
    {
        UserId = userId;
        Username = username;
        FirstName = firstName;
        SurName = surName;
        CreationDate = creationDate;
    }
}
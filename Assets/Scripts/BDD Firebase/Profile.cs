using System;
using UnityEngine;

[Serializable]
public class Avatar
{
    public int body;
    public int face;
    public int hair;
    public int kit;

    public Avatar(int body, int face, int hair, int kit)
    {
        this.body = body;
        this.face = face;
        this.hair = hair;
        this.kit = kit;
    }

    public Avatar()
    {
        body = 0;
        face = 0;
        hair = 0;
        kit = 0;
    }
}

[Serializable]
public class Profile
{
    [HideInInspector]
    public string UserId;
    public string Username;
    public string FirstName;
    public string SurName;
    public string CreationDate;
    public Avatar Avatar;

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
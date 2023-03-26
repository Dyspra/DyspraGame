using System;
using UnityEngine;

[Serializable]
public class Profile
{
    [HideInInspector]
    public string UserId;
    public string Username;

    public Profile() {    }

    public Profile(string userId, string username)
    {
        UserId = userId;
        Username = username;
    }
}

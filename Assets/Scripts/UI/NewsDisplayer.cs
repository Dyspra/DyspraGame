using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class New
{
    public string NewId;
    public string NewText;
}

public class NewsDisplayer : MonoBehaviour
{
    [SerializeField] GameObject NotifNumber;
    [SerializeField] Text NotifNumberText;
    [SerializeField] GameObject NewsPanel;
    [SerializeField] Text NewsText;
    List<New> News;
    float lastCheckTime;
    readonly float checkInterval = 0.5f;

    void Start()
    {
        if (BDDInteractor.Instance.GetCachedNews() == null)
            BDDInteractor.Instance.FetchNews();
        lastCheckTime = Time.time;
    }

    private void Update()
    {
        if (News == null && Time.time - lastCheckTime >= checkInterval)
        {
            lastCheckTime = Time.time;
            List<New> news_tmp = BDDInteractor.Instance.GetCachedNews();
            if (news_tmp != null)
            {
                News = new List<New>();
                int unseenNewsCount = 0;
                foreach (New single_new in news_tmp)
                {
                    if (!WasNewsSeen(single_new.NewId))
                    {
                        unseenNewsCount++;
                    }
                    News.Add(single_new);
                    NewsText.text += "- " + single_new.NewText + "\n";
                }
                if (unseenNewsCount > 0)
                {
                    NotifNumber.SetActive(true);
                    NotifNumberText.text = unseenNewsCount.ToString();
                }
            }
        }
    }

    public void OpenNews()
    {
        foreach (New single_new in News)
        {
            MarkNewsAsSeen(single_new.NewId);
        }
        NotifNumber.SetActive(false);
        NewsPanel.SetActive(true);
    }

    private bool WasNewsSeen(string newsId)
    {
        return PlayerPrefs.GetString("SeenNews", "").Contains(newsId);
    }

    private void MarkNewsAsSeen(string newsId)
    {
        string seenNews = PlayerPrefs.GetString("SeenNews", "");
        string[] seenNewsArray = seenNews.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (Array.Exists(seenNewsArray, element => element == newsId))
            return;

        seenNews += (string.IsNullOrEmpty(seenNews) ? "" : ",") + newsId;
        PlayerPrefs.SetString("SeenNews", seenNews);
        PlayerPrefs.Save();
    }

    [Button]
    void ResetSeenNews()
    {
        PlayerPrefs.SetString("SeenNews", "");
        lastCheckTime = Time.time;
        NewsText.text = "";
        News = null;
        BDDInteractor.Instance.FetchNews();
    }
}

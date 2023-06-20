using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
//using GooglePlayGames.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    private static Dictionary<string, string> _achievementID = new Dictionary<string, string>() {
        {"Enma no Champion", "CgkIvpfI760aEAIQAg"},
        {"This'll tickle their feet", "CgkIvpfI760aEAIQCA"},
        {"Green Day", "CgkIvpfI760aEAIQBQ"},
        {"I've got balls of steel", "CgkIvpfI760aEAIQBw"},
        {"Here comes a new challenger", "CgkIvpfI760aEAIQAA"},
        {"It's a trap!", "CgkIvpfI760aEAIQBA"},
        {"How the hell?", "CgkIvpfI760aEAIQBg"},
        {"Enma no Danjon", "CgkIvpfI760aEAIQAQ"},
        {"Glue you back together, IN HELL", "CgkIvpfI760aEAIQAw"}
    };

    private static GooglePlayManager _instance;

    public static GooglePlayManager Instance
    {
        get { return _instance; }
    }

    private bool _isAuthenticated = false;

    public bool IsAuthenticated
    {
        get { return _isAuthenticated; }
    }

    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("GooglePlayManager created");
        }
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("GooglePlayManager: ProcessAuthentication: Signed in!");
            _isAuthenticated = true;
        }
        else
        {
            Debug.Log("GooglePlayManager: ProcessAuthentication: Sign-in failed!");
            _isAuthenticated = false;
        }
    }

    public void HandleAchievement(string achievementName)
    {
        string achievementID = _achievementID[achievementName];

        if (!_isAuthenticated)
            return;
        Social.ReportProgress(achievementID, 100.0f, (bool success) => {
            if (success)
            {
                Debug.Log("GooglePlayManager: HandleAchievement: ReportProgress: success");
            }
            else
            {
                Debug.Log("GooglePlayManager: HandleAchievement: ReportProgress: failed");
            }
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
//using GooglePlayGames.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    // private static Dictionary<string, string> _achievementID = {
    //     {"Here comes a new challenger", "CgkIvpfI760aEAIQAA" }
    // };

    // private Dictionary<string, string> _achievementID = {
    //     {"Enma no Champion", "CgkIvpfI760aEAIQAg"},
    //     {"This'll tickle their feet", "CgkIvpfI760aEAIQCA"},
    //     {"Green Day", "CgkIvpfI760aEAIQBQ"},
    //     {"I've got balls of steel", "CgkIvpfI760aEAIQBw"},
    //     {"Here comes a new challenger", "CgkIvpfI760aEAIQAA"},
    //     {"It's a trap!", "CgkIvpfI760aEAIQBA"},
    //     {"How the hell?", "CgkIvpfI760aEAIQBg"},
    //     {"Enma no Danjon", "CgkIvpfI760aEAIQAQ"},
    //     {"Glue you back together in hell", "CgkIvpfI760aEAIQAw"}
    // };

    private static GooglePlayManager _instance;

    public static GooglePlayManager Instance
    {
        get { return _instance; }
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
        }
        else
        {
            Debug.Log("GooglePlayManager: ProcessAuthentication: Sign-in failed!");
        }
    }

    private void HandleAchievement(string achievementName)
    {
        string achievementID = _achievementID[achievementName];

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

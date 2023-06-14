using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
//using GooglePlayGames.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    private static Dictionary<string, string> _achievementID = new Dictionary<string, string>{
        {"Here comes a new challenger", "CgkIvpfI760aEAIQAA" }
    };
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
        //string achievementID = _achievementID[achievementName];

        Social.ReportProgress(achievementName, 100.0f, (bool success) => {
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

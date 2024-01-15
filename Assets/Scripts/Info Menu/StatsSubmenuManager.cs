using Cysharp.Threading.Tasks;
using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class StatsSubmenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text personalStats;
    [SerializeField] TMP_Text globalStats;

    string personalStatsText;
    string globalStatsText;

    private void Awake()
    {
        personalStatsText = personalStats.text;
        globalStatsText = globalStats.text;
    }

    public void OpenStatsSubmenu()
    {
        UIManager.OpenSubMenu(gameObject);
    }

    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => PGManager.finishedSetup == true);
        if (!PlayerAuthentication.LoggedIn)
        {
            personalStats.text = personalStatsText + "Unavailable";
            globalStats.text = globalStatsText + "Unavailable";
            return;
        }

        personalStats.text = personalStatsText;
        globalStats.text = globalStatsText;

        LootLockerSDKManager.GetMemberRank("portraits-generated", PlayerPrefs.GetString("PlayerID"), (response) =>
        {
            if (response.success)
                personalStats.text = personalStatsText + response.score.ToString("N0");
            else
            {
                Debug.LogWarning("Failed to fetch leaderbaord data: " + response.errorData.message);
                personalStats.text = personalStatsText + "Unavailable";
            }
        });

        LootLockerSDKManager.GetScoreList("total-portraits-generated", 1, 0, (response) =>
        {
            if (response.success)
            {
                globalStats.text = globalStatsText + response.items[0].score.ToString("N0");
            }
            else
            {
                Debug.LogWarning("Failed to fetch leaderbaord data: " + response.errorData.message);
                globalStats.text = globalStatsText + "Unavailable";
            }
        });
    }
}

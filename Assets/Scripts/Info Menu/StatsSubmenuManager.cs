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

        LootLockerSDKManager.GetMemberRank("11561", PlayerPrefs.GetString("PlayerID"), (response) =>
        {
            if (response.success)
                personalStats.text = personalStatsText + response.score;
            else
            {
                Debug.LogWarning("Failed to fetch leaderbaord data: " + response.Error);
                personalStats.text = personalStatsText + "Unavailable";
            }
        });

        LootLockerSDKManager.GetScoreList("11560", 1, 0, (response) =>
        {
            if (response.success)
            {
                globalStats.text = globalStatsText + response.items[0].score;
            }
            else
            {
                Debug.LogWarning("Failed to fetch leaderbaord data: " + response.Error);
                globalStats.text = globalStatsText + "Unavailable";
            }
        });
    }
}

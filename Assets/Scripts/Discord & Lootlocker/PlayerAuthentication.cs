using System.Collections;
using UnityEngine;
using LootLocker.Requests;

public class PlayerAuthentication : MonoBehaviour
{
    public static bool LoggedIn { get; private set; } = false;
    private void Awake()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        bool gotResponse = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                //Debug.Log("Successfuly Logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                LoggedIn = true;
                gotResponse = true;
            }
            else
            {
                Debug.Log("Login Failed: " + response.Error);
                gotResponse = true;
            }
        });

        yield return new WaitWhile(() => gotResponse == false);

        if (!LoggedIn) yield break;

        yield return new WaitWhile(() => DiscordController.RetreivedUsername == false);

        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                if (response.name == "" || response.name != DiscordController.Username)
                    StartCoroutine(SetPlayerName(DiscordController.Username));
            }
            else
                Debug.LogError("Unable to get player name: " + response.Error);
        });
    }

    IEnumerator SetPlayerName(string newName)
    {
        bool done = false;
        LootLockerSDKManager.SetPlayerName(newName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfuly set player name: " + newName);
            }
            else
                Debug.Log("Failed to set player name: " + response.Error);

            done = true;
        });

        yield return new WaitWhile(() => done == false);
    }
}

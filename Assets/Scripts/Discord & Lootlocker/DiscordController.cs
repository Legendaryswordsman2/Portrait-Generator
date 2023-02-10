using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
    public static string Username { get; private set; }

    public static bool RetreivedUsername { get; private set; } = false;

    Discord.Discord discord;
    Discord.UserManager userManager;

    private void Awake()
    {
        try
        {
            discord = new Discord.Discord(1073376927840215132, (ulong)Discord.CreateFlags.NoRequireDiscord);
            userManager = discord.GetUserManager();
            userManager.OnCurrentUserUpdate += UserManager_OnCurrentUserUpdate;

            StartCoroutine(UpdateDiscordActivity());
        }
        catch
        {
            Debug.Log("Unable to connect to discord (Discord is likely not open or installed)");
            Destroy(this);
        }
    }


    IEnumerator UpdateDiscordActivity()
    {
        var activityManager = discord.GetActivityManager();

        Discord.Activity activity;
        activity = new Discord.Activity
        {
            State = "Making Portraits",
            Assets =
            {
                LargeImage = "logo",
                LargeText = "Modern UI Portrait Generator"
            },

            Timestamps =
            {
                Start = System.DateTimeOffset.Now.ToUnixTimeMilliseconds()
            }
        };

        bool done = false;

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                //Debug.Log("Discord status set to: " + detailsDescription);
                //Debug.Log("Updated State: " + updateState);
            }
            else
            {
                Debug.Log("Discord status failed!");
            }

            done = true;
        });
        yield return new WaitWhile(() => done == false);
    }
    private void UserManager_OnCurrentUserUpdate()
    {
        Username = userManager.GetCurrentUser().Username + "#" + userManager.GetCurrentUser().Discriminator;

        RetreivedUsername = true;
    }

    private void Update()
    {
        discord.RunCallbacks();
    }
}


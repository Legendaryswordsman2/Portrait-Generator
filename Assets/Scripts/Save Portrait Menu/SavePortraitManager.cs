using Cysharp.Threading.Tasks;
using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavePortraitManager : MonoBehaviour
{
    [SerializeField] TMP_InputField fileNameInputField;
    [SerializeField] TMP_Dropdown sizeDropdown;
    [SerializeField] Button saveButton;

    [Space]

    [SerializeField] PortraitPieceMerger ppMerger;
    [SerializeField] GameObject savePortraitMenus;
    [SerializeField] GameObject creatingPortraitOverlay;

    [Space]

    [SerializeField] GameObject finishedSavingPortraitMenu;
    [SerializeField] TMP_Text personalStatsText;
    [SerializeField] TMP_Text globalStatsText;

    Sprite finalSprite;

    PortraitSize size;

    int portraitsGeneratedPersonal;
    int portraitsGeneratedGlobal;
    bool doneContactingServer = false;
    public static bool SavingPortrait = false;

    public void OpenSavePortraitMenu()
    {
        fileNameInputField.interactable = true;
        sizeDropdown.interactable = true;
        saveButton.interactable = true;
        creatingPortraitOverlay.SetActive(false);
        finishedSavingPortraitMenu.SetActive(false);

        UIManager.OpenMenu(savePortraitMenus);

        gameObject.SetActive(true);
    }

    public void OpenSavedPortraitsFileLocation()
    {
        if (Directory.Exists(Directory.GetCurrentDirectory() + "/Saved Portraits"))
        {
            Debug.Log("Opened file explorer to 'saved portraits' folder");
            Application.OpenURL(Directory.GetCurrentDirectory() + "/Saved Portraits");
        }
        else
            Debug.LogWarning("Cannot open file explorer to 'saved portraits' folder because that folder does not exist");
    }

    public async void SavePortrait()
    {
        doneContactingServer = false;
        SavingPortrait = true;

        fileNameInputField.interactable = false;
        sizeDropdown.interactable = false;
        saveButton.interactable = false;

        creatingPortraitOverlay.SetActive(true);

        UpdateScores();

        switch (sizeDropdown.value)
        {
            case 0:
                size = PortraitSize.Sixteen;
                break;
            case 1:
                size = PortraitSize.Thirtytwo;
                break;
            case 2:
                size = PortraitSize.Fortyeight;
                break;
        }
        finalSprite = await ppMerger.CombinePortraitPieces(size);
        if(finalSprite != null)
        SavePortraitToFile();
        else
            UIManager.ForceCloseMenu();
    }

    async void SavePortraitToFile()
    {
        byte[] bytes = finalSprite.texture.EncodeToPNG();

        if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Saved Portraits"))
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Saved Portraits");

        if (fileNameInputField.text == "")
            fileNameInputField.text = "Unnamed Portrait";

        File.WriteAllBytes(Directory.GetCurrentDirectory() + "/Saved Portraits/" + fileNameInputField.text + ".png", bytes);

        if (PlayerAuthentication.LoggedIn)
        {
            await UniTask.WaitUntil(() => doneContactingServer == true);

            personalStatsText.text = "You've saved " + portraitsGeneratedPersonal + " portraits total.";
            globalStatsText.text = portraitsGeneratedGlobal + " portraits have been saved globally.";

            finishedSavingPortraitMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            personalStatsText.text = "";
            globalStatsText.text = "";

            finishedSavingPortraitMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        SavingPortrait = false;
    }

    async void UpdateScores()
    {
        if (PlayerAuthentication.LoggedIn)
        {
            List<Task> tasks = new()
            {
               UpdateGlobalScore(),
               UpdatePersonalScore()
            };

            await Task.WhenAll(tasks);

            doneContactingServer = true;
        }
    }

    async Task UpdateGlobalScore()
    {
        int score = 0;

        bool finished = false;

        bool succesful = true;
        LootLockerSDKManager.GetScoreList("11560", 1, 0, (response) =>
        {
            if (response.success)
            {
                score = response.items[0].score;
            }
            else
            {
                Debug.LogWarning("Failed to fetch leaderbaord data: " + response.Error);
                succesful = false;
            }

            finished = true;
        });

        await UniTask.WaitUntil(() => finished);

        if (!succesful) return;

        bool done = false;

        // Add to global score
        LootLockerSDKManager.SubmitScore("155", score + 1, "11560", (response) =>
        {
            if (response.success)
            {
                portraitsGeneratedGlobal = score + 1;
                //Debug.Log(score + " Global");
            }
            else
            {
                Debug.Log("Failed" + response.Error);
            }

            done = true;
        });

        await UniTask.WaitUntil(() => done);
    }

    async Task UpdatePersonalScore()
    {
        int score = 0;

        bool finished = false;

        bool succesful = true;
        LootLockerSDKManager.GetMemberRank("11561", PlayerPrefs.GetString("PlayerID"), (response) =>
        {
            if (response.success)
                score = response.score;
            else
            {
                Debug.LogWarning("Failed to fetch leaderbaord data: " + response.Error);
                succesful = false;
            }

            finished = true;
        });

        await UniTask.WaitUntil(() => finished);

        if (!succesful) return;

        bool done = false;

        LootLockerSDKManager.SubmitScore(PlayerPrefs.GetString("PlayerID"), score + 1, "11561", (response) =>
        {
            if (response.success)
            {
                portraitsGeneratedPersonal = score + 1;
                //Debug.Log(score + " Pesonal");
            }
            else
                Debug.LogWarning("Unable to upload personal score: " + response.Error);

            done = true;
        });

        await UniTask.WaitUntil(() => done);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyPuzzleGenerator : MonoBehaviour {

    public int randomSeedNumber;
    private int totalNumberEasyPuzzles = 168;
    private int totalNumberHardPuzzles = 25;
    private List<int> DailyPuzzle_easyLevels = new List<int>();
    private List<int> DailyPuzzle_hardLevels = new List<int>();
    private int checkDateCounter = 0;

    void Start() {
        GetDailyPuzzlesFromGameData();
        NewDayCheck();
    }

    private void GetDailyPuzzlesFromGameData() {
        DailyPuzzle_easyLevels = GameDataControl.gdControl.daily_easyLevel_Indexes;
        DailyPuzzle_hardLevels = GameDataControl.gdControl.daily_hardLevel_Indexes;
    }

    //User switches apps. When our app regains focus we run the day/time check incase its a different day
    void OnApplicationFocus(bool hasFocus) {
        if (hasFocus && checkDateCounter > 0) {
            Debug.LogWarning("Has Focus, Checking Daily Puzzle Date.");
            NewDayCheck();
        }
    }

    private void NewDayCheck() {
        GetSeedFromPrefs();
        StartCoroutine(CheckDate());
    }

    private void GetSeedFromPrefs() {
        randomSeedNumber = PlayerPrefs.GetInt("dailySeedNumber", 0);
    }

    private IEnumerator CheckDate() {
        Debug.Log("Getting The Date For Daily Puzzles.");
        yield return StartCoroutine(TimeManager.sharedInstance.getTime());
        int date = TimeManager.sharedInstance.getCurrentDateNow();

        //we are using the date as our randomSeedNumber if they dont match then its a new day
        if (randomSeedNumber != date) {
            Debug.Log("Get new DailyPuzzles with new seed number");
            randomSeedNumber = date;
            PlayerPrefs.SetInt("dailySeedNumber", randomSeedNumber);
            SelectRandomDailyPuzzles(randomSeedNumber);
            GetDailyPuzzlesFromGameData();
        }
        else {
            Debug.Log("Dailies From GameData");
        }
        checkDateCounter++;
    }

    private void SelectRandomDailyPuzzles(int seed) {
        int easyLevelsNeeded = DailyPuzzle_easyLevels.Count;
        int hardLevelsNeeded = DailyPuzzle_hardLevels.Count;
        DailyPuzzle_easyLevels.Clear();
        DailyPuzzle_hardLevels.Clear();

        //Set the randomSeedNumber so all devices randomize the same daily puzzles.
        UnityEngine.Random.InitState(seed);
        for (int i = 0; i < easyLevelsNeeded; i++) {
            int randomLevel = UnityEngine.Random.Range(0, totalNumberEasyPuzzles);

            //add this if so we dont get the same puzzle twice
            if (!DailyPuzzle_easyLevels.Contains(randomLevel)) {
                DailyPuzzle_easyLevels.Add(randomLevel);
            }
            else {
                easyLevelsNeeded++;
            }
        }

        for (int i = 0; i < hardLevelsNeeded; i++) {
            int randomLevel = UnityEngine.Random.Range(0, totalNumberEasyPuzzles);
            if (!DailyPuzzle_hardLevels.Contains(randomLevel)) {
                DailyPuzzle_hardLevels.Add(randomLevel);
            }
            else {
                hardLevelsNeeded++;
            }
        }

        ResetRandomSeed();
        SetGameDataWithNewPuzzles();
    }

    private void ResetRandomSeed() {
        UnityEngine.Random.InitState(System.Environment.TickCount);
    }

    private void SetGameDataWithNewPuzzles() {
        GameDataControl.gdControl.hardUnlocked = false;
        GameDataControl.gdControl.daily_level_results = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        GameDataControl.gdControl.daily_easyLevel_Indexes = DailyPuzzle_easyLevels;
        GameDataControl.gdControl.daily_hardLevel_Indexes = DailyPuzzle_hardLevels;
        GameDataControl.gdControl.SavePlayerData();
    }

)

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
        //List<int> { 1, 2, 3, 4, 5 }
        DailyPuzzle_easyLevels = GameDataController.gdControl.dailyPuzzle_easyLevels;
        //List<int> { 1, 2, 3, 4, 5 }
        DailyPuzzle_hardLevels = GameDataController.gdControl.dailyPuzzle_hardLevels;
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
        //Calls my TimeManager script retrieving the correct date
        //This can also be using unity built in DateTime.Now;
        yield return StartCoroutine(TimeManager.sharedInstance.getTime());
        //january 2, 1988 = 01021988 or MMddyyyy
        int date = TimeManager.sharedInstance.getCurrentDateNow(); 

        //The date is our randomSeedNumber, if these numbers dont match then it must be a new day
        if (randomSeedNumber != date) {
            Debug.Log("Get new DailyPuzzles with new seed number");
            randomSeedNumber = date;
            //Save our seed number for next time user opens app
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
        
        //Clearing our list of puzzles numbers so they are ready to randomize some new puzzle numbers
        DailyPuzzle_easyLevels.Clear();
        DailyPuzzle_hardLevels.Clear();

        //Set the randomSeedNumber so all devices randomize the same daily puzzles for each day.
        UnityEngine.Random.InitState(seed);
        for (int i = 0; i < easyLevelsNeeded; i++) {
            int randomLevel = UnityEngine.Random.Range(0, totalNumberEasyPuzzles);
            //So we dont get the same puzzle twice on one day
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
        //Now we have 2 new randomized lists
        ResetRandomSeed();
        SaveNewDailyPuzzleGameData();
    }

    private void ResetRandomSeed() {
        UnityEngine.Random.InitState(System.Environment.TickCount);
    }

    private void SaveNewDailyPuzzleGameData() {
        GameDataController.gdControl.hardUnlocked = false;
        GameDataController.gdControl.daily_level_results = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        GameDataController.gdControl.dailyPuzzle_easyLevels = DailyPuzzle_easyLevels;
        GameDataController.gdControl.dailyPuzzle_hardLevels = DailyPuzzle_hardLevels;
        GameDataController.gdControl.SavePlayerData();
    }

    //User switches apps. When our app regains focus we run the day/time check incase its a different day
    void OnApplicationFocus(bool hasFocus) {
        if (hasFocus && checkDateCounter > 0) {
            Debug.LogWarning("Has Focus, Checking Daily Puzzle Date.");
            NewDayCheck();
        }
    }
    
)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int difficulty; //ezt beállítottam 1-re, hogy a menüben ne lehessen meghalni, meg ne bugoljon
    public float timer;
    public bool startTimer;
    public int levelNumber;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null) //nem akarjuk, hogy minden egyes szinten egy újabb PlayerManager objektumot létrehozzon.
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void Start()
    {
        if (difficulty == 0)
        {
            difficulty = PlayerPrefs.GetInt("Difficulty");
        }
    }

    private void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
    }

    public void SaveDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void SaveBestTime()
    {
        startTimer = false;

        float lastTime = PlayerPrefs.GetFloat("Level" + levelNumber + "BestTime", 999);

        if(timer < lastTime)
            PlayerPrefs.SetFloat("Level" + levelNumber + "BestTime", timer);
        
        timer = 0;
    }

    public void SaveCollectedCoins()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoinsCollected");

        int newTotalCoins = totalCoins + PlayerManager.instance.coin;

        PlayerPrefs.SetInt("TotalCoinsCollected", newTotalCoins);
        PlayerPrefs.SetInt("Level" + levelNumber + "CoinsCollected", PlayerManager.instance.coin);

        PlayerManager.instance.coin = 0;
    }

    public void SaveLevel()
    {
        int nextLevelNumber = levelNumber + 1;
        PlayerPrefs.SetInt("Level" + nextLevelNumber + "Unlocked", 1);
    }
}

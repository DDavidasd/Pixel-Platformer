using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI currentCoinAmount;
    public Image HealthImage;
    public TextMeshProUGUI Health;
    [SerializeField] private TextMeshProUGUI endTimerText;
    [SerializeField] private TextMeshProUGUI endBestTimeText;
    [SerializeField] private TextMeshProUGUI endCoinsText;

    private bool gamePaused;

    [Header("Menu")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject pausedUI;
    [SerializeField] private GameObject endLevelUI;
    
    private void Start()
    {
        GameManager.instance.levelNumber = SceneManager.GetActiveScene().buildIndex; 
        PlayerManager.instance.playerUI = this; 
        Time.timeScale = 1;
        SwitchUI(inGameUI);
        Cursor.visible = false;
    }

    void Update()
    {
        UpdateInGameInfo();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CheckIfNotPaused();
        }

        if (GameManager.instance.difficulty == 1)
        {
            HealthImage.gameObject.SetActive(false);
            Health.gameObject.SetActive(false);
        }

        if (GameManager.instance.difficulty > 1)
        {
            HealthImage.gameObject.SetActive(true);
            Health.gameObject.SetActive(true);
            Health.text = "Health: " + PlayerManager.instance.health.ToString();
        }
    }

    private bool CheckIfNotPaused()
    {
        Cursor.visible = true;

        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0;
            SwitchUI(pausedUI);
            return true;
        }
        else
        {
            gamePaused = false;
            Time.timeScale = 1;
            SwitchUI(inGameUI);
            Cursor.visible = false;
            return false;
        }
    }

    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1;
        SwitchUI(inGameUI);
        Cursor.visible = false;
    }

    public void OnLevelFinished()
    {
        Cursor.visible = true;

        endCoinsText.text = "Coin: " + PlayerManager.instance.coin;
        endTimerText.text = "Your time: " + GameManager.instance.timer.ToString("00") + " s"; ;
        endBestTimeText.text = "Best time: " + PlayerPrefs.GetFloat("Level" + GameManager.instance.levelNumber + "BestTime", 999).ToString("00") + " s";

        SwitchUI(endLevelUI);
    }


    public void UpdateInGameInfo()
    {
        timerText.text = "Timer: " + GameManager.instance.timer.ToString("00") + " s";
        currentCoinAmount.text = PlayerManager.instance.coin.ToString();
    }

    public void PermaDeath()
    { 
        SwitchUI(pausedUI);
    }

    public void SwitchUI(GameObject UIMenu)
    {
        Cursor.visible = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        UIMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        ResetTimerAndCoin();
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        ResetTimerAndCoin();
    }
        
    public void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void ResetTimerAndCoin()
    {
        GameManager.instance.timer = 0;
        GameManager.instance.startTimer = false;
        
        PlayerManager.instance.coin = 0;
        int levelNumber = GameManager.instance.levelNumber;
        PlayerPrefs.SetInt("Level" + levelNumber + "TotalCoins", 0);
    }
}

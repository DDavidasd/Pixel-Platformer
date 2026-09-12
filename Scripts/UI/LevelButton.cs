using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI bestTime;

    public void UpdateTextInfo(int levelNumber)
    { 
        levelName.text = "Level " + levelNumber;
        bestTime.text = "Best time: " + PlayerPrefs.GetFloat("Level" + levelNumber + "BestTime", 999).ToString("00" + " sec");
    }
}

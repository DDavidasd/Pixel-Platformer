using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    void Start()
    {
        int levelNumber = GameManager.instance.levelNumber;
        //Debug.Log(levelNumber);
        int totalAmountCoins = PlayerPrefs.GetInt("Level" + levelNumber + "TotalCoins");

        //Érmék számának nullázása a pályák elején:
        if (totalAmountCoins != 0)
        {
            PlayerPrefs.SetInt("Level" + levelNumber + "TotalCoins", 0); //0: pálya indításakor az adott pályán a coin számláló 0-val fog kezdeni
        }
    }
}

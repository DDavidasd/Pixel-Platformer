using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private VolumeControllerUI[] volumeController;

    private void Start()
    {
        bool showButton = PlayerPrefs.GetInt("Level" + 2 + "Unlocked") == 1;
        continueButton.SetActive(showButton);

        for (int i = 0; i < volumeController.Length; i++) 
        { 
            volumeController[i].GetComponent<VolumeControllerUI>().SetUpVolumeSlider(); 
        } 
    }

    public void SwitchMenuTo(GameObject UIMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        { 
            transform.GetChild(i).gameObject.SetActive(false);
        }

        AudioManager.instance.PlaySFX(4);
        UIMenu.SetActive(true);
    }

    public void GameDifficulty(int i) => GameManager.instance.difficulty = i;

    public void QuitButton()
    { 
        Application.Quit();
    }

}

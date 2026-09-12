using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Diagnostics.SymbolStore;

public class SkinSelectionUI : MonoBehaviour
{
    [SerializeField] private bool[] skinPurchased;
    [SerializeField] private int[] skinPrice;
    private int skinID;

    [SerializeField] private Animator anim;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private TextMeshProUGUI bankText;

    /* private void Start()
     {
         PlayerPrefs.SetInt("TotalCoinsCollected", 500);
     }*/

    private void OnEnable()
    {
        Skin();
    }

    private void OnDisable()
    {
        equipButton.SetActive(false);
    }

    public void NextSkin()
    {
        AudioManager.instance.PlaySFX(4);

        skinID++;

        if (skinID > 2) //3. skin esetén skinID > 2
        {
            skinID = 0;
        }

        Skin();
    }

    public void PreviousSkin()
    {
        AudioManager.instance.PlaySFX(4);

        skinID--;

        if (skinID < 0)
        {
            skinID = 2; //3. skin esetén skinID = 2
        }

        Skin();
    }

    private void Skin()
    {
        skinPurchased[0] = true;

        for (int i = 1; i < skinPurchased.Length; i++)
        {
            bool skinUnlecked = PlayerPrefs.GetInt("SkinPurchased" + i) == 1;

            if (skinUnlecked)
            {
                skinPurchased[i] = true;
            }
        }    

        bankText.text = PlayerPrefs.GetInt("TotalCoinsCollected").ToString();

        equipButton.SetActive(skinPurchased[skinID]);
        buyButton.SetActive(!skinPurchased[skinID]);

        if (!skinPurchased[skinID])
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Price: " + skinPrice[skinID];

        anim.SetInteger("skinID", skinID);
    }

    public bool EnoughCoin()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoinsCollected");

        if (totalCoins >= skinPrice[skinID])
        {
            totalCoins = totalCoins - skinPrice[skinID];

            PlayerPrefs.SetInt("TotalCoinsCollected", totalCoins);

            AudioManager.instance.PlaySFX(5);
            return true;
        }

        AudioManager.instance.PlaySFX(6);
        return false;
    }


    public void Buy()
    {
        if (EnoughCoin())
        {
            PlayerPrefs.SetInt("SkinPurchased" + skinID, 1);
            Skin();
        }
        else
        {
            Debug.Log("Not enough coin");
        }
        
    }

    public void Equip()
    {
        PlayerManager.instance.chosenSkinID = skinID;
    }

    public void SwitchEquipButton(GameObject newButton)
    { 
        equipButton = newButton;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private PlayerUI playerUI; 

    private void Start()
    {
        playerUI = GameObject.Find("Canvas").GetComponent<PlayerUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            GetComponent<Animator>().SetTrigger("activate");

            Destroy(collision.gameObject);
            AudioManager.instance.PlaySFX(2);
         
            playerUI.OnLevelFinished();

            GameManager.instance.SaveBestTime();
            GameManager.instance.SaveCollectedCoins();
            GameManager.instance.SaveLevel();
        }
    }
}

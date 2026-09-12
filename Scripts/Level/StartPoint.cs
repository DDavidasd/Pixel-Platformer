using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void Awake()
    {
        PlayerManager.instance.respawnPoint = respawnPoint;
        PlayerManager.instance.PlayerRespawn();
    }

    private void Start() 
    { 
        AudioManager.instance.PlayRandomBGM(); 
    } 

    private void OnTriggerExit2D(Collider2D collision) //a Timer-hez szükséges az egész.
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (!GameManager.instance.startTimer)
            { 
                GameManager.instance.startTimer = true;
            }
        }
    }
}

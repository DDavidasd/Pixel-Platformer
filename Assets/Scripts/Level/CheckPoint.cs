using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !isActivated)
        {
            isActivated = true;
            GetComponent<Animator>().SetTrigger("activate");
            AudioManager.instance.PlaySFX(10);
            PlayerManager.instance.respawnPoint = transform;
        }
    }
}
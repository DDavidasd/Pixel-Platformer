using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public PlayerUI playerUI;

    public int coin;
    public int health = 3;

    public Transform respawnPoint;
    public GameObject currentPlayer;
    public int chosenSkinID;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject deathFx;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            PlayerDie();
        }
    }

    public void PlayerRespawn()
    {
        if(GameManager.instance.difficulty < 3)
            health = 3; //ha ez itt nem lenne, azaz nem állítanám minden respawnoláskor vissza 3-ra, akkor az elsõ 3 élet elvesztése után -1, -2, -3 stb lenne minden egyes halálnál, és csak 1 élete lenne igazából.

        if (GameManager.instance.difficulty == 3)
            health = 1;

        if (currentPlayer == null)
        {
            AudioManager.instance.PlaySFX(11);
            currentPlayer = Instantiate(playerPrefab, respawnPoint.position, transform.rotation);
        }
    }

    public void PlayerDie()
    {
        AudioManager.instance.PlaySFX(0);

        GameObject newDeathFx = Instantiate(deathFx, currentPlayer.transform.position, currentPlayer.transform.rotation);
        Destroy(newDeathFx, 0.4f);
        Destroy(currentPlayer);

        if (GameManager.instance.difficulty < 4)
        {
            Invoke("PlayerRespawn", 1);
        }
        else
        {
            //playerUI.PermaDeath();
        }
    }
}

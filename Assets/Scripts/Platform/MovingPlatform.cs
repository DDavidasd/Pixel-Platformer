using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed;
    [SerializeField] private float cooldown;

    private float cooldownTimer;
    private int movePointIndex;

    void Start()
    {
        transform.position = movePoint[0].position;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        bool isMoving = cooldownTimer < 0;

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            cooldownTimer = cooldown;
            movePointIndex++;

            if (movePointIndex >= movePoint.Length)
            {
                movePointIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        { 
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.transform.SetParent(null);
        }
    }
}

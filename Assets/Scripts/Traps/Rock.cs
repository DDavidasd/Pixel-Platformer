using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speedUp = 1.0f; // Sebesség felfelé
    [SerializeField] private float speedDown = 2.0f; // Sebesség lefelé
    [SerializeField] private float cooldownAtTop = 1.0f; // Várakozási idõ a felsõ pozícióban
    [SerializeField] private float cooldownAtBottom = 2.0f; // Várakozási idõ az alsó pozícióban

    private float cooldownTimer;
    private int movePointIndex;
    private bool movingDown;

    void Start()
    {
        transform.position = movePoint[0].position;
        // Kezdéskor meghatározzuk az elsõ irányt
        movingDown = movePoint.Length > 1 && movePoint[1].position.y < movePoint[0].position.y;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        bool isMoving = cooldownTimer < 0;
        if (isMoving)
        {
            //float currentSpeed = movingDown ? speedDown : speedUp;
            float currentSpeed;
            if (movingDown)
            {
                currentSpeed = speedDown;
            }
            else
            {
                currentSpeed = speedUp;
            }

            transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, currentSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
            {
                //cooldownTimer = movingDown ? cooldownAtBottom : cooldownAtTop;
                if (movingDown)
                {
                    cooldownTimer = cooldownAtBottom;
                }
                else
                {
                    cooldownTimer = cooldownAtTop;
                }

                movePointIndex++;

                if (movePointIndex >= movePoint.Length)
                {
                    movePointIndex = 0;
                }

                // Ellenõrizzük az új célpont irányát
                movingDown = movePoint[movePointIndex].position.y < transform.position.y;
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
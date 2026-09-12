using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Trap
{
    private Animator anim;
    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed;

    private int movePointIndex;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = movePoint[0].position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            //Debug.Log("Arrived");
            movePointIndex++;

            if (movePointIndex >= movePoint.Length)
            {
                movePointIndex = 0;
            }

            if (movePointIndex == 0)
                Flip();
            if (movePointIndex == 1)
                Flip();

        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, 1);
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    
    protected int facingDirection = 1;
    //[SerializeField] private float speed;

    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;

    protected bool wallDetected;
    protected bool groundDetected;

    [SerializeField] private GameObject deathFx;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
       // rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    public void Damage() 
    {
        anim.SetTrigger("Die");
    }

    public void DestroyMe()
    {
        if (deathFx != null)
        {
            GameObject newDeathFx = Instantiate(deathFx, transform.position, transform.rotation);
            Destroy(newDeathFx, 0.3f);
        }
        
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)  //Ez azért kell, hogy ha a fejére ugrok, akkor meghaljon, de a karakterem ne.
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            Player player = collision.collider.GetComponent<Player>();

            if (player.transform.position.x > transform.position.x)
            {
                player.Knockback(1);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                player.Knockback(-1);
            }
        }
    }

    protected virtual void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void CollisionChecks()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
    }
}

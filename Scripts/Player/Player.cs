using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Effects information")]
    [SerializeField] private ParticleSystem dustFx;
    [SerializeField] private ParticleSystem dashFx;
    [SerializeField] private ParticleSystem glideFx;

    [Header("Move information")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 36f;
    private float dirX;
    private float dirY;
    private bool canDoubleJump = true;
    private float doubleJumpForce = 30f;
    private float defaultJumpForce;
    private bool canBeControlled;

    [Header("Ground/Wall information")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;   //1.05f
    [SerializeField] private float wallCheckDistance;     //0.72f
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;      //0.4f
    private bool isGrounded;
    private bool isWallDetected;
    private bool isWallSliding;
    private bool canWallSlide;

    private bool facingRight = true; 
    private int facingDirection = 1; 

    [Header("Dash information")]
    [SerializeField] private float dashingVelocity = 70;  
    [SerializeField] private float dashingTime = 0.06f;   
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    private float dash_Cooldown = 1.0f;
    private float dash_CooldownCurrent = 0.0f;
    private bool dash_CooldownReady;

    [Header("Coyote Jump information")]
    [SerializeField] private float coyoteJumpTime = 0.1f;
    private float coyoteJumpCounter;
    private bool canHaveCoyoteJump;

    [Header("Gliding information")]
    [SerializeField] private float glidingSpeed;
    public bool isGlide = false; 

    [Header("Knockback information")]
    [SerializeField] private Vector2 knockbackDirection;
    private bool isKnocked;
    private bool canBeKnock = true;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackProtectionTime;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        LayerSkinAnim();

        defaultJumpForce = jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControllers();

        if (isKnocked)
        { 
            return;
        }

        if (Input.GetKey(KeyCode.V) && !isGrounded && !isWallSliding)
        {
            if (!glideFx.isPlaying)
                glideFx.Play();
        }
        else
        {
            if (glideFx.isPlaying)
                glideFx.Stop();
        }
           

        CollisionChecks();
        FlipController();
        Move();
        Dash();
        WallSlideCheck();
        Glide();
        EnemyCheck();

        coyoteJumpCounter -= Time.deltaTime;
    }

 

    private void AnimationControllers() //Animáció függvény
    {
        bool isMoving;

        if (rb.velocity.x != 0 && !isWallDetected)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
        anim.SetBool("isGlide", isGlide);
        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("canBeControlled", canBeControlled);
    }

    private void LayerSkinAnim() //Kiválasztott skinhez kell
    {
        int skinIndex = PlayerManager.instance.chosenSkinID;

        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(skinIndex, 1);
    }

    private void Move() //horizontális mozgás és ugrás
    {
        if (!canBeControlled)
            return;

        dirX = Input.GetAxisRaw("Horizontal"); //sima GetAxis-al csúszkál, míg a Raw-al igazi 2d-s mozgás lesz
        dirY = Input.GetAxisRaw("Vertical"); 

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (dirY < 0) //(Input.GetAxis esetén ez a gyorsulós téma van, ezért kell Raw, ameddig nyomom az "s"-t vagy lefele nyilat, csak addig csússzon
        {
            canWallSlide = false;
        }

        JumpCheck();
    }

    public void ReturnControll() //respawn közben ne lehessen mozogni
    { 
        canBeControlled = true;
    }

    private void JumpCheck() //dupla ugráshoz szükséges feltételek vizsgálata
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallSliding && isWallDetected) //isWallSliding azért kell, hogy ne lehessen felfutni a falon ugrásokkal
            {
                WallJump();
            }
            else if ((isGrounded || coyoteJumpCounter > 0) && !isWallDetected)
            {
                Jump();
            }
            else if (isGrounded && isWallDetected)
            {
                Jump();
            }
            else if (canDoubleJump && !isWallSliding && !isWallDetected) 
            {
                canDoubleJump = false;
                isGlide = false; //ezzel fog kijönni glide közben a glide animációból és ugrik egyet.
                jumpForce = doubleJumpForce;
                Jump();
                jumpForce = defaultJumpForce;
            }

            canWallSlide = false;
        }

        if (isGrounded || isWallDetected)
        {
            canDoubleJump = true; //csak 1x tudna doubleJumpolni, ha itt nem változtatnám meg true-ra.
            canHaveCoyoteJump = true;
        }
        if (!isGrounded && canHaveCoyoteJump)
        {
            canHaveCoyoteJump = false;
            coyoteJumpCounter = coyoteJumpTime;
        }
    }

    private void Jump() //Ugrás
    {
        AudioManager.instance.PlaySFX(3);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        if(isGrounded)
            dustFx.Play();
    }

    private void WallJump() //Falról ugrás
    {
        AudioManager.instance.PlaySFX(3);
        rb.velocity = new Vector2(13 * -facingDirection, 36);
    }

    private void Glide()
    {
        if (Input.GetKey(KeyCode.V) && rb.velocity.y < 0 && !isGrounded && !isWallDetected)  //&& !isWallDetected ez azt akadályozza meg, hogy ha falon csúszok, akkor ne azzal a sebességgel csússzon, mint amivel glideolni, tehát ha érzékel falat, akkor ne tudjon glideolni faloncsúszás animációval
        {
            isGlide = true;
            rb.velocity = new Vector2(rb.velocity.x * 0.7f, -glidingSpeed); //-glidingSpeed azért kell -1*, mert különben felfele repülne 
        }

        if (!Input.GetKey(KeyCode.V) || isGrounded || Input.GetKeyDown(KeyCode.LeftShift))
        {
            isGlide = false;
        }
    }

    private void FlipController() //Sprite forgatás feltételek
    {
        if (facingRight && rb.velocity.x < 0) //dirX helyett rb.velocity.x kell mert ha elugrik a faltól, akkor változzon meg a sprite iránya 
        {
            Flip();
        }
        else if (!facingRight && rb.velocity.x > 0) 
        {
            Flip();
        }
    }

    private void Flip() //Sprite forgatás
    {
        facingRight = !facingRight;
        facingDirection *= -1;

        Vector3 segedScale = transform.localScale;
        segedScale.x *= -1;
        transform.localScale = segedScale;
    }

    private void Dash() //pici emberke dash képessége
    {
        DashCD();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && canBeControlled) //canBeControlled: respawn közben ne tudjon dashelni
        {    
            dashFx.Play();
            AudioManager.instance.PlaySFX(12);
            isDashing = true;
            canDash = false;
            dash_CooldownCurrent = 0.0f;
            rb.gravityScale = 0; //ezáltal dashelés közben 0 lesz a gravitáció, így vízszintesen fog dashelni, majd ebbõl az ifbõl kilépve, vissza lesz állítva a gravity 11-re.

            anim.SetTrigger("dash");

            //dash direction:  
            if (isWallSliding && facingDirection == 1 && !isGrounded)
            {
                dashingDir = new Vector2(-1, 0f);
                Flip();
            }
            else if (isWallSliding && facingDirection == -1 && !isGrounded)
            {
                dashingDir = new Vector2(1, 0f);
                Flip();
            }
            else if (!isWallSliding && facingDirection == 1)
            {
                dashingDir = new Vector2(1, 0f);
            }
            else if (!isWallSliding && facingDirection == -1)
            {
                dashingDir = new Vector2(-1, 0f);
            }

            StartCoroutine(StopDashing()); //stop dash -> megálljon, ne fusson el a pici ember
        }

        rb.gravityScale = 11; //gravitáció visszaállítva, dash közben ez 0-ra változik
       
        Dashing();
    }

    private void DashCD() //!!!Dash Cooldown, hogy ne tudjon folyamatosan dashelni Groundon, meg levegõben ne csak 1x tudjon dashelni!!!
    {
        if (dash_CooldownCurrent >= dash_Cooldown)
        {
            dash_CooldownReady = true;
        }
        else
        {
            dash_CooldownCurrent = dash_CooldownCurrent + Time.deltaTime;
            dash_CooldownReady = false;
        }
    }

    private IEnumerator StopDashing() //ne repüljön el, meg kell állítani a kis gazfickót
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
    }

    private void Dashing() //Dash-hez szükséges feltételek vizsgálata
    {
        if (isDashing)
        {
            rb.velocity = dashingDir.normalized * dashingVelocity; //dashingVelocity ennél kerekítési hiba szokott lenni a float miatt, ezért kell inkább intet használni.     dashingDir.normalized
            //rb.velocity = new Vector2(dashingVelocity * facingDirection, 0);
            return;
        }
        if (dash_CooldownReady)//(isGrounded || isWallDetected) && Ez azért nem kell, mert akkor levegõben nem állítódna vissza true-ra.
        {
            canDash = true;
        }
    }

    private void WallSlideCheck() //Falon csúszás és feltételvizsgálat
    {
        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f); //0.1f-el 10%-os csúszás lesz.
        }

        if (!isWallDetected) //Ha ezt átrakom a CollisionChecks()-n belüli if-be, akkor nem fog mûködni
        {
            isWallSliding = false;
        }
    }

    private void EnemyCheck() //Ellenség fejére ugrás vizsgálata
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius);

        foreach (var enemy in hitColliders)
        {
            if (enemy.GetComponent<Enemy>() != null && rb.velocity.y < 0) // azért kell y tengelyen megvizsgálni, hogy felfele ugrásnál meg dashnél ne tudjam megölni az ellenséget, csak ha tényleg esek.
            {
                AudioManager.instance.PlaySFX(1);
                enemy.GetComponent<Enemy>().Damage();
                isGlide = false;
                Jump();
            }
        }
    }

    public void Knockback(int direction) //Player hátralökése
    {
        AudioManager.instance.PlaySFX(9);

        if (!canBeKnock)
            return;

        if (GameManager.instance.difficulty > 1)  //azért > 1 mert akkor nem kell a hard fokozatnál is vonni az életet, ezáltal 1 sort meg lehet spórolni.
        {
            PlayerManager.instance.health--;

            if (PlayerManager.instance.health < 1)
            {
                PlayerManager.instance.PlayerDie();
                
            }
        }

        isKnocked = true;
        canBeKnock = false;

        rb.velocity = new Vector2(knockbackDirection.x * direction, knockbackDirection.y);

        Invoke("CancelKnockback", knockbackTime);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }

    private void CancelKnockback()
    { 
        isKnocked = false;
    }

    private void AllowKnockback()
    {
        canBeKnock = true;
    }

    private void CollisionChecks() //Platformok érzékelése, ground/wall
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        if (isWallDetected && rb.velocity.y < 0)
        {
            canWallSlide = true;
        }

        if (!isWallDetected) //fal érzékelés után nem tudott ugrani, ez azt javítja ki
        {
            canWallSlide = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance)); 
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
    }
}
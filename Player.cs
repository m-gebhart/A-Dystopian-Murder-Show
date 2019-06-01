/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    /* This class contains all abilities both characters share for spawning, jumping, moving, dying, scoring, etc. */
    public Sprite Sprite;
    public Animator animator;

    public float speed, jumpHeight, jumpMomentum;
    public float dragInAir;
    protected float originDrag;
    public float gravityInAir;
    protected float originGravity;

    protected float mindedJumpTime = 0.2f; //minded Jump: if still in air for x remaining time and Jump-Button pressed, an auto-jump is executed when 'grounded' 
    protected bool mindedJump = false; //note: coyoteTime was also considered, but refused due to Design decisions 

    public bool isJumping = false;   //whether character is grounded or not
    public Laurel LaurelScript;
    protected bool horizontalAirMove = false;
    protected bool isAlive = true;
    public bool facingRight;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb2d;
    protected BoxCollider2D bc2d;
    protected GameObject manager;
    protected Playermanager playermanager;


    //spawning and setting up components, called at the beginning of the scene
    protected void setComponents()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        setUpAudio();
        manager = GameObject.Find("/Overlay/UIManager");
        playermanager = GameObject.Find("/Playermanager").GetComponent<Playermanager>();
        originDrag = rb2d.drag;
        facingRight = true;
        originGravity = rb2d.gravityScale;
        InputManager.playerAlive = true;
    }

    protected void checkInput()
    {
        if (InputManager.MoveRightInput()) //moveRight
        {
            if (!facingRight)
            {
                setRight();
            }
            walk(true);
        }
        else if (InputManager.MoveLeftInput()) //moveLeft
        {
            if (facingRight)
            {
                setLeft();
            }
            walk(false);
        }
        //jump momentum after letting D or A go; so if Input isn't given any more ~ similar to GetKeyUp 
        else if (isJumping)
        {
            if (facingRight && horizontalAirMove) rb2d.velocity = new Vector2(jumpMomentum, rb2d.velocity.y);
            else if (!facingRight && horizontalAirMove) rb2d.velocity = new Vector2(-jumpMomentum, rb2d.velocity.y);
        }
        else if (rb2d.velocity.y == 0 && !isJumping)
            GroundContact();
    }

    //following components just for footstep sounds
    protected AudioSource audioSource;
    public AudioClip footstep1, footstep2, footstep3, footstep4, footstep5;
    private AudioClip[] footsteps;
    public bool audioStepPlaying = false;
    public float stepVolume;
    public float stepAudioDuration;

    void setUpAudio()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        footsteps = new AudioClip[5];
        footsteps[0] = footstep1; footsteps[1] = footstep2; footsteps[2] = footstep3; footsteps[3] = footstep4; footsteps[4] = footstep5;
    }

    public void walk(bool rightwards)
    {
        int negator = 1;
        if (!rightwards) negator = -1;
        if (rb2d.velocity.x != 0)
            animator.SetBool("isWalking", true);
        horizontalAirMove = true;
        rb2d.velocity = new Vector2(negator * speed, rb2d.velocity.y);

        if (!audioStepPlaying && isAlive && !isJumping) {
            audioStepPlaying = true;
            audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length-1)], stepVolume * Volumes.effectsVol);
            StartCoroutine(waitUntilSoundHasEnded());
        }
    }

    //so that all the steos are not played at the same time
    IEnumerator waitUntilSoundHasEnded()
    {
        yield return new WaitForSeconds(stepAudioDuration);
        audioStepPlaying = false;
    }

    //setting rotation to leftwards
    public void setLeft()
    {
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        facingRight = false;
    }

    //setting rotation to rightwards
    public void setRight()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        facingRight = true;
    }

    //jumpCooldown instantiated right after jump, in case if shapeshifting happens shortly after (within jumpCoolDownTime)
    public bool jumpCooldown = false;
    public float jumpCooldownTime = 0.15f;

    public void jump()
    {
        animator.SetBool("isJumping", true);
        animator.SetBool("midAir", true);
        jumpCooldown = true;
        rb2d.velocity = Vector2.up * jumpHeight;
        isJumping = true;
        inAir();
        StartCoroutine(waitForJumpCooldown());
    }

    IEnumerator waitForJumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldownTime);
        jumpCooldown = false;
    }

    //if walking from a platform, called by platform's OnCollisionExit2D
    protected void setFall()
    {
        isJumping = true;
        Laurel.doubleJump = false;
        animator.SetBool("midAir", true);
        inAir();
    }

    //grounded
    protected void GroundContact()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("midAir", false);
        if (this.name == "PlayerLaurel")
        {
            animator.SetBool("isDoubleJumping", false);
            animator.SetBool("isFalling", false);
        }
        isJumping = false;
        horizontalAirMove = false;
        rb2d.drag = originDrag;
        rb2d.gravityScale = originGravity;
        Laurel.doubleJump = false;
        playermanager.jumpCooldownPending = true;
        if (mindedJump)
            jump();
    }

    //bool for: if still in air for x remaining time and Jump-Button pressed, an auto-jump is executed when 'grounded' 
    protected IEnumerator setMindedJump()
    {
        mindedJump = true;
        yield return new WaitForSeconds(mindedJumpTime);
        mindedJump = false;
    }

    public AudioClip deathSound;
    public float deathVolume;

    protected void die(string enemyKilledBy)
    {
        if (isAlive) //preventing double execution
        {
            InputManager.playerAlive = false;
            isAlive = false;
            bc2d.enabled = false;
            rb2d.velocity = Vector3.zero; //freezing Position
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

            audioSource.PlayOneShot(deathSound, deathVolume * Volumes.effectsVol);

            //saving data for Game Over screen
            PlayerPrefs.SetString("killedBy", enemyKilledBy);
            PlayerPrefs.SetFloat("LevelTime", Time.timeSinceLevelLoad);
            PlayerPrefs.SetFloat("CurrentScore", Mathf.Floor(GameObject.Find("/Overlay/UIManager").GetComponent<UIManager>().score));
            PlayerPrefs.SetString("LastLevel", GameObject.Find("Exit").GetComponent<Exit>().levelName);
            PlayerPrefs.SetInt("LastLevelIndex", SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(deathAnim());
        }
    }

    public float deathAnimTime = 2f;

    IEnumerator deathAnim()
    {
        animator.SetBool("dead", true);
        animator.SetBool("stayDead", true);
        transform.position += new Vector3(0f, 0f, -0.2f);
        yield return new WaitForSeconds(0.001f); //prevent animation looping bug
        animator.SetBool("dead", false);

        yield return new WaitForSeconds(deathAnimTime);
        AudioSource start = GameObject.Find("MrManager").GetComponent<AudioSource>(); //Disable Music
        start.volume = 0;
        GameObject.Find("/MrManager/MusicLoop").SetActive(false);

        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }

    public void inAir() //lossing contact to ground/platform
    {
        if (isJumping)
        {
            rb2d.drag = dragInAir;
            rb2d.gravityScale = gravityInAir;
        }
    }

    //rewards (basic scores diferent for each enemy type)
    public float rewardLvl1, rewardLvl2, rewardLvl3, rewardLvl4, rewardLvl5;

    public float JumpMultiplier;
    public float AttackMultiplier;

    public void checkAttackReward(int lvl) //Hardy: Hammer, Laurel: Hat
    {
        checkReward(lvl, AttackMultiplier);
    }

    public void checkJumpReward(int lvl) //if jumping on top of one enemy's head
    {
        jump(); //auto-jump
        checkReward(lvl, JumpMultiplier);
    }

    //reward / score is sent to UIManager to update current score
    public void checkReward(int lvl, float multiplier)
    {
        switch (lvl)
        {
            case 11: manager.SendMessage("ChangeScore", rewardLvl1 * multiplier); break;
            case 12: manager.SendMessage("ChangeScore", rewardLvl2 * multiplier); break;
            case 13: manager.SendMessage("ChangeScore", rewardLvl3 * multiplier); break;
            case 14: manager.SendMessage("ChangeScore", rewardLvl4 * multiplier); break;
            case 15: manager.SendMessage("ChangeScore", rewardLvl5 * multiplier); break;
        }
    }
}
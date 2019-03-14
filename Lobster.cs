/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobster : MovingEnemy {
    /*this enemy type moves back and forth on a platform and attacks if the player is close to him*/

    public float attackingTime;
    public float reactionTime;
    private float originReactionTime;
    public float cooldown;
    private float originCooldown;
    private bool cooling;
    private bool isAttacking;
    private bool attackInstantiated;

    void Start()
    {
        SetLobsterComponents();
        SetManager();
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (growable && !fullyGrown) Grow();
        else if (movable && !isAttacking && !cooling) Move();
        if (isAttacking && !attackInstantiated)
        {
            reactionTime -= Time.deltaTime;
            if (reactionTime < 0f) {
                ExecuteAttack();
            }
        }
        if (cooling)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0f)
            {
                cooldown = originCooldown;
                isAttacking = false;
                cooling = false;
            }
        }
    }

    void LateUpdate()
    {
        if (respawnable) RespawnProcessing();
    }

    void SetLobsterComponents()
    {
        if (goingRight)
            SetRight();
        else if (!goingRight)
            SetLeft();
        SetComponents();
        originReactionTime = reactionTime;
        originCooldown = cooldown;
        attackInstantiated = false;
        cooling = false;
    }

    void SetManager()
    {
        enemyManager = GameObject.Find("/LobsterManager");
        this.transform.parent = enemyManager.transform;
    }

    public GameObject attackTrigger;

    void Attack() //called by AttackTrigger
    {
        isAttacking = true;
        animator.SetBool("isReacting", true);
    }

    void ExecuteAttack() //creates MeleeAttackTrigger
    {
        animator.SetBool("isReacting", false);
        animator.SetBool("isAttacking", true);
        MeleeAttack newAttack = Instantiate(Resources.Load("MeleeAttack", typeof(MeleeAttack))) as MeleeAttack;
        newAttack.friendly = false;
        newAttack.GetComponent<AudioSource>().volume *= Volumes.effectsVol;
        newAttack.attackingTime = this.attackingTime;
        newAttack.transform.parent = this.transform;
        newAttack.transform.position = attackTrigger.transform.position;
        newAttack.transform.localScale = attackTrigger.transform.localScale;
        attackInstantiated = true;
        cooling = true;
    }

    void SetMeleeAttack() //called if MeleeAttackTrigger is shut down
    {
        animator.SetBool("isAttacking", false);
        attackInstantiated = false;
        reactionTime = originReactionTime;
    }

    void OnCollisionEnter2D(Collision2D col) //polygoncollider = whole body = player dies if colliding with it
    {
        if (col.gameObject.layer == 10 && fullyGrown)
            col.gameObject.SendMessage("Die", this.name); //if player
    }

    void OnTriggerEnter2D(Collider2D col) //trigger located at enemy's head (bc2d) to make player jump
    {
        if (col.gameObject.layer == 10 && col.gameObject.GetComponent<Player>().isJumping) //if player is in air
        {
            //exception: if player is really on top (otherwise confusion with attackTrigger)
            if (col.transform.position.y > this.transform.position.y)
            {
                col.gameObject.SendMessage("CheckJumpReward", this.gameObject.layer);
                if (!Playermanager.isLaurel)
                {
                    Die(); //Hardy kills enemy while jumping on top of Lobster
                }
            }
        }
    }
}
 

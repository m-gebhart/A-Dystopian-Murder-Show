/* Michael Gebhart
 * Cologne Game Lab 
 * BA 1 - Ludic Game 2018/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardy : Player
{
    /* This class contains all abiltits only Laurel has, i.e. MeleeAttack*/
    public MeleeAttack meleeAttackPrefab;
    public float meleeAttackingTime, meleeCooldown;
    [HideInInspector]
    public float cooldownTimer = 0;

    void Start()
    {
        setComponents();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        checkInput();
        checkInputAbilitites();
    }

    void checkInputAbilitites()
    {
        //Input for Jumping
        if (InputManager.JumpInput())
        {
            if (rb2d.velocity.y == 0 && !isJumping) //normal Jump if grounded
                jump();
            else if (isJumping) //minded Jump (auto-jump if button pressed before touching ground)
                StartCoroutine(setMindedJump());
        }
        // Input for Melee Attack
        if ((InputManager.AttackInput()) && cooldownTimer <= 0)
            meleeAttack();
    }

    void meleeAttack()
    {
        cooldownTimer = meleeCooldown;
        animator.SetBool("isAttacking", true);

        //A trigger MeleeAttack gets instantiated, that tells every enemy touching it to die
        MeleeAttack newAttack = Instantiate(Resources.Load("MeleeAttack", typeof (MeleeAttack))) as MeleeAttack;
        newAttack.friendly = true;
        newAttack.attackingTime = meleeAttackingTime;
        newAttack.transform.parent = this.transform;
        if (facingRight)
            newAttack.transform.position = new Vector3(transform.position.x + (transform.localScale.x), transform.position.y, transform.position.z);
        else if (!facingRight)
                newAttack.transform.position = new Vector3(transform.position.x - (transform.localScale.x), transform.position.y, transform.position.z);
        newAttack.GetComponent<AudioSource>().volume *= Volumes.effectsVol;
        newAttack.GetComponent<AudioSource>().Play();
    }

    //called by Melee Attack trigger, when it's destroyed
    void setMeleeAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    //if Hardy receives the hat, Laurel will already have right after shapeshifting to him
    void setHatThrown()
    {
        playermanager.hatReceived = true;
    }
}
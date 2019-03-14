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
    public float meleeAttackingTime;
    public float meleeCooldown;
    [HideInInspector]
    public float cooldownTimer = 0;

    void Start()
    {
        SetComponents();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        CheckInput();
        CheckInputAbilitites();
    }

    void CheckInputAbilitites()
    {
        //Input for Jumping
        if (rb2d.velocity.y == 0 && !isJumping && InputManager.JumpInput())
        {
            animator.SetBool("midAir", true);
            animator.SetBool("isJumping", true);
            Jump();
        }
        // Ibput for Melee Attack
        if ((InputManager.AttackInput()) && cooldownTimer <= 0)
            MeleeAttack();
    }

    void MeleeAttack()
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
    void SetMeleeAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    //if Hardy receives the hat, Laurel will already have right after shapeshifting to him
    void SetHatThrown()
    {
        playermanager.hatReceived = true;
    }
}
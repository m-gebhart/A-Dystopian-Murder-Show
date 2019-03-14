/* Michael Gebhart
 * Cologne Game Lab 
 * BA 1 - Ludic Game 2018/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laurel : Player
{
    /* This class contains all abiltits only Laurel has, i.e. Ranged Attack*/
    public static bool doubleJump;
    public GameObject rangedAttackPrefab;
    public float attackOffset;
    private float originAttackOffset;

    //for checking if animations is passed
    public float throwAnimTime;
    private float originThrowAnimTime;
    public float catchAnimTime;
    private float originCatchAnimTime;

    //if ranged attack is executed, i.e. the hat is trown
    public bool hatThrown;
    private bool attackProcessing;
    private GameObject hatAttack;

    void Start()
    {
        SetLaurelComponents();
        doubleJump = false;
    }

    void Update()
    {
        CheckInput();
        CheckInputAbilitites();
    }

    void SetLaurelComponents()
    {
        SetComponents();
        originThrowAnimTime = throwAnimTime;
        originCatchAnimTime = catchAnimTime;
        originAttackOffset = attackOffset;
    }

    void CheckInputAbilitites()
    {
        if (InputManager.JumpInput())
        {
            if (rb2d.velocity.y == 0 && !isJumping && !doubleJump) //Jump
            {
                animator.SetBool("isJumping", true);
                Jump();
                animator.SetBool("midAir", true);
            }

            else if (isJumping && !doubleJump) //DoubleJump
            {
                animator.SetBool("isDoubleJumping", true);
                Jump();
                doubleJump = true;
            }
        }
        if (isJumping && rb2d.velocity.y < 0f) //is falling
            animator.SetBool("isFalling", true);
        if (InputManager.AttackInput() && !hatThrown && !animator.GetBool("isCatching") && !animator.GetBool("isAttacking")) //Attack
        {
            attackProcessing = true;
            CheckRangedAttack();
            animator.SetBool("isAttacking", true);
            animator.SetBool("isThrowing", true);
        }
        CheckRangedAttack();
    }

    void CheckRangedAttack()
    {
        if (attackProcessing)
        {
            attackOffset -= Time.deltaTime;
            if (attackOffset < 0f)
            {
                //spawning Hat close by Laurel
                GameObject hatAttack = GameObject.Instantiate(rangedAttackPrefab);
                float xHatSpawnPos = 0f;
                if (facingRight)
                    xHatSpawnPos = 0.2f;
                else if (!facingRight)
                    xHatSpawnPos = -0.2f;
                hatAttack.transform.position = this.transform.position + new Vector3(xHatSpawnPos, 0.25f, 0f);
                hatThrown = true;
                attackProcessing = false;
            }
        }

        if (animator.GetBool("isAttacking") && animator.GetBool("isThrowing"))  //throwing hat animation
        {
            throwAnimTime -= Time.deltaTime;
            if (throwAnimTime < 0f)
            {
                animator.SetBool("isThrowing", false);
                throwAnimTime = originThrowAnimTime;
            }
        }
        else if (animator.GetBool("isCatching"))    //catching head animation
        {
            catchAnimTime -= Time.deltaTime;
            if (catchAnimTime < 0f)
            {
                animator.SetBool("isCatching", false);
                animator.SetBool("isAttacking", false);
                catchAnimTime = originCatchAnimTime;
            }
        }
    }

    //called by Laurel's hat, if player receives it
    void SetHatThrown()
    {
        animator.SetBool("isCatching", true);
        SetAttackEnd();
    }

    public void SetAttackEnd()
    {
        attackOffset = originAttackOffset;
        hatThrown = false; //Attack has officially ended
    }
}
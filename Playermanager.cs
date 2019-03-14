/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermanager: MonoBehaviour
{
    /*This class is mainly for the shapeshifting mechanic for Laurel / Hardy
     * Here, the input for the shapeshifting will be called and the information between the two characters are shared
     * such as alignments in rotation, 
     */
    private GameObject Laurel;
    private GameObject Hardy;
    static public bool isLaurel;
    private Laurel LaurelScript;
    private Hardy HardyScript;

    void Start()
    {
        Laurel = GameObject.Find("PlayerLaurel");
        Hardy = GameObject.Find("PlayerHardy");
        LaurelScript = GameObject.Find("PlayerLaurel").GetComponent<Laurel>();
        HardyScript = GameObject.Find("PlayerHardy").GetComponent<Hardy>();

        //scene should begin with the same character the last scene ended with
        if (PlayerPrefs.GetString("LastCharacter") == "H")
        {
            //spawning Hardy
            isLaurel = false;
            SetCharacter(Laurel, Hardy, true);
        }
        else
        {
            //spawning Laurel
            isLaurel = true;
            SetCharacter(Hardy, Laurel, true);
        }
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        //Shapeshifting
        if (InputManager.ShapeshiftInput())
        {
            ShapeShift();
        }

        //respawning should be already possible during the player's death animation, without needing to wait for the Game Over screen
        if (InputManager.RespawnInput())
        {
            GameOver.Respawn();
        }
    }

    void ShapeShift()
    {
        if (isLaurel) //if he's Laurel, turning into Hardy
        {
            SetCharacter(Laurel, Hardy, false);
        }
        else //if he's Hardy, turning into Laurel
        {
            SetCharacter(Hardy, Laurel, false);
        }
    }

    public bool hatReceived;
    public bool jumpCooldownPending = true;

    //cloud appearing during transformation / shapeshifting
    private GameObject transformation;
    private float transformAnimTime = 0.3f;

    //here, the information transfer happens
    void SetCharacter(GameObject lastChar, GameObject nextChar, bool startOfGame)
    {
        //saving position of switched-from character
        float xPos = lastChar.transform.position.x;
        float yPos = lastChar.transform.position.y;

        //instantiating transformation cloud animation
        GameObject cloud = Instantiate(Resources.Load("Transformation", typeof(GameObject))) as GameObject;
        if (!startOfGame)
        {
            cloud.transform.position = new Vector3(xPos, yPos, -0.2f);
        }
        else
        {
            cloud.GetComponent<SpriteRenderer>().enabled = false;
            cloud.GetComponent<AudioSource>().enabled = false;
        }

        //saving scripts of the switched-to (nextCharScript) and the switched from character (lastCharScript)
        nextChar.SetActive(true);
        Player lastCharScript = lastChar.GetComponent<Player>();
        Player nextCharScript = nextChar.GetComponent<Player>();

        //applying directions / rotations
        if (lastCharScript.facingRight && !nextChar.GetComponent<Player>().facingRight)
            nextCharScript.SetRight();
        else if (!lastCharScript.facingRight && nextChar.GetComponent<Player>().facingRight)
            nextCharScript.SetLeft();

        //applying animation
        Animator lastCharAnim = lastChar.GetComponent<Animator>();
        Animator nextCharAnim = nextChar.GetComponent<Animator>();
        if (lastCharAnim.GetBool("midAir"))
        {
            nextCharAnim.SetBool("shapeshift", true);
            nextCharAnim.SetBool("midAir", true);
        }

        //applying whether Hardy has picked up Laurel's hat or not
        if (nextChar.name.Contains("Laurel"))
        {
            if (hatReceived)
            {
                nextChar.GetComponent<Laurel>().SetAttackEnd();
                hatReceived = false;
            }
            else if (nextChar.GetComponent<Laurel>().hatThrown)
                nextCharAnim.SetBool("isAttacking", true); ;
        }

        //applying little adjustments in the Sprite's sizes
        if (nextChar == Laurel)
            nextChar.transform.position = new Vector2(xPos, yPos - 0.122f); //difference in characters' sizes
        else
            nextChar.transform.position = new Vector2(xPos, yPos);
        nextChar.transform.parent = this.transform;

        //applying whether the character is in air AND shapeshifted in the moment before (within the last 0.15s)
        //if shapeshifting didn't happen in that time: set to same velocity
        if (!lastCharScript.jumpCooldown)
            nextChar.GetComponent<Rigidbody2D>().velocity = lastChar.GetComponent<Rigidbody2D>().velocity;

        //if shapeshifting happened in that time: auto-jump()
        else if (lastCharScript.jumpCooldown && this.jumpCooldownPending)
        {
            jumpCooldownPending = false; //prevent bug of infinite Jumps after pressing Shapeshift-jumps repeatedly, set to true if grounded again
            nextCharScript.Jump(); //if shapeshifting shortly after a jump: jump is yet executed
        }

        lastChar.SetActive(false);

        //saving who the switched-to character is
        if (lastChar == Laurel) isLaurel = false;
        else isLaurel = true;

        if (lastCharScript.isJumping)
        {
            nextCharScript.isJumping = true;
            nextCharScript.InAir();
        }

        //setting audio data back, to prevent continous footstep sound of last character
        nextCharScript.audioStepPlaying = false;

        //making transformation cloud disappear
        StartCoroutine(SetBackAnim(nextCharAnim, cloud));
    }

    IEnumerator SetBackAnim(Animator nextCharAnim, GameObject cloud)
    {
        yield return new WaitForSeconds(0.001f); //prevent bug to get stuck in shapeshifting anim
        nextCharAnim.SetBool("shapeshift", false);
        yield return new WaitForSeconds(transformAnimTime);
        Destroy(cloud);
    }
}
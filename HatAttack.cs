/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatAttack : MonoBehaviour {

    /* this class is the projectile / instantiated ranged attack of Laurel */
    public float speed;
    private float originSpeed;
    public float midAirStopTime; //how long should the hat spin without moving
    private bool midAirStop;
    private bool stopPassed;
    public float resistance; //air resistance
    public float bounceback; //if wall is hit
    public float momentumPower; //if player moves while attacking, more momentum is given to the hat
    private bool goingRight; //first direction of hat
    private bool flyingBack;

    private GameObject laurel;
    private Laurel LaurelScript;
    private Animator laurelAnim;
    private int negation;

    void Start()
    {
        SetComponents();
    }

    void Update()
    {
         MakeItFly();
    }

    //during spawn
    void SetComponents()
    {
        laurel = GameObject.Find("PlayerLaurel");
        LaurelScript = GameObject.Find("PlayerLaurel").GetComponent<Laurel>();
        laurelAnim = GameObject.Find("PlayerLaurel").GetComponent<Animator>();
        if (!LaurelScript.facingRight)
            negation = -1; //for leftwards transform-based movement
        else if (LaurelScript.facingRight)
        {
            negation = 1;
            bounceback = -bounceback;
        }
        flyingBack = false;
        midAirStop = false;
        stopPassed = false;
        momentumPower /= 10f;
        originSpeed = speed;
        this.gameObject.GetComponent<AudioSource>().volume *= Volumes.effectsVol;
    }

    GameObject player;

    void MakeItFly()
    {
        if (!flyingBack)
        {
            if (speed >= 0.01f)
            {
                speed -= Time.deltaTime / resistance;
                transform.position += new Vector3(negation*speed * Time.deltaTime, 0f, 0f); } //flying rightwards if negation = 1, otherwise leftwards (=-1)
            else
            {
                flyingBack = true;
                midAirStop = true;
            }
        }
        else if (flyingBack && midAirStop && !stopPassed) //mid air stop
        {
            midAirStopTime -= Time.deltaTime;
            if (midAirStopTime < 0f)
            {
                stopPassed = true;
                midAirStop = false;
            }
        }
        else if (flyingBack && stopPassed) //flying back to player
        {
            if (speed < originSpeed)
                speed += Time.deltaTime / resistance;
            if (laurel.activeInHierarchy)
                player = laurel;
            else if (!laurel.activeInHierarchy)
                player = GameObject.Find("PlayerHardy");
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position + new Vector3(0, 0.25f), speed * Time.deltaTime); //Aims for Laurel's hand
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //if the player (layer = 10) moves with the hat while still attacking, more momentum is given to the hat
        if (col.gameObject.layer == 10 && col.attachedRigidbody.velocity.x != 0f && !flyingBack)
            speed += momentumPower/resistance;
        
        //bounceback at the wall (layer = 8, 9)
        if ((col.gameObject.layer == 8 || col.gameObject.layer == 9) && !flyingBack)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(bounceback, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            flyingBack = true;
            midAirStop = true;
        }
        if (10 < col.gameObject.layer && col.gameObject.layer < 17) //enemies (layer: 10 < x 17) are killed if hit
        {
            LaurelScript.CheckAttackReward(col.gameObject.layer);
            col.gameObject.SendMessage("Die"); //enemy gets killed
        }
        else if (col.gameObject.layer == 10 && (flyingBack || midAirStop || !laurelAnim.GetBool("isThrowing")))
        {
            col.gameObject.SendMessage("SetHatThrown"); //player receiving hat back
            Destroy(this.gameObject);
        }
    }
}
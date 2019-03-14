/* Yannick Renz
 * Cologne Game Lab 
 * BA 1 - Ludic Game 2018/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    private GameObject laurel = null;
    private GameObject hardy = null;
    private GameObject manager;
    public float speed; //actually more like acceleration, this is the amount by which the velocity is changed every frame.
    public float speedLimit; //maximum velocity this object can reach; if its too low (e.g. 100), bouncing doesn't work properly.

    private float stunned;

    void Start()
    {
        SetBatComponents();
    }

    void Update()
    {
        if (respawnable)
            RespawnProcessing();
        if (growable && !fullyGrown)
            Grow();
        else if(stunned > 0)
            stunned -= Time.deltaTime;
        else
            SetDirection();

    }

    void SetBatComponents()
    {
        SetComponents();
        laurel = GameObject.Find("PlayerLaurel");
        hardy = GameObject.Find("PlayerHardy");
        enemyManager = GameObject.Find("/BatManager");
        speed /= 100;
        speedLimit /= 100;
        stunned = 0;
    }

    void SetDirection()
    {
        float xPlayer, yPlayer;
        float x = transform.position.x;
        float y = transform.position.y;

        Vector2 direction = new Vector2(x, y); //Vector of bat's location
        if (Playermanager.isLaurel)
        {
            if(laurel == null) laurel = GameObject.Find("PlayerLaurel"); //in case it wasnt detected on startup
            xPlayer = laurel.transform.position.x;
            yPlayer = laurel.transform.position.y;
        }
        else
        {
            if (hardy == null) hardy = GameObject.Find("PlayerHardy"); //in case it wasnt detected on startup
            xPlayer = hardy.transform.position.x;
            yPlayer = hardy.transform.position.y;
        }

        direction = new Vector2(xPlayer, yPlayer) - direction; //Vector points from bat to player
        direction = direction.normalized * speed; //Normalizes the vector length to 1 so that distance to player does not matter

        rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity + direction, speedLimit); //Final velocity vector length never exceeds speedLimit
    }

    protected void OnCollisionEnter2D(Collision2D col) //polygoncollider = whole body = player dies if colliding with it
    {
        if (col.gameObject.layer == 10 && fullyGrown)
            col.gameObject.SendMessage("Die", this.name); //if player (10)

        if (col.gameObject.layer == 8) { //contact with ground
            if (stunned > 0)
                rb2d.velocity = Vector2.zero;
            else
                stunned = 0.5f;
        }
    }

    protected void OnTriggerEnter2D(Collider2D col) //trigger located at enemy's head (bc2d)
    {
        if (col.gameObject.layer == 10 && col.gameObject.GetComponent<Player>().isJumping)  //if player is in air
        {
            if (!Playermanager.isLaurel)
                Die(); //Hardy kills enemy while jumping on top of Bat
            col.gameObject.SendMessage("CheckJumpReward", this.gameObject.layer);
        }
    }
}
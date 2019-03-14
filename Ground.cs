/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool deadly; //whether this object should kill the player instantly or not

    public bool movableX; //moving horizontally (X)
    public bool movableY; //or vertically (y)
    public bool rightwards; //the first direction the object should move to
    public bool upwards;
    public float speed;         //speed of movement
    public float xLimit;        //local limit of possible movement 
    public float yLimit;

    private float leftLimit;    //internal determinations of borders
    private float rightLimit;
    private float upperLimit;
    private float lowerLimit;

    private Rigidbody2D rb2d;
    private EdgeCollider2D ec2d;
    private GameObject Playermanager;

    void Start()
    {
        SetComponents();
        SetLimits();
        if (!deadly) CreateEdges();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (movableX)
        {
            if (rightwards && transform.position.x < rightLimit) //movement to the right
            {
                rb2d.velocity = Vector2.right * speed;
            }
            else if (!rightwards && transform.position.x > leftLimit) //movement to the left
            {
                rb2d.velocity = Vector2.left * speed;
            }

            if ((!rightwards && transform.position.x < leftLimit) || (rightwards && transform.position.x > rightLimit))
            {
                rightwards = !rightwards; //changing the direction of movement
            }
        }
        if (movableY)
        {
            if (upwards && transform.position.y < upperLimit) //movement to the right
            {
                rb2d.velocity = Vector2.up * speed;
            }
            else if (!upwards && transform.position.y > lowerLimit) //movement to the left
            {
                rb2d.velocity = Vector2.down * speed;
            }

            if ((!upwards && transform.position.y < lowerLimit) || (upwards && transform.position.y > upperLimit))
            {
                upwards = !upwards; //changing the direction of movement
            }
        }
    }

    void SetComponents()
    {
        if (movableX || movableY)
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
        Playermanager = GameObject.Find("Playermanager");
    }

    void SetLimits()
    {
        if (movableX)
        {
            if (rightwards) //following: setting limits in relation to horizontal movement
            {
                leftLimit = transform.position.x;
                rightLimit = xLimit + transform.position.x;

            }
            else if (!rightwards)
            {
                leftLimit = transform.position.x - xLimit;
                rightLimit = transform.position.x;
            }
        }
        if (movableY)
        {
            if (upwards) //following: setting limits in relation to vertical movement
            {
                lowerLimit = transform.position.y;
                upperLimit = yLimit + transform.position.y;

            }
            else if (!upwards)
            {
                lowerLimit = transform.position.y - yLimit;
                upperLimit = transform.position.y;
            }
        }
    }

    protected GameObject edgePrefab;


    //here, triggers at the end of every edge are created, that tell back-and-forth moving enemies to turn around
    void CreateEdges()
    {
        GameObject leftEdge = (GameObject)Instantiate(Resources.Load("EdgeTrigger"));
        leftEdge.transform.parent = this.transform;
        leftEdge.transform.position = this.transform.position;
        leftEdge.transform.localPosition += new Vector3(-9.7f, 10f, 0f); //position at the left end of the platform

        GameObject rightEdge = (GameObject)Instantiate(Resources.Load("EdgeTrigger"));
        rightEdge.transform.parent = this.transform;
        rightEdge.transform.position = this.transform.position;
        rightEdge.transform.localPosition = new Vector3(9.7f, 10f, 0f); //position at the right end of the platform
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (movableX || movableY) col.transform.parent = this.transform; //if platform are movable, the object on it should move with it
        if (col.gameObject.name.StartsWith("Player")) //if deadly / Spikes = player dies
        {
            if (deadly) col.gameObject.SendMessage("Die", this.name);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == 10) //if player
        {
            col.transform.parent = Playermanager.transform;
            if (col.rigidbody.velocity.y < 0f && col.transform.position.y > this.transform.position.y)
                col.gameObject.SendMessage("SetFall"); //if he's falling from the platform
        }
        else if (col.transform.parent = this.transform) col.transform.parent = null;
    }

    void OnTriggerStay2D(Collider2D col) //EdgeCollider2D on the surface, telling the player to be grounded
    {
        if (col.gameObject.layer == 10 && col.attachedRigidbody.velocity.y == 0) col.gameObject.SendMessage("GroundContact");
    }
}
/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /* This class contains all abilities (spawn, respawn, grow, die) following enemies share: Cannon, Banana, Lobster*/

        //for respawning
    public bool respawnable;
    public float respawnTime;
    protected float respawnTimer;
    protected float[] spawnPos;
    protected bool isAlive;

    //for growth anim at the start of spawn
    public bool growable; //for the 'sprite getting bigger'-animation
    public float growthSpeed;
    private Vector2 originScale;
    protected bool fullyGrown;

    protected GameObject enemyManager;
    public GameObject enemySpawner;
    public bool spawner; //if instatiated by Enemy Spawner

    protected SpriteRenderer spriteRenderer;
    protected PolygonCollider2D pc2d; //the whole body: player dies if colliding with pc2d
    protected BoxCollider2D bc2d; //located at head (for Jump/-Kills)
    protected Rigidbody2D rb2d;

    protected AudioSource audioSource;
    protected Animator animator;
    public AudioClip death;
    public float deathVolume;

    //spawning, setting components
    protected void SetComponents()
    {
        isAlive = true;
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume *= Volumes.effectsVol;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        if (growable)
        {
            growthSpeed /= 100;
            originScale = transform.localScale;
            transform.localScale = Vector2.zero;
            fullyGrown = false;
            if (gameObject.layer == 11 || gameObject.layer == 13 || gameObject.layer == 15)
                (Instantiate(Resources.Load("Smoke", typeof(GameObject))) as GameObject).transform.position = this.transform.position - new Vector3(0f, 0f, 0.4f);
        }
        else if (!growable)
        {
            fullyGrown = true;
        }
        if (respawnable)
        {
            pc2d = gameObject.GetComponent<PolygonCollider2D>();
            bc2d = gameObject.GetComponent<BoxCollider2D>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            SetSpawnPos();
        }
    }

    protected void RespawnProcessing() // counting time until next respawn, vita for cannons
    {
        if (!isAlive)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                SetRespawn();
            }
        }
    }

    protected void SetRespawn() //Respawning
    {
        this.transform.position = new Vector2(spawnPos[0], spawnPos[1]);
        spriteRenderer.enabled = true;
        pc2d.enabled = true;
        if (bc2d != null) bc2d.enabled = true;
        isAlive = true;
        if (growable)
        {
            fullyGrown = false;
            transform.localScale = Vector2.zero;
        }
    }

    protected void SetSpawnPos() //determining Spawn Position = original pos
    {
        spawnPos = new float[2];
        spawnPos[0] = transform.position.x;
        spawnPos[1] = transform.position.y;
    }

    protected void Grow() //during spawning: enemy gets bigger and bigger
    {
        if (transform.localScale.x < originScale.x && transform.localScale.y < originScale.y)
            transform.localScale += new Vector3(growthSpeed, growthSpeed, 0f); //here: actual growing
        else
        // if full size is reached
        {
            fullyGrown = true;
            audioSource.Play();
        }
    }

    protected void Die() //Death
    {
        if(gameObject.layer == 14 || gameObject.layer == 12)
            (Instantiate(Resources.Load("Oil", typeof(GameObject))) as GameObject).transform.position = this.transform.position + new Vector3(0f, 0f, 0.1f);
        else
            (Instantiate(Resources.Load("Blood", typeof(GameObject))) as GameObject).transform.position = this.transform.position + new Vector3(0f, 0f, 0.1f);

        GameObject.Find("MrManager").GetComponent<MrManager>().playDeathSound(death, deathVolume * Volumes.effectsVol);
        if (!attackedByPie) enemyManager.SendMessage("Killcount", this.name);
        if (spawner) enemySpawner.SendMessage("InstanceDied");
        if (respawnable)
        {
            pc2d.enabled = false;
            if (bc2d != null) bc2d.enabled = false;
            spriteRenderer.enabled = false; //GameObject becomes 'invisible'
            isAlive = false;
            respawnTimer = respawnTime;
        }
        else
            Destroy(this.gameObject);
    }

    public bool attackedByPie = false; //for bats to avoid scoring

    //no scoring, if attacked by pie
    protected void AttackOfPie()
    {
        attackedByPie = true;
    }
}
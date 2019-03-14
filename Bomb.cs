/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Enemy
{
    public float exploAnimTime;
    private float exploTriggerTime;
    private Vector3 scale;
    private bool exploded = false;

    void Start()
    {
        SetBombComponents();
    }

    void Update()
    {
        if (growable && !fullyGrown)
            Grow();
        else if (fullyGrown)
            Fall();
        if (animator.GetBool("isExploding"))
        {
            exploAnimTime -= Time.deltaTime;
            if (exploAnimTime < 0f)
                Destroy(this.gameObject);
        }
        if (respawnable)
            RespawnProcessing();
    }

    //during spawn
    void SetBombComponents()
    {
        scale = this.transform.localScale;
        SetComponents();
        enemyManager = GameObject.Find("/BombManager");
        this.transform.parent = enemyManager.transform;
        bc2d = gameObject.GetComponent<BoxCollider2D>();
        bc2d.enabled = false; //explosion trigger
        exploTriggerTime = exploAnimTime - 0.65f;
    }

    //falling determined by physics
    void Fall()
    {
        if (!animator.GetBool("isFalling"))
        {
            rb2d.constraints = RigidbodyConstraints2D.None;
            animator.SetBool("isFalling", true);
        }
        rb2d.AddForce(Physics.gravity);
    }

    //when exploding, the explosion activates the trigger (BoxCollider2D) as its shock wave
    void Explode()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.PlayOneShot(death, deathVolume * Volumes.effectsVol);
        
        transform.position += new Vector3(0f, 0.4f, 0f);
        rb2d.bodyType = RigidbodyType2D.Static;
        this.transform.localScale = scale;
        animator.SetBool("isExploding", true);
        enemySpawner.SendMessage("InstanceDied");
        bc2d.enabled = true; //trigger for explosion damage
        exploded = true;

        StartCoroutine(RunExploTrigger(exploTriggerTime));
    }

    private IEnumerator RunExploTrigger(float time)
    {
        yield return new WaitForSeconds(time);
        bc2d.enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!exploded && col.gameObject.layer == 17) //gets attacked by player before explosion
        {
            enemyManager.SendMessage("Killcount", this.name);
            Destroy(this.gameObject);
        }
        else if (!exploded)
            Explode(); //player touches any thing -> explosion
        if (exploded && col.gameObject.layer == 10) //player touches explosion
            col.gameObject.SendMessage("Die", this.name);
    }
}

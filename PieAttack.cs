/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieAttack : MonoBehaviour {

    /* this class is the projectile / instantiated attack of the Pie Cannons */

    public float speed; //will be overwritten in class Canon!

    private AudioSource audioSource;
    public Animator animator;
    public AudioClip death;
    public float deathVolume;

    private void Start()
    {
        if (speed < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);

        speed *= 10;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update () {
        Fly();
	}

    void Fly() //supposed that those pies fly in a straight line
    {
        transform.position += (new Vector3(speed / 10f, 0f, 0f)) * Time.deltaTime;
    }

    void Die()
    {
        GameObject.Find("MrManager").GetComponent<MrManager>().playDeathSound(death, deathVolume * Volumes.musicVol);
        (Instantiate(Resources.Load("Blood", typeof(GameObject))) as GameObject).transform.position = this.transform.position + new Vector3(0f, 0f, 0.1f);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer != 12) //if pie collides with anything but cannon
        {
            if (col.gameObject.layer == 15) //if pie collides with a Bat
            {
                col.gameObject.SendMessage("AttackOfPie");
                col.gameObject.SendMessage("Die");
            }
            else if (col.gameObject.layer == 10)
                col.gameObject.SendMessage("Die", this.name);
            Die();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackCollider
{
    public float speed;
    public Vector3 movementVector;


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void PrepareProjectile(int damage, float speed) 
    {
        SetDamage(damage);
        SetSpeed(speed);
    }

    public virtual void SetSpeed (float newSpeed) 
    {
        speed = newSpeed;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Environment") 
        {
            Destroy(this);
        }
    }*/

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "Player") 
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AttackCollider
{
    public float speed;
    public Vector3 movementVector;


    // Update is called once per frame
    void Update()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Environment") 
        {
            Destroy(this);
        }
    }
    private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy" || collision.gameObject.tag != "Player") 
        {
            Destroy(this);
        }
    }
}

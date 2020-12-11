using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject Projectile;
    private GameObject newProjectile;
    public float projectileSpeed;
    public float timeUntilAttack;
    public float timeRecoveryAttack;
    public float timeActiveAttack;
    float counter = 0f;

    public override void PrepareAttackBehaviour()
    {
        base.PrepareAttackBehaviour();
        SpeedStop();
        SwitchAttackState(attackState.Preparing);
        SwitchState(state.Attack);
    }

    public override void PreparingAttack()
    {
        base.PreparingAttack();
        counter += 1 * Time.deltaTime;
        if (counter >= timeUntilAttack)
        {
            counter = 0f;
            SwitchAttackState(attackState.Attack);
        }
    }

    public override void InAttack()
    {
        base.InAttack();
        SpawnProjectile(Projectile, enemyStats.attack.GetValue(), projectileSpeed);
        SwitchAttackState(attackState.Recovery);
        
    }

    public virtual void SpawnProjectile(GameObject projectile, int damage, float speed) 
    {
        newProjectile = Instantiate(projectile, this.transform.position, this.transform.rotation);
        newProjectile.GetComponent<Projectile>().PrepareProjectile(damage, speed);

    }

    public override void Recovery()
    {
        base.Recovery();
        counter += 1 * Time.deltaTime;
        if (counter >= timeRecoveryAttack)
        {
            counter = 0f;
            SetNormalSpeed();
            SwitchState(state.Idle);
        }
    }
    public override void Attack()
    {
        SpeedStop();
        transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
        base.Attack();
    }
}

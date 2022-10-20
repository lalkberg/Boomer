using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float stoppingDistance = 15;
    public float speed = 1;
    public float maxSpeed = 50;
    public float sightRange = 150;
    public LayerMask targets;
    public float attackCoolDown = 2.5f;

    public float damage = 2;
    public float knockbackForce = 1;

    Rigidbody rb;
    EnemyShoot enemyShoot;
    EEnemyState state = EEnemyState.Serching;
    Transform target;
    Vector3 randomMoveTarget = Vector3.zero;
    float antiStuckyWuckyTimeStamp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyShoot = GetComponent<EnemyShoot>();
        antiStuckyWuckyTimeStamp = Time.time + 5;
        randomMoveTarget = transform.position;
    }
    void Update()
    {
        switch (state)
        {
            case EEnemyState.Serching:
                RandomMoveAndLookForTarget();
                break;
            case EEnemyState.MovingTo:
                FollowTargetWithoutRotation();
                break;
            case EEnemyState.Fighting:
                Fighting();
                break;
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        if(target != null)
        {
            transform.LookAt(target);
        }
    }

    public void Damaged()
    {
        target = GameObject.FindWithTag("Player").transform;
        state = EEnemyState.MovingTo;
    }

    void RandomMoveAndLookForTarget()
    {
        Ray ray = new(transform.position, transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * sightRange, Color.green, 1f);
        if (Physics.Raycast(ray, out RaycastHit hit,sightRange, targets))
        {
            target = hit.transform;
            state = EEnemyState.MovingTo;
        }
        if (Vector3.Distance(transform.position,randomMoveTarget) < 2 || Time.time > antiStuckyWuckyTimeStamp)
        {
            randomMoveTarget = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            antiStuckyWuckyTimeStamp = Time.time + 5;
            rb.velocity = Vector3.zero;
        }
        else
        {
            transform.LookAt(randomMoveTarget);
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
        } 

    }
    void FollowTargetWithoutRotation()
    {
        if (Vector3.Distance(transform.position, target.position) > (enemyShoot.currentWeapon.bulletConeLength - Random.Range(1,2)))
        {
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
        }
        else
        {
            state = EEnemyState.Fighting;
        }
    }
    void Fighting()
    {
        if (Vector3.Distance(transform.position, target.position) > stoppingDistance)
        {
            state = EEnemyState.MovingTo;
        }
        enemyShoot.OnShoot();

        //rb.velocity = Vector3.zero;
        // leo kommenterade ut det här pga fuckade med knockback
    }

}

public enum EEnemyState
{
    Serching,
    MovingTo,
    Fighting
}



// 15 ml Tequila
// 15 ml Vodka
// 15 ml White rum
// 15 ml Cointreau
// 15 ml Gin
// 30 ml Lemon juice
// 20 ml simple syrup 
//
// 350 ml / 15 = 23
// 
// 30 ml * 23 = 690 ml
// 20 ml * 23 = 460 ml
// 350 ml * 5 = 1750 ml
// 1750 ml + 460 ml + 690 ml = 2900 ml 
// 2900 ml / 23 = 126 ml

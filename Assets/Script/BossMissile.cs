using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    GameObject target;
    NavMeshAgent nav;
    Boss bMg;
    GameObject boss;

    void Awake(){
        nav = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
        boss = GameObject.Find("Enemy D");
        bMg = boss.GetComponent<Boss>();
    }
    void Update()
    {
        if(!bMg.isDead)
            nav.SetDestination(target.transform.position);
    }
}

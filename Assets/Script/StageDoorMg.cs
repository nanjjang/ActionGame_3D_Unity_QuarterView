using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageDoorMg : MonoBehaviour
{
    GameObject target;
    public Animator anim;
    void Start()
    {
        target = GameObject.Find("Player");
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, target.transform.position) <= 30)
        {
            anim.SetBool("isFar",false);
        }
        else
        {
            anim.SetBool("isFar",true);
        }
    }
}

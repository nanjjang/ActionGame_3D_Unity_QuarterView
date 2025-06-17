using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rb;
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; //angularVelocity:오브젝트가 회전하는 속도
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 30, Vector3.up, 0, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }
        Destroy(gameObject, 5);
    }

    void Start()
    {
        StartCoroutine(Explosion());
    }
}
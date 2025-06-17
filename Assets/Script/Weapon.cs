using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bulletCase;
    public GameObject bullet;
    public Transform bulletCasePos;
    public int maxAmmo;
    public int curAmmo;

    public void Use()
    {
        if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Shot()
    {
        //1. 총알 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = intantBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 50;
        yield return null;

        //2. 탄피 배출
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRb = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRb.AddForce(caseVec, ForceMode.Impulse);
        caseRb.AddTorque(Vector3.up * 10, ForceMode.Impulse); //AddTorque : 회전력을 추가
    }
}
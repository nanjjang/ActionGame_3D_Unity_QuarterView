using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRock : Bullet
{
    Rigidbody rb;
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool isShoot=false;
    GameObject boss;
    Boss bMg;

    void Awake()
    {
        boss = GameObject.Find("Enemy D");
        bMg = boss.GetComponent<Boss>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(1.89f);
        isShoot = true;
        rb.AddForce(bMg.gameObject.transform.forward * 300f, ForceMode.Impulse);
    }

    IEnumerator GainPower()
    {
        while(!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;

            transform.localScale = Vector3.one * scaleValue;
            rb.AddTorque(-transform.right * angularPower, ForceMode.Force);

            yield return null; // 얘 없으면 계속 돎.
        }
    }
}

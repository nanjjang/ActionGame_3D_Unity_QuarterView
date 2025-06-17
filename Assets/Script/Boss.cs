using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    public Transform rockShotPos;
    public Vector3 lookVec;
    Vector3 tauntVec;
    public bool isLook;
    bool think = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponentInChildren<Animator>();
        target = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();

        nav.isStopped = true;
    }

    void Update()
    {
        if(mg.Screen!=null)
        {
            StopAllCoroutines();
            return;
        }
        if(isDead)
        {
            StopAllCoroutines();
            think=false;
            return;
        }

        if(Vector3.Distance(transform.position, target.transform.position) >= 60)
        {
            StopAllCoroutines();
            think=false;
            return;
        }

        else if(think == false && Vector3.Distance(transform.position, target.transform.position) <= 100)
        {
            StartCoroutine(Think());
            think=true;
        }

        else if(isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.transform.position + lookVec);
        }
        else
        {
            nav.SetDestination(tauntVec);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.8f);

        int ranAction = Random.Range(0, 5);

        switch(ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(MissileShot()); //40%
                break;
            case 2:
            case 3:
                StartCoroutine(RockShot()); //40%
                break;
            case 4:
                StartCoroutine(Taunt()); //20%
                break;
        }
    }

    IEnumerator MissileShot()
    {
        anim.SetTrigger("doShot");

        yield return new WaitForSeconds(0.2f);
        Instantiate(missile, missilePortA.position, missilePortA.rotation);
        
        yield return new WaitForSeconds(0.3f);
        Instantiate(missile, missilePortB.position, missilePortB.rotation);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, rockShotPos.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        tauntVec = target.transform.position + lookVec;
        
        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;
        Debug.Log("콜라이더 켜짐");

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        Debug.Log("콜라이더 꺼짐");

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;

        StartCoroutine(Think());
    }
}
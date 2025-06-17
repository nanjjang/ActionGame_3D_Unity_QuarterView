using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour 
{
    public enum Type { A, B, C, D, tester};
    public enum Type1 { attck, chase, idle };
    public Type enemyType;
    public Type1 status;
    CoinSystem coinSystem;
    GameObject coinS;
    public int maxHealth;
    public int curHealth;
    public bool isChase;
    public bool isAttack;
    public GameObject bullet;
    public Rigidbody rb;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;
    public Animator anim;
    public GameObject target;
    public NavMeshAgent nav;
    public BoxCollider meleeArea;
    public bool isDead;
    float timer=0f;
    float waiting_time = 0.4f;
    float waiting_time_C = 3f;
    public GameManager mg;
    int count=0;

    public void Distance()
    {
        if(enemyType==Type.C && 20 < Vector3.Distance(transform.position, target.transform.position) && Vector3.Distance(transform.position, target.transform.position) < 50)
        {
            isChase = true;
            isAttack = false;
            status = Type1.chase;
        }
        else if(enemyType==Type.C&&Vector3.Distance(transform.position, target.transform.position) <= 20)
        {
            isChase = false;
            isAttack = true;
            status = Type1.attck;
        }
        if(enemyType!=Type.C&&Vector3.Distance(transform.position,target.transform.position)>6)
        {
            isChase = true;
            isAttack = false;
            status = Type1.chase;
        }
        else if(enemyType!=Type.C&&Vector3.Distance(transform.position,target.transform.position) <= 6)
        {
            isAttack = true;
            isChase = false;
            status = Type1.chase;
        }
        if(Vector3.Distance(transform.position,target.transform.position) >= 50)
        {
            isChase = false;
            isAttack = false;
            anim.SetBool("isIdle",true);
            status = Type1.idle;
        }   
    }
    void Awake()
    {
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        coinS = GameObject.Find("coinS");
        coinSystem = coinS.GetComponent<CoinSystem>();

        if(enemyType != Type.D && status==Type1.chase)
            Invoke("ChaseStart", 2);
    }
    
    void Update()
    {
        if(mg.Screen!=null)
        {
            StopAllCoroutines();
            return;
        }
        Distance();
        timer += Time.deltaTime;
        if (nav.enabled)
        {
            ChaseStart();
            nav.SetDestination(target.transform.position);
            nav.isStopped = !isChase;
        }
    }
    
    void FixedUpdate()
    { 
        FreezeVelocity();
        Targeting();
    }

    public void ChaseStart()
    {
        if(isChase && enemyType != Type.tester && status == Type1.chase)
        {
            if(!anim.GetBool("isAttack"))
                anim.SetBool("isIdle",false);
                anim.SetBool("isWalk", true);
        }
    }

    void Targeting()
    {
        if(!isDead&& enemyType != Type.tester)
        {            
            transform.LookAt(target.transform.position);
            
            if(isAttack && count<1)
            {
                StartCoroutine(Attack());
                timer = 0f;
                count++;
            }
            else if (count>=1 && isAttack && ((enemyType != Type.C && timer >= waiting_time)||(timer>= waiting_time_C && enemyType == Type.C)))
            {
                if(!anim.GetBool("isAttack")) StartCoroutine(Attack()); // 중복 실행 방지를 위해
                timer=0f;
            }
        }
    }
    
    IEnumerator Attack()
    {
        if(enemyType != Type.tester)
        {
            isChase = false;
            isAttack = true;
            anim.SetBool("isIdle",false);
            anim.SetBool("isWalk",false);
            anim.SetBool("isAttack", true);
            
            switch (enemyType)
            {
                case Type.A:
                    yield return new WaitForSeconds(0.8f);
                    meleeArea.enabled = true;
                    
                    yield return new WaitForSeconds(1f);
                    meleeArea.enabled = false;
                    
                    yield return new WaitForSeconds(0.4f); // 애니메이션과 실제 시간, 동작 맞춰주기 위함.
                    break;
                case Type.B:
                    yield return new WaitForSeconds(0.5f);
                    rb.AddForce(transform.forward * 20, ForceMode.Impulse);
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1f);
                    rb.velocity = Vector3.zero;
                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(0.5f);
                    break;
                case Type.C:
                    yield return new WaitForSeconds(0.5f);
                    GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                    Rigidbody rbullet = instantBullet.GetComponent<Rigidbody>();
                    rbullet.velocity = transform.forward * 40;

                    yield return new WaitForSeconds(1f);
                    break;
            }
            isAttack = false;
            anim.SetBool("isAttack", false);
        }
    }

    public void FreezeVelocity()
    {
        if(enemyType != Type.tester)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec,false));
        }
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec,false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 90;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec,true));
    }

    public IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach(MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }

        else
        {
            Debug.Log(isDead);
            if(enemyType!=Type.C)
                meleeArea.enabled=false;
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;
            
            if(!isDead&&enemyType!=Type.D)
            {
                Transform deadP = transform;
                coinSystem.Drop(enemyType, deadP);
            }
            isDead = true;
            isChase = false;
            gameObject.layer = 14;

            anim.SetTrigger("doDie");
            nav.enabled = false;

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;
                rb.freezeRotation = false;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
                rb.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            Destroy(gameObject, 2);
        }
    }
}
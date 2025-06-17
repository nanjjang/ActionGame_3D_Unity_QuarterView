using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public enum Type{swing, handgunshooting, machinegunshooting, throw_generad, item_get, coin_get, healing, reloading, hited}
    public Type status;
    WeaponColiderController WeaponColiderCon;
    public int keys;
    public GameObject[] weapons;
    public GameObject[] grenades;
    public bool[] hasWeapons;
    public LayerMask layer;
    public GameObject grenadeObj;
    public Camera follwCamera;
    public int ammo;
    public int health;
    public int coin;
    public int hasGrenades;
    public int maxAmmo;
    public int maxHealth;
    public int maxCoin;
    public int maxHasGrenades;
    int equipWeaponIndex = -1;
    public float speed;
    float hAxis;
    float vAxis;
    float fireDelay;
    bool dash;
    bool isDodge;
    bool isSwap;
    bool interaction;
    bool n1;
    bool n2;
    bool n3;
    bool fDown;
    bool isFireReady = true;
    bool rDown;
    bool isReload;
    bool gDown;
    bool isDamage;
    Animator anim;
    Vector3 moveVec;
    Vector3 dodgeVec;
    Rigidbody rb;
    public Weapon equipWeapon;
    MeshRenderer[] meshs;
    public Transform player_SpawnPoint;
    GameObject manager;
    GameManager mg;
    SoundMg soundMg;
    GameObject sound;
    CoinSystem system;
    GameObject coinS;
    public LayerMask weapon_layer;
    public LayerMask chest_layer;
    public LayerMask key_layer;
    public bool doOpen = false;
    public Video video;
    void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        rb.freezeRotation = true;
        WeaponColiderCon = GetComponentInChildren<WeaponColiderController>();
        manager = GameObject.Find("GameManager");
        mg = manager.GetComponent<GameManager>();
        sound = GameObject.Find("SoundManager");
        soundMg = sound.GetComponent<SoundMg>();
        coinS = GameObject.Find("coinS");
        system = coinS.GetComponent<CoinSystem>();
    }

    void Start()
    {
        transform.position = player_SpawnPoint.position;
        video.SMove();
    }
    void Update()
    {
        if(mg.Screen!=null)
        {
            moveVec = Vector3.zero;
            rb.velocity = Vector3.zero;
            return;
        }
        GetInput();
        Move();
        Turn();
        Dodge();
        Grenade();
        Swap();
        Attack();
        Reload();
        Interaction();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        dash = Input.GetButton("Run");
        interaction = Input.GetButtonDown("Interation");
        n1 = Input.GetButtonDown("Swap1");
        n2 = Input.GetButtonDown("Swap2");
        n3 = Input.GetButtonDown("Swap3");
        fDown = Input.GetButton("Fire1");
        rDown = Input.GetButtonDown("Reload");
        gDown = Input.GetButtonDown("GThrow");
    }

    void Move()
    {
        
        if (isFireReady && !isReload)
        {
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
            rb.velocity = moveVec * speed * (dash ? 8 : 5) + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            moveVec = Vector3.zero;
            rb.velocity = Vector3.zero;
        }

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        rb.velocity = moveVec * (dash ? 8 : 5) * speed + new Vector3(0, rb.velocity.y, 0);

        if (isDodge) moveVec = dodgeVec;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", dash);
    }

    void Turn()
    {
        // 현재 이동 벡터를 기반으로 회전
        transform.LookAt(transform.position + moveVec);

        // 마우스 클릭 시 회전
        if (fDown)
        {
            Ray ray = follwCamera.ScreenPointToRay(Input.mousePosition);
            // ScreenPointToRay: 화면의 마우스 위치를 기준으로 3D 공간에서의 ray를 생성
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                // out: ray가 충돌한 지점의 정보를 반환
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;  // y축 방향 회전을 방지
                transform.LookAt(transform.position + nextVec);
            }
        }
    }


    void Grenade()
    {
        if (hasGrenades == 0) return;
        if(hasGrenades !=0) video.SThrowing();
        if (gDown && !isDodge && !isReload)
        {
            status = Type.throw_generad;
            soundMg.SFX(status);
            Ray ray = follwCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 10;
                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody GrenadeRb = instantGrenade.GetComponent<Rigidbody>();
                GrenadeRb.AddForce(nextVec, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }
    }

    void Attack()
    {
        if (equipWeapon == null) return;

        if(equipWeapon != null) video.SAttack();

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isDodge && isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            if(equipWeapon.type == Weapon.Type.Melee)
            {
                status = Type.swing;
                soundMg.SFX(status);
            }
            else
            {
                if(equipWeaponIndex == 1)
                {
                    status = Type.handgunshooting;
                    soundMg.SFX(status);
                }
                else if(equipWeaponIndex == 2)
                {
                    status = Type.machinegunshooting;
                    soundMg.SFX(status);
                }
            }
            fireDelay = 0; //공격 쿨타임을 다시 0으로 초기화
        }
    }

    void Reload()
    {
        if (equipWeapon == null) return;
        if (equipWeapon.type == Weapon.Type.Melee) return;
        if (ammo == 0) return;

        if (rDown && !isDodge && isSwap && isFireReady)
        {
            status = Type.reloading;
            soundMg.SFX(status);
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Dodge()
    {
        if (Input.GetButtonDown("Dodge") && !isDodge && moveVec != Vector3.zero)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap()
    {
        if((n1 && !hasWeapons[0]) || (n2 && !hasWeapons[1]) || (n3 && !hasWeapons[2])) return;

        else if(!isDodge&&(n1||n2||n3))
        {
            if(n1) equipWeaponIndex = 0;
            if(n2) equipWeaponIndex = 1;
            if(n3) equipWeaponIndex = 2;

            var otherWeapons = weapons.Where((weapon, index) => index != equipWeaponIndex).ToArray();
            foreach (var weapon in otherWeapons)
            {
                weapon.gameObject.SetActive(false);
            }

            equipWeapon = weapons[equipWeaponIndex].GetComponent<Weapon>();

            if(!equipWeapon.gameObject.activeSelf)
            {
                equipWeapon.gameObject.SetActive(true);
                if(equipWeaponIndex==0) WeaponColiderCon.Find();
                isSwap = true;
            }
            else
            {
                SwapOut();
                isSwap = false;
            }
        }
    }

    void SwapOut()
    {
        equipWeapon = weapons[equipWeaponIndex].GetComponent<Weapon>();
        equipWeapon.gameObject.SetActive(false);
    }

    void Interaction()//이거
    {
        Collider[] coinColliders = Physics.OverlapSphere(gameObject.transform.position, 10, layer);
        Collider[] weaponColliders = Physics.OverlapSphere(gameObject.transform.position, 10, weapon_layer);
        Collider[] chestColliders = Physics.OverlapSphere(gameObject.transform.position,5,chest_layer);
        Collider[] keyColliders = Physics.OverlapSphere(gameObject.transform.position,10, key_layer);

        if (weaponColliders.Length != 0 && interaction && !isDodge)
        {
            Item item = weaponColliders[0].GetComponent<Item>();
            int weaponIndex = item.value;
            hasWeapons[weaponIndex] = true;
            Destroy(weaponColliders[0].gameObject);
        }
        if(coinColliders.Length != 0 && interaction)
        {
            Destroy(coinColliders[0].gameObject);
            status = Type.coin_get;
            soundMg.SFX(status);
            coin += system.value;
        }
        if(chestColliders.Length != 0 && interaction)
        {
            chestColliders[0].GetComponent<Chest>().Open();
        }
        if(keyColliders.Length != 0 && interaction)
        {
            keys += 1;
            Destroy(keyColliders[0].gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isJump", false);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.AddForce(Vector3.back * 1, ForceMode.Impulse);
        }

        if(collision.gameObject.CompareTag("FinishDoor"))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("PlayStartDoor"))
            mg.PlayStart();

        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();

            switch (item.type)
            {
                case Item.Type.Ammo:
                    status = Type.item_get;
                    soundMg.SFX(status);
                    ammo += item.value;
                    if (ammo > maxAmmo) maxAmmo = ammo;
                    break;
                case Item.Type.Heart:
                    status = Type.healing;
                    soundMg.SFX(status);
                    health += item.value;
                    if (health > maxHealth) maxHealth = health;
                    break;
                case Item.Type.Grenade:
                    status = Type.item_get;
                    soundMg.SFX(status);
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades) hasGrenades = maxHasGrenades;
                    break;
            }
            Destroy(other.gameObject);
        }

        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                StartCoroutine(OnDamage());
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
            if(health <= 0)
            {
                mg.Dead();
                gameObject.SetActive(false);
            }
        }

        else if(other.tag == "FinishDoor")
        {
            if(keys == 3)
                mg.Finish();
            else
                mg.cantFinish();
        }
    }

    IEnumerator OnDamage()
    {
        status = Type.hited;
        soundMg.SFX(status);

        isDamage = true;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);

        isDamage = false;

        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
    }
}
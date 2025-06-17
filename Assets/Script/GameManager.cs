using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public GameObject Screen;
    public Text cantFinish_txt;
    public GameObject[] enemyGroup1;
    public GameObject[] enemyGroup2;
    GameObject[] enemyA;
    GameObject[] enemyB;
    GameObject[] enemyC;
    GameObject player;
    Player pMg;
    public GameObject[] Stage_spawnpoint;
    public GameObject deadScreen;
    public Text healthTxt;
    public Text coinTxt;
    public Text ammoTxt;
    public Image hammerIcon;
    public Image handgunIcon;
    public Image machinegunIcon;
    public Image grenadeIncon;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public GameObject bossUiGroup;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealth;
    public LayerMask layer;
    public GameObject playerPositionMark;
    GameObject rMg;
    Rewards rewards;
    public GameObject Finish_door;
    bool esc;
    public GameObject Pause;
    public GameObject ItemShop;
    public GameObject WeaponShop;
    public Boss bMg;
    void Awake()
    {
        player = GameObject.Find("Player");
        pMg = player.GetComponent<Player>();
        bossUiGroup.SetActive(false);
        rMg = GameObject.Find("RMg");
        rewards = rMg.GetComponent<Rewards>();
        ItemShop.SetActive(false);
        WeaponShop.SetActive(false);
    }

    public void PlayStart()
    {
        player.transform.position = Stage_spawnpoint[0].transform.position;
    }
    void Update()
    {
        esc = Input.GetKeyDown(KeyCode.Escape);
        playerPositionMark.transform.position = new Vector3(player.transform.position.x, playerPositionMark.transform.position.y, player.transform.position.z);
        
        enemyA = GameObject.FindGameObjectsWithTag("A");
        enemyB = GameObject.FindGameObjectsWithTag("B");
        enemyC = GameObject.FindGameObjectsWithTag("C");

        if(Screen==null&&esc)
        {
            Screen = Pause;
            Pause.SetActive(true);
        }
        else if(esc)
        {
            Screen.SetActive(false);
            Screen = null;
        }
    }
    void LateUpdate()
    {
        healthTxt.text = pMg.health + " / " + pMg.maxHealth;
        coinTxt.text = string.Format("{0:n0}", pMg.coin);
        if(pMg.equipWeapon == null)
            ammoTxt.text = " - / " + pMg.ammo;
        else if(pMg.equipWeapon.type == Weapon.Type.Melee)
            ammoTxt.text = " - / " + pMg.ammo;
        else
            ammoTxt.text = pMg.equipWeapon.curAmmo + " / " + pMg.ammo;
        
        hammerIcon.color = new Color(1,1,1, pMg.hasWeapons[0] ? 1:0);
        handgunIcon.color = new Color(1,1,1, pMg.hasWeapons[1] ? 1:0);
        machinegunIcon.color = new Color(1,1,1, pMg.hasWeapons[2] ? 1:0);
        grenadeIncon.color = new Color(1,1,1, pMg.hasGrenades > 0 ? 1:0);

        enemyATxt.text = enemyA.Length.ToString();
        enemyBTxt.text = enemyB.Length.ToString();
        enemyCTxt.text = enemyC.Length.ToString();

        Collider[] bossColliders = Physics.OverlapSphere(player.transform.position, 30, layer);
        
        if(bossColliders.Length >= 0 && Vector3.Distance(player.transform.position, bMg.gameObject.transform.position) <= 45) // 보스 죽어도 계속 접근하는거 수정
        {
            bossUiGroup.SetActive(true);
            if(bMg.curHealth>=0)
                bossHealth.localScale = new Vector3((float)bMg.curHealth / bMg.maxHealth , 1, 1);
        }
        else
            bossUiGroup.SetActive(false);

        if(enemyGroup1.All(itme => itme == null))
        {
            rewards.Visible(0);
        }
        if(enemyGroup2.All(itme => itme == null))
        {
            rewards.Visible(1);
        }

        if(bMg.isDead)
        {
            rewards.Visible(2);
            Finish_door.SetActive(true);
        }
    }

    public void Dead()
    {
        deadScreen.SetActive(true);
    }

    public void Restart()
    {
        if(deadScreen)
            deadScreen.SetActive(false);
        SceneManager.LoadScene("Ingame");
    }
    public void Continue()
    {
        Pause.SetActive(false);
        Screen=null;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
    public void game_exit()
    {
        Application.Quit();
    }
    public void Finish()
    {
        SceneManager.LoadScene("Menu");
    }
    public void cantFinish()
    {
        cantFinish_txt.gameObject.SetActive(true);
        
    }
}
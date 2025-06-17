using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject ShopScreen;
    public Animator anim;
    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform itemPos;
    public string[] talkData;
    public Text talkText;
    public Player player;
    public GameManager gm;
    public Video video;
    public Button button1;
    public Button button2;
    public Button button3;

    public void Enter()//이거
    {
        if(!ShopScreen.activeSelf){
            ShopScreen.SetActive(true);
            gm.Screen = ShopScreen;

            button1.onClick.RemoveAllListeners(); // 이전 리스너 제거
            button2.onClick.RemoveAllListeners();
            button3.onClick.RemoveAllListeners();
            button1.onClick.AddListener(() => Buy(0)); // 새로운 메서드 등록
            button2.onClick.AddListener(() => Buy(1));
            button3.onClick.AddListener(() => Buy(2));
        }
    }
    public void Exit()
    {
        if (ShopScreen.activeSelf)
        {
            ShopScreen.SetActive(false);
            anim.SetTrigger("doHello");
            gm.Screen = null;
        }
    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if (price > player.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }
        player.coin -= price;
        Instantiate(itemObj[index], itemPos.position, itemPos.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Enter();
            video.SBuy();
        }
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(10f);
        talkText.text = talkData[0];
    }
}
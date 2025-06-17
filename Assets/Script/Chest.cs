using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour //이거
{
    public Animator anim;
    public GameObject keySpawnPos;
    public int value;
    public bool isOpen;
    GameObject rMg;
    Rewards rewards;

    void Awake()
    {
        isOpen = false;
        rMg = GameObject.Find("RMg");
        rewards = rMg.GetComponent<Rewards>();
    }

    public void Open()
    {
        if(!isOpen)
        {
            isOpen = true;
            anim.SetBool("isOpen",true);
            StartCoroutine(wait()); 
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.9f);
        rewards.KeyDrop(keySpawnPos);
    }
}
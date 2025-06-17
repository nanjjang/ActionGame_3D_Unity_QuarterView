using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour //이거
{
    public GameObject b_coin;
    public GameObject s_coin;
    public GameObject g_coin;
    public int value = 0;

    public void Drop(Enemy.Type type, Transform deadPosition)
    {
        switch (type)
        {
            case Enemy.Type.A:
                value = 250;
                GameObject coinP = Instantiate(b_coin);
                coinP.transform.position = deadPosition.position;
                coinP.SetActive(true);
                break;
            case Enemy.Type.B:
                value = 400;
                coinP = Instantiate(s_coin);
                coinP.transform.position = deadPosition.position;
                coinP.SetActive(true);
                break;
            case Enemy.Type.C:
                value = 500;
                coinP = Instantiate(g_coin);
                coinP.transform.position = deadPosition.position;
                coinP.SetActive(true);
                break;
        }
    }
}
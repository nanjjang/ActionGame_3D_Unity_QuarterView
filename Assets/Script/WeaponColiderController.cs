using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponColiderController : MonoBehaviour
{
    Weapon weapon;
    GameObject Object;

    public void Find()
    {
        Object = GameObject.Find("Weapon Hammer");
        weapon = Object.GetComponent<Weapon>();
    }
//Animation에서 아래 두 함수 호출
    public void Collider_turn_up()
    {
        weapon = GetComponentInChildren<Weapon>();
        weapon.meleeArea.enabled = true;
        weapon.trailEffect.enabled = true;
    }

    public void Collider_turn_down()
    {
        weapon = GetComponentInChildren<Weapon>();
        weapon.meleeArea.enabled = false;
        weapon.trailEffect.enabled = false;
    }

}

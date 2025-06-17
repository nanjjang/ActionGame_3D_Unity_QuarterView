using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMg : MonoBehaviour
{
    public AudioSource sfx;
    public AudioClip handgun;
    public AudioClip machinegun;
    public AudioClip hammer;
    public AudioClip heal;
    public AudioClip item;
    public AudioClip grenade;
    public AudioClip coin;
    public AudioClip reload;
    public AudioClip hit;

    public void SFX(Player.Type s)
    {
        switch (s)
        {
            case Player.Type.swing :
                sfx.clip = hammer;
                sfx.Play();
                break;
            case Player.Type.handgunshooting :
                sfx.clip = handgun;
                sfx.Play();
                break;
            case Player.Type.machinegunshooting :
                sfx.clip = machinegun;
                sfx.Play();
                break;
            case Player.Type.coin_get :
                sfx.clip = coin;
                sfx.Play();
                break;
            case Player.Type.item_get :
                sfx.clip = item;
                sfx.Play();
                break;
            case Player.Type.healing :
                sfx.clip = heal;
                sfx.Play();
                break;
            case Player.Type.hited :
                sfx.clip = hit;
                sfx.Play();
                break;
            case Player.Type.reloading :
                sfx.clip = reload;
                sfx.Play();
                break;
            case Player.Type.throw_generad :
                sfx.clip = grenade;
                sfx.Play();
                break;
        }
    }
}

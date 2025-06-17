using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public VideoPlayer tutorial;
    public GameObject tutorialScreen;
    public VideoClip move;
    public VideoClip buy;
    public VideoClip attack;
    public VideoClip getKey;
    public VideoClip throwing;
    public Camera cam;
    public RenderTexture end;
    public GameObject endScreen;
    bool moveT = false;
    bool buyT = false;
    bool attackT = false;
    bool keyT = false;
    bool throwingT = false;
    
    void Start()
    {
        tutorial.loopPointReached += OnVideoFinished;
        moveT = PlayerPrefs.GetInt("mveT", 0) == 1;  // 저장된 값이 1이면 true, 없거나 0이면 false
        buyT = PlayerPrefs.GetInt("byT", 0) == 1;
        attackT = PlayerPrefs.GetInt("atkT", 0) == 1;
        keyT = PlayerPrefs.GetInt("getkeyT", 0) == 1;
        throwingT = PlayerPrefs.GetInt("throT", 0) == 1;
    }
    public void enter()
    {
        tutorialScreen.SetActive(true);
    }
    public void OnVideoFinished(VideoPlayer vp)
    {
        tutorialScreen.SetActive(false);
    }
    public void SMove()
    {
        if(!moveT)
        {
            enter();
            tutorial.clip = move;
            tutorial.Play();
            moveT = true;
            PlayerPrefs.SetInt("mveT",moveT ? 1:0);
            PlayerPrefs.Save();
        }
    }

    public void SAttack()
    {
        if(!attackT)
        {
            enter();
            tutorial.clip= attack;
            attackT = true;
            PlayerPrefs.SetInt("atkT",attackT ? 1:0);
            PlayerPrefs.Save();
        }
    }

    public void SBuy()
    {
        if(!buyT)
        {
            enter();
            tutorial.clip = buy;
            buyT = true;
            PlayerPrefs.SetInt("byT",buyT ? 1:0);
            PlayerPrefs.Save();
        }
    }

    public void SGetKey()
    {
        if(!keyT){
            enter();
            tutorial.clip = getKey;
            keyT = true;
            PlayerPrefs.SetInt("getkeyT",keyT ? 1:0);
            PlayerPrefs.Save();
        }
    }

    public void SThrowing()
    {
        if(!throwingT){
            enter();
            tutorial.clip = throwing;
            throwingT = true;
            PlayerPrefs.SetInt("throT",throwingT ? 1:0);
            PlayerPrefs.Save();
        }
    }

    public void Ending()
    {
        tutorial.targetTexture = end;
        cam.transform.position = endScreen.transform.position;
    }
}
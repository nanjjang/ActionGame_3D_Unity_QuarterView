using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class option : MonoBehaviour
{
    public Toggle FullToggle;
    public Toggle PopUpToggle;
    public AudioMixer mixer;
    public Slider volumeSlider;
    public GameObject optionScreen;
    public GameObject pause;
    public void Option()
    {
        if(FullToggle.isOn == true)
        {
            Screen.fullScreen = true;
            PopUpToggle.isOn = false;
        }
        if(PopUpToggle.isOn == true)
        {
            Screen.fullScreen = false;
            FullToggle.isOn = false;
        }
            

        if(PlayerPrefs.HasKey("volume"))
            LoadVolume();
        else if(!PlayerPrefs.HasKey("volume"))
            SetVolume();
    }
    public void OpenOp()
    {
        optionScreen.SetActive(true);
        if(pause) pause.SetActive(false);
    }

    public void CloseOp()
    {
        optionScreen.SetActive(false);
    }

    void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        SetVolume();
    }

    void SetVolume()
    {
        mixer.SetFloat("volume",volumeSlider.value);
        PlayerPrefs.SetFloat("volume",volumeSlider.value);
    }
}

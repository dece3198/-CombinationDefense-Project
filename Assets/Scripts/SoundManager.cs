using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer audioMixer;
    [SerializeField] private GameObject soundObj;
    [SerializeField] private GameObject delete;
    [SerializeField] private GameObject bgVolume;
    [SerializeField] private GameObject sfxVolume;
    [SerializeField] private Slider bgBar;
    [SerializeField] private Slider sfxBar;
    private bool isSound = false;
    private bool isBg = false;
    private bool isSFX = false;
    public float bgValue = 0;
    public float sfxValue = 0;
    private bool isDelete = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SoundSetting();
        }
    }

    public void ResetSlider()
    {
        audioMixer.SetFloat("BGSound", bgValue);
        audioMixer.SetFloat("SFXVolume", sfxValue);
        bgBar.value = Mathf.Pow(10, bgValue / 20) / 1;
        sfxBar.value = Mathf.Pow(10, sfxValue / 20) / 1;
    }

    public void BGSoundVolume(float val)
    {
        if(!isBg)
        {
            audioMixer.SetFloat("BGSound", Mathf.Log10(val) * 20);
        }
        bgValue = Mathf.Log10(val) * 20;
    }

    public void SFXSoundVolume(float val)
    {
        if(!isSFX)
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(val) * 20);
        }
        sfxValue = Mathf.Log10(val) * 20;
    }

    public void SoundSetting()
    {
        isSound = !isSound;

        if(isSound)
        {
            soundObj.SetActive(true);
            StageManager.instance.audioSource.PlayOneShot(StageManager.instance.audioClips[0]);
        }
        else
        {
            soundObj.SetActive(false);
            StageManager.instance.audioSource.PlayOneShot(StageManager.instance.audioClips[1]);
        }
    }

    public void BGButton()
    {
        isBg = !isBg;

        if (isBg)
        {
            bgVolume.SetActive(true);
            audioMixer.SetFloat("BGSound", -80);
        }
        else
        {
            bgVolume.SetActive(false);
            audioMixer.SetFloat("BGSound", bgValue);
        }

    }

    public void SFXButton()
    {
        isSFX = !isSFX;
        if(isSFX)
        {
            sfxVolume.SetActive(true);
            audioMixer.SetFloat("SFXVolume", -80);
        }
        else
        {
            sfxVolume.SetActive(false);
            audioMixer.SetFloat("SFXVolume", sfxValue);
        }
    }

    public void DeleteButton()
    {
        isDelete = !isDelete;
        if(isDelete)
        {
            delete.SetActive(true);
        }
        else
        {
            delete.SetActive(false);
        }
    }

    public void EndButton()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject soundObj;
    [SerializeField] private GameObject bgVolume;
    [SerializeField] private GameObject sfxVolume;
    private bool isSound = false;
    private bool isBg = false;
    private bool isSFX = false;
    [SerializeField] private float bgValue = 0;
    private float sfxValue = 0;

    private void Start()
    {
        BGSoundVolume(0.2f);
        SFXSoundVolume(0.2f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SoundSetting();
        }
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

    public void EndButton()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//Audio管理器
public class AudioManager : MonoBehaviour
{
    public static AudioManager s_AudioManager { get { return audioManager; } }
    private static AudioManager audioManager;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip
        MoveClip,//移動
        ClickClip;//確認
    private void Awake()
    {
        if (audioManager == null)
            audioManager = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        
    }
    public static void PlayAudio(AudioClip _audioClip)
    {
        audioManager.audioSource.PlayOneShot(_audioClip);
    }
    //移動鍵音效
    public void PlayAudio_Move()
    {
        audioManager.audioSource.PlayOneShot(MoveClip);
    }
    //確認音效
    public void PlayAudio_Click()
    {
        audioManager.audioSource.PlayOneShot(ClickClip);
    }
}

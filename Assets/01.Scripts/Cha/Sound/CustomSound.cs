using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioClip;
    
    private bool isFirstEnable = true; // 첫 번째 OnEnable 호출 여부 확인
    
    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // 처음 호출 시에는 무시하고 플래그를 꺼줌
        if (isFirstEnable)
        {
            isFirstEnable = false;
            return;
        }

        // 이후 활성화 시 사운드 재생
        audioSource?.PlayOneShot(audioClip);
    }
    
}

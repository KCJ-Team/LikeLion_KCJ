using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    // UI 요소 참조
    public Toggle fullscreenToggle; // 전체화면 Toggle
    public Toggle windowedToggle;   // 창모드 Toggle
    public ToggleGroup screenModeToggleGroup; // Toggle 그룹
    public Slider volumeSlider;     // 마스터 볼륨 슬라이더

    // 기본 해상도
    private const int FullscreenWidth = 1920;
    private const int FullscreenHeight = 1080;
    private const int WindowedWidth = 1280;
    private const int WindowedHeight = 720;
    
    // 비율
    private const float AspectRatio = 16f / 9f;
    
    // 볼륨 저장 키
    private const string VolumeKey = "MASTER_VOLUME";

    void Start()
    {
        // 초기 창모드 해상도로 시작
        Screen.SetResolution(WindowedWidth, WindowedHeight, false); // 창모드
        Screen.fullScreenMode = FullScreenMode.Windowed;
        
        // Toggle 그룹 설정
        if (screenModeToggleGroup != null)
        {
            fullscreenToggle.group = screenModeToggleGroup;
            windowedToggle.group = screenModeToggleGroup;
        }

        // Toggle 초기화 및 이벤트 리스너 설정
        fullscreenToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) SetFullscreenMode(true);
        });

        windowedToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) SetFullscreenMode(false);
        });

        InitializeFullscreenToggle();

        // Slider 초기화 및 이벤트 리스너 설정
        volumeSlider.onValueChanged.AddListener(SetMasterVolume);
        InitializeVolume();
    }

    // 화면 모드 설정
    public void SetFullscreenMode(bool isFullscreen)
    {
        if (isFullscreen)
        {
            // 전체화면 모드로 전환
            Screen.SetResolution(FullscreenWidth, FullscreenHeight, true); // 전체화면
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            // 창모드로 전환
            Screen.SetResolution(WindowedWidth, WindowedHeight, false); // 창모드
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    // 볼륨 설정
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume); // 볼륨 값 저장
    }

    // 초기화: Toggle
    private void InitializeFullscreenToggle()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            windowedToggle.isOn = true;
        }
        else
        {
            fullscreenToggle.isOn = true;
        }
    }

    // 초기화: Slider
    private void InitializeVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1.0f); // 기본값: 1.0
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
    }
    
    // void Update()
    // {
    //     // 창모드에서 크기 조정 시 비율 유지
    //     if (Screen.fullScreenMode == FullScreenMode.Windowed)
    //     {
    //         int newWidth = Screen.width;
    //         int newHeight = Mathf.RoundToInt(newWidth / AspectRatio);
    //
    //         // 현재 크기와 새 크기가 다른 경우 적용
    //         if (Screen.height != newHeight)
    //         {
    //             Screen.SetResolution(newWidth, newHeight, false);
    //         }
    //     }
    // }
}

using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioSourceType
{
    BGM, SFX, UI
}

public enum SFXSoundType
{
    Skill1, Skill2
}

public enum BGMSoundType
{
    MainMenuTheme,
    LobbyTheme,
    DungeonTheme,
    EndingTheme
}

public enum PlayerSoundType
{
    Footstep, Jump
}

public enum UISoundType
{ 
    Hover, Click, OK, Cancel
}

public enum MonsterSoundType
{
    Idle, Damaged
}


public class SoundManager : SceneSingleton<SoundManager>
{
    [Header("Mixer와 오디오 소스들")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource bgmSource; // BGM만 관리
    [SerializeField] private AudioSource sfxSource; // 스킬 사운드 효과, 폭발 효과 등 SFX
    [SerializeField] private AudioSource uiSource; // UI 사운드만 관리
    [SerializeField] private AudioSource playerSource; // 플레이어 발자국, 점프 등 플레이어만 관리
    [SerializeField] private AudioSource monsterSource; // 몬스터만 관리

    [Header("각 사운드 타입별 오디오 클립 모음")] 
    public SerializedDictionary<BGMSoundType, AudioClip> bgmClips = new();
    public SerializedDictionary<SFXSoundType, AudioClip> sfxClips = new();
    public SerializedDictionary<PlayerSoundType, AudioClip> playerClips = new();
    public SerializedDictionary<UISoundType, AudioClip> uiClips = new();
    public SerializedDictionary<MonsterSoundType, AudioClip> monsterClips = new();
    
    // BGM 재생 메소드
    public void PlayBGM(BGMSoundType bgmType)
    {
        if (bgmClips.TryGetValue(bgmType, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM clip for '{bgmType}' not found.");
        }
    }

    // SFX 재생 메소드
    public void PlaySFX(SFXSoundType sfxType)
    {
        if (sfxClips.TryGetValue(sfxType, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX clip for '{sfxType}' not found.");
        }
    }

    // 플레이어 효과음 재생 메소드
    public void PlayPlayerSound(PlayerSoundType playerSoundType)
    {
        if (playerClips.TryGetValue(playerSoundType, out AudioClip clip))
        {
            playerSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Player sound clip for '{playerSoundType}' not found.");
        }
    }

    // UI 사운드 재생 메소드
    public void PlayUISound(UISoundType uiSoundType)
    {
        if (uiClips.TryGetValue(uiSoundType, out AudioClip clip))
        {
            uiSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"UI sound clip for '{uiSoundType}' not found.");
        }
    }

    // 몬스터 효과음 재생 메소드
    public void PlayMonsterSound(MonsterSoundType monsterSoundType)
    {
        if (monsterClips.TryGetValue(monsterSoundType, out AudioClip clip))
        {
            monsterSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Monster sound clip for '{monsterSoundType}' not found.");
        }
    }

    // 모든 BGM 정지 메소드
    public void StopBGM()
    {
        bgmSource.Stop();
    }
} // end class

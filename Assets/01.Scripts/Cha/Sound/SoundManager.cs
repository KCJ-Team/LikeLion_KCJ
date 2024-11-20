using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using PlayerInfo;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public enum AudioSourceType
{
    BGM, SFX, UI, Player, Monster, Weapon
}

public enum SFXSoundType
{
    PositivePop, NegativePop, Skill_Invisibility,Skill_KnockBack, Skill_Mine, Skill_Pull, Skill_Shadow, Skill_BlackHole, Skill_Bomb, Skill_RainBullet, Skill_Turret, Buff
}

public enum BGMSoundType
{
    MainMenuTheme,
    LobbyTheme,
    DungeonTheme,
    EndingTheme
}

public enum UISoundType
{ 
    Hover, Click, OK, Cancel, LoadScene, Thick, Pop, None, Noti, Alert, Ending
}

public enum MonsterSoundType
{
    Idle, Died
}

public class SoundManager : MonoBehaviour
{
    // 전역 싱글톤 인스턴스
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 인스턴스가 없으면 GameObject 생성 후 컴포넌트 추가
                GameObject singletonObj = new GameObject(nameof(SoundManager));
                _instance = singletonObj.AddComponent<SoundManager>();
                DontDestroyOnLoad(singletonObj);
            }
            return _instance;
        }
    }
    
    [Header("Mixer와 오디오 소스들")]
    [SerializeField] private AudioMixer audioMixer;
    public SerializedDictionary<AudioSourceType, AudioSource> audioSources;

    [Header("각 사운드 타입별 오디오 클립 모음")] 
    public SerializedDictionary<BGMSoundType, AudioClip> bgmClips = new();
    public SerializedDictionary<SFXSoundType, AudioClip> sfxClips = new();
    public SerializedDictionary<UISoundType, AudioClip> uiClips = new();
    public SerializedDictionary<MonsterSoundType, AudioClip> monsterClips = new();
    public SerializedDictionary<PlayerWeaponType, AudioClip> weaponClips = new();
  
    [Header("footstep")]
    public AudioClip[] playerFootStep;
    private Coroutine footstepCoroutine;
    private int _currentFootstepIndex = 0;
    private bool isPlayingFootsteps = false;
    
    private Dictionary<SFXSoundType, float> sfxCooldowns = new Dictionary<SFXSoundType, float>();
    private const float SFXCooldownDuration = 0.2f; // SFX 쿨다운 시간 (초 단위)
    
    private void Awake()
    {
        // 싱글톤 인스턴스가 중복으로 생성되지 않도록 보장
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private AudioSource GetAudioSource(AudioSourceType type)
    {
        if (audioSources.TryGetValue(type, out AudioSource source))
        {
            return source;
        }

        return null;
    }
    
    // BGM 재생
    public void PlayBGM(BGMSoundType bgmType)
    {
        var source = GetAudioSource(AudioSourceType.BGM);
        if (source != null && bgmClips.TryGetValue(bgmType, out AudioClip clip))
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }
        else
        {
            Debug.LogWarning($"BGM clip for '{bgmType}' not found.");
        }
    }
    
    // SFX 재생 메소드
    public void PlaySFX(SFXSoundType sfxType)
    {
         var source = GetAudioSource(AudioSourceType.SFX);
        // if (source != null && sfxClips.TryGetValue(sfxType, out AudioClip clip))
        // {
        //     source.PlayOneShot(clip);
        // }
        // else
        // {
        //     Debug.LogWarning($"SFX clip for '{sfxType}' not found.");
        // }
        
        // 쿨다운 체크
        if (sfxCooldowns.ContainsKey(sfxType) && Time.time < sfxCooldowns[sfxType])
        {
            Debug.Log($"SFX '{sfxType}' is on cooldown. Skipping.");
            return; // 쿨다운 중이면 재생하지 않음
        }

        // SFX 재생
        if (source != null && sfxClips.TryGetValue(sfxType, out AudioClip clip))
        {
            source.PlayOneShot(clip);
            sfxCooldowns[sfxType] = Time.time + SFXCooldownDuration; // 쿨다운 설정
        }
        else
        {
            Debug.LogWarning($"SFX clip for '{sfxType}' not found.");
        }
    }
    
    // 플레이어 효과음 재생 메소드 ex 풋스텝
    public void PlayFootstep(float speed)
    {
        if (isPlayingFootsteps) return; // 이미 재생 중이라면 무시

        isPlayingFootsteps = true;
        footstepCoroutine = StartCoroutine(PlayFootstepRoutine(speed));
    }
    
    public void StopFootstep()
    {
        if (!isPlayingFootsteps) return; // 이미 멈춰 있으면 무시

        isPlayingFootsteps = false;
        if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
        }
    }
    
    private IEnumerator PlayFootstepRoutine(float speed)
    {
        // 이동 속도에 따라 재생 간격 계산
        float footstepInterval = 1f / speed;

        while (true)
        {
            // 발자국 소리가 없을 경우 실행하지 않음
            if (playerFootStep.Length == 0) yield break;

            // 현재 발자국 소리 재생
            var source = GetAudioSource(AudioSourceType.Player);
            if (source != null)
            {
                AudioClip clip = playerFootStep[_currentFootstepIndex];
                source.PlayOneShot(clip);

                // 다음 발자국 소리로 이동 (루프)
                _currentFootstepIndex = (_currentFootstepIndex + 1) % playerFootStep.Length;
            }

            // 다음 발자국 소리까지 대기
            yield return new WaitForSeconds(footstepInterval);
        }
    }
    
    // UI 사운드 재생 메소드
    public void PlayUISound(UISoundType uiSoundType)
    {
        var source = GetAudioSource(AudioSourceType.UI);
        if (source != null && uiClips.TryGetValue(uiSoundType, out AudioClip clip))
        {
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"UI sound clip for '{uiSoundType}' not found.");
        }
        
        // // 쿨다운 체크
        // if (sfxCooldowns.ContainsKey(uiSoundType) && Time.time < sfxCooldowns[uiSoundType])
        // {
        //     return; // 쿨다운 중이면 재생하지 않음
        // }
        //
        // // SFX 재생
        // if (source != null && uiClips.TryGetValue(uiSoundType, out AudioClip clip))
        // {
        //     source.PlayOneShot(clip);
        //     sfxCooldowns[uiSoundType] = Time.time + SFXCooldownDuration; // 쿨다운 설정
        // }
        // else
        // {
        //     Debug.LogWarning($"SFX clip for '{uiSoundType}' not found.");
        // }
    }
    
    // 몬스터 효과음 재생 메소드
    public void PlayMonsterSound(MonsterSoundType monsterSoundType)
    {
        var source = GetAudioSource(AudioSourceType.Monster);
        if (source != null && monsterClips.TryGetValue(monsterSoundType, out AudioClip clip))
        {
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Monster sound clip for '{monsterSoundType}' not found.");
        }
    }

    public void WeaponSound(PlayerWeaponType weaponType)
    {
        var source = GetAudioSource(AudioSourceType.Weapon);
        if (source != null && weaponClips.TryGetValue(weaponType, out AudioClip clip))
        {
            source.PlayOneShot(clip);
        }
    }
    
    // 특정 AudioSourceType 정지
    public void StopAudio(AudioSourceType type)
    {
        var source = GetAudioSource(type);
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }
    
} // end class

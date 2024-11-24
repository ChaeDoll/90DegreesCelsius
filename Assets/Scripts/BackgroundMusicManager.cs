using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* Developed By 임채윤 2024.11.24
 * 설명 : 배경음악을 관리하는 Script이다.
 * Singleton으로 구현되어 있으며, Phase 변환 시 자연스럽게 음향이 변경된다. */
public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }

    [SerializeField] private float MusicVolume = 0.5f;
    [SerializeField] private AudioClip[] BackgroundMusics;

    private float ChangeDuration = 1.0f;
    private AudioSource BackgroundAudioSource;

    private void Awake()
    {
        // Singleton 패턴 구현
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복된 SoundManager 제거
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
    }

    private void Start()
    {
        BackgroundAudioSource = GetComponent<AudioSource>();
        PhaseManager.Instance.OnPhaseChanged += UpdateMusic;
        UpdateMusic(PhaseManager.Instance.CurrentPhase);
    }

    private void UpdateMusic(int Phase) // OnPhaseChanged Callback Event
    {
        DisableBackgroundMusic(); // 배경음악 변경 전, 음량 FadeOut

        AudioClip ClipToPlay = BackgroundMusics[Phase];
        if (ClipToPlay != null && BackgroundAudioSource.clip != ClipToPlay)
        {
            BackgroundAudioSource.clip = ClipToPlay;
            BackgroundAudioSource.Play();
        }

        EnableBackgroundMusic(); // 배경음악 변경 후, 음량 FadeIn
    }

    /* Developed By 임채윤 2024.11.24
     * 설명 : 배경음악을 서서히 Enable 또는 Disable한다. */
    public void EnableBackgroundMusic()
    {
        StartCoroutine(ChangeVolume(0f, MusicVolume, ChangeDuration));
    }
    public void DisableBackgroundMusic()
    {
        StartCoroutine(ChangeVolume(MusicVolume, 0f, ChangeDuration));
    }
    IEnumerator ChangeVolume(float CurrentVolume, float TargetVolume, float Duration)
    {
        float TotalTime = 0; // 경과 시간
        while (TotalTime < Duration)
        {
            BackgroundAudioSource.volume = Mathf.Lerp(CurrentVolume, TargetVolume, TotalTime / Duration);
            TotalTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        BackgroundAudioSource.volume = TargetVolume;
    }

    private void OnDestroy()
    {
        PhaseManager.Instance.OnPhaseChanged -= UpdateMusic;
    }
}

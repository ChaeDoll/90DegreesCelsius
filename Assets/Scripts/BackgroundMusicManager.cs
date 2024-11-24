using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* Developed By ��ä�� 2024.11.24
 * ���� : ��������� �����ϴ� Script�̴�.
 * Singleton���� �����Ǿ� ������, Phase ��ȯ �� �ڿ������� ������ ����ȴ�. */
public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }

    [SerializeField] private float MusicVolume = 0.5f;
    [SerializeField] private AudioClip[] BackgroundMusics;

    private float ChangeDuration = 1.0f;
    private AudioSource BackgroundAudioSource;

    private void Awake()
    {
        // Singleton ���� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �ߺ��� SoundManager ����
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
    }

    private void Start()
    {
        BackgroundAudioSource = GetComponent<AudioSource>();
        PhaseManager.Instance.OnPhaseChanged += UpdateMusic;
        UpdateMusic(PhaseManager.Instance.CurrentPhase);
    }

    private void UpdateMusic(int Phase) // OnPhaseChanged Callback Event
    {
        DisableBackgroundMusic(); // ������� ���� ��, ���� FadeOut

        AudioClip ClipToPlay = BackgroundMusics[Phase];
        if (ClipToPlay != null && BackgroundAudioSource.clip != ClipToPlay)
        {
            BackgroundAudioSource.clip = ClipToPlay;
            BackgroundAudioSource.Play();
        }

        EnableBackgroundMusic(); // ������� ���� ��, ���� FadeIn
    }

    /* Developed By ��ä�� 2024.11.24
     * ���� : ��������� ������ Enable �Ǵ� Disable�Ѵ�. */
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
        float TotalTime = 0; // ��� �ð�
        while (TotalTime < Duration)
        {
            BackgroundAudioSource.volume = Mathf.Lerp(CurrentVolume, TargetVolume, TotalTime / Duration);
            TotalTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }
        BackgroundAudioSource.volume = TargetVolume;
    }

    private void OnDestroy()
    {
        PhaseManager.Instance.OnPhaseChanged -= UpdateMusic;
    }
}

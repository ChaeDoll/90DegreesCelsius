using System;
using UnityEngine;

/* Developed By 임채윤 2024.11.24
 * 설명 : Phase를 Singleton으로 어느 위치에서든 편리하게 관리한다.
 * Phase에 맞게 현재 Environment와 BackgroundMusic을 설정한다. */
public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance { get; private set; }
    public int CurrentPhase { get; private set; } = 0;
    public event Action<int> OnPhaseChanged;

    [SerializeField] private GameObject[] Environments;

    private void Awake()
    {
        // Singleton 패턴 구현
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (Environments == null || Environments.Length == 0) { return; }

        UpdateEnvironment(PhaseManager.Instance.CurrentPhase);
        PhaseManager.Instance.OnPhaseChanged += UpdateEnvironment;
    }

    private void UpdateEnvironment(int Phase)
    {
        for (int i = 0; i < Environments.Length; i++)
        {
            Environments[i].SetActive(i == Phase);
        }
    }

    public void SetPhase(int newPhase)
    {
        if (CurrentPhase == newPhase)
            return;
        CurrentPhase = newPhase;
        // Phase 변경 이벤트가 있으면 실행
        OnPhaseChanged?.Invoke(CurrentPhase);
    }

    private void OnDestroy()
    {
        if (PhaseManager.Instance != null)
        {
            PhaseManager.Instance.OnPhaseChanged -= UpdateEnvironment;
        }
    }
}

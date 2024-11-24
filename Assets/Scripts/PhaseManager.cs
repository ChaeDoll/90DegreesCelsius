using System;
using UnityEngine;

/* Developed By ��ä�� 2024.11.24
 * ���� : Phase�� Singleton���� ��� ��ġ������ ���ϰ� �����Ѵ�.
 * Phase�� �°� ���� Environment�� BackgroundMusic�� �����Ѵ�. */
public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance { get; private set; }
    public int CurrentPhase { get; private set; } = 0;
    public event Action<int> OnPhaseChanged;

    [SerializeField] private GameObject[] Environments;

    private void Awake()
    {
        // Singleton ���� ����
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
        // Phase ���� �̺�Ʈ�� ������ ����
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOpacityManager : MonoBehaviour
{
    [Header("Initial Setting")]
    [SerializeField] private Transform HeadPosition;
    [SerializeField] private OVRPassthroughLayer PassthroughLayer;

    public bool VRMode;

    private void Start()
    {
        PassthroughLayer.textureOpacity = 0f; 
        //PassthroughLayer.textureOpacity = 1f; //�ʱ� Opacity�� 1 (MR ���)
        VRMode = true;
        //VRMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (VRMode) // It works only VR Mode
        {
            float HeadRotation = HeadPosition.localEulerAngles.x;
            PassthroughLayer.textureOpacity = RotateToOpacity(HeadRotation);
        }
        else // MR Mode
        {
            
        }
    }

    /* Developed By ��ä�� 2024.11.24
     * ���� : VR ���� MR ��带 �����Ѵ�. */
    public void EnableVRMode()
    {
        StartCoroutine(ChangeOpacity(1f, 0f, 1f));
        VRMode = true;
    }
    public void EnableMRMode()
    {
        StartCoroutine(ChangeOpacity(0f, 1f, 1f));
        VRMode = false;
    }
    IEnumerator ChangeOpacity(float Start, float End, float Duration)
    {
        float TotalTime = 0; // ��� �ð�
        while(TotalTime < Duration)
        {
            PassthroughLayer.textureOpacity = Mathf.Lerp(Start, End, TotalTime / Duration);
            TotalTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }
        PassthroughLayer.textureOpacity = End;
    }

    /* Developed By ��ä�� 2024.11.19
     * ���� : �� ������ 0���� 1 ������ Passthrough Opacity ������ �����Ѵ�. */
    float RotateToOpacity(float angle)
    {
        float opacity;
        if (angle > 180f) angle -= 360f;
        angle = Math.Clamp(angle, -90f, 45f);
        if (angle >= 15f)
            opacity = Mathf.Clamp((angle - 15f) / 20f, 0f, 1f); // �� ���� �Ʒ��� 15 to 35 -> Opacity 1 to 0 (VR -> MR ���)
        else
            opacity = 0f; // �� ���� �Ʒ��� 15������ ������ �׻� Opacity to 0 (�׻� VR ���)
        return opacity;
    }
}

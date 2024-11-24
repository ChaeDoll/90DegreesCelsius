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
        //PassthroughLayer.textureOpacity = 1f; //초기 Opacity는 1 (MR 모드)
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

    /* Developed By 임채윤 2024.11.24
     * 설명 : VR 모드와 MR 모드를 변경한다. */
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
        float TotalTime = 0; // 경과 시간
        while(TotalTime < Duration)
        {
            PassthroughLayer.textureOpacity = Mathf.Lerp(Start, End, TotalTime / Duration);
            TotalTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        PassthroughLayer.textureOpacity = End;
    }

    /* Developed By 임채윤 2024.11.19
     * 설명 : 고개 각도를 0에서 1 사이의 Passthrough Opacity 값으로 변경한다. */
    float RotateToOpacity(float angle)
    {
        float opacity;
        if (angle > 180f) angle -= 360f;
        angle = Math.Clamp(angle, -90f, 45f);
        if (angle >= 15f)
            opacity = Mathf.Clamp((angle - 15f) / 20f, 0f, 1f); // 고개 각도 아래로 15 to 35 -> Opacity 1 to 0 (VR -> MR 모드)
        else
            opacity = 0f; // 고개 각도 아래로 15도보다 높으면 항상 Opacity to 0 (항상 VR 모드)
        return opacity;
    }
}

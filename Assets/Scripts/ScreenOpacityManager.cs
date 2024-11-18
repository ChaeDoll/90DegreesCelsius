using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOpacityManager : MonoBehaviour
{
    [Header("Initial Setting")]
    [SerializeField] private Transform headPosition;
    [SerializeField] private OVRPassthroughLayer passthroughLayer;

    private void Start()
    {
        passthroughLayer.textureOpacity = 0f; //초기 Opacity는 0 (VR 모드)
    }

    // Update is called once per frame
    void Update()
    {
        float headRotation = headPosition.localEulerAngles.x;
        passthroughLayer.textureOpacity = RotateToOpacity(headRotation);
    }

    /* Developed By 임채윤
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

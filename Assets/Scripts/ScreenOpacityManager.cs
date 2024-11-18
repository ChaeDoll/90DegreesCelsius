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
        passthroughLayer.textureOpacity = 0f; //�ʱ� Opacity�� 0 (VR ���)
    }

    // Update is called once per frame
    void Update()
    {
        float headRotation = headPosition.localEulerAngles.x;
        passthroughLayer.textureOpacity = RotateToOpacity(headRotation);
    }

    /* Developed By ��ä��
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

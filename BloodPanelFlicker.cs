using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodPanelFlicker : MonoBehaviour
{
    [Header("Panel ����ͧ��á�о�Ժ")]
    [SerializeField] private Image bloodPanel;

    [Header("��Ҥ�������ʵ���ش-�٧�ش")]
    [SerializeField] private float minAlpha = 0.5f; // �� 50%
    [SerializeField] private float maxAlpha = 0.7f; // �� 70%

    [Header("��������㹡�á�о�Ժ (����ҡ�������)")]
    [SerializeField] private float flickerSpeed = 0.5f;

    private Coroutine flickerRoutine;

    /// <summary>
    /// ���¡�����ʹ��������������о�Ժ
    /// </summary>
    public void StartFlicker()
    {
        // ����ա�á�о�Ժ�������� �����ش��͹
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
        }
        // ����� coroutine ����
        flickerRoutine = StartCoroutine(FlickerLoop());
    }

    /// <summary>
    /// ���¡�����ʹ���������ش��о�Ժ
    /// </summary>
    public void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }

        // ���� alpha = 0 (���ͨе���繤����� � ����)
        if (bloodPanel != null)
        {
            Color c = bloodPanel.color;
            c.a = 0f;
            bloodPanel.color = c;
        }
    }

    private IEnumerator FlickerLoop()
    {
        // ������鹴��� alpha ��ҧ �
        float alpha = minAlpha;
        bool goingUp = true; // �����Ѻ��ȷҧ���ŧ

        while (true)
        {
            // ���� � ��Ѻ alpha
            float step = flickerSpeed * Time.deltaTime;
            if (goingUp)
            {
                alpha += step;
                if (alpha >= maxAlpha)
                {
                    alpha = maxAlpha;
                    goingUp = false;
                }
            }
            else
            {
                alpha -= step;
                if (alpha <= minAlpha)
                {
                    alpha = minAlpha;
                    goingUp = true;
                }
            }

            // �絤�ҡ�Ѻ价�� Image
            if (bloodPanel != null)
            {
                Color c = bloodPanel.color;
                c.a = alpha;
                bloodPanel.color = c;
            }

            yield return null;
        }
    }
}

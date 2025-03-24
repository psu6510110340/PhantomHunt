using UnityEngine;

public class HandBloodController : MonoBehaviour
{
    private Renderer[] allRenderers;

    // ��һѨ�غѹ���������¢ͧ Alpha
    private float currentAlpha = 0f;
    private float targetAlpha = 0f;

    // ��Ѻ������ͧ��� (����ҡ ���࿴���)
    private float fadeSpeed = 10f;

    void Awake()
    {
        allRenderers = GetComponentsInChildren<Renderer>();

        // ������鹷�� Alpha = 0 (�ͧ������)
        SetAlpha(0f);
        // �Դ Renderer �ѹ�� (����ͧ Render ��� alpha = 0)
        EnableRenderers(false);
    }

    void Update()
    {
        // ����� currentAlpha ��ѧ targetAlpha ���ҧ�������
        if (Mathf.Abs(currentAlpha - targetAlpha) > 0.01f)
        {
            currentAlpha = Mathf.MoveTowards(
                currentAlpha,
                targetAlpha,
                fadeSpeed * Time.deltaTime
            );
            SetAlpha(currentAlpha);
        }
        else
        {
            // ����������������� ���ѧ�Ѻ�繤�����
            currentAlpha = targetAlpha;
            SetAlpha(currentAlpha);

            // ��� alpha == 0 => �Դ renderer
            if (Mathf.Approximately(currentAlpha, 0f))
            {
                EnableRenderers(false);
            }
        }
    }

    /// <summary>
    /// ���¡����� �ⴹ俔 => visible = true => targetAlpha = 1
    ///        ����� ����ⴹ俔 => visible = false => targetAlpha = 0
    /// </summary>
    public void SetVisible(bool visible)
    {
        targetAlpha = visible ? 1f : 0f;

        // ��Ҩ�����ʴ� ���Դ renderer �ѹ�� ������������࿴�ҡ 0 -> 1 ��
        if (visible)
        {
            EnableRenderers(true);
        }
    }

    /// <summary>
    /// �� alpha ���Ѻ material �ͧ renderer ������
    /// �����˵�: ��ͧ������ Shader ������� property ����Ѻ alpha 
    ///           �� "_BaseColor" ���� "_Color" (������ Shader)
    ///           ����� URP �ѡ���� "_BaseColor"
    /// </summary>
    private void SetAlpha(float alpha)
    {
        foreach (var rend in allRenderers)
        {
            // ������ҧ����� "_BaseColor" ����� Standard Pipeline �Ҩ�� "_Color"
            if (rend.material.HasProperty("_BaseColor"))
            {
                Color c = rend.material.GetColor("_BaseColor");
                c.a = alpha;
                rend.material.SetColor("_BaseColor", c);
            }
            // ��� Shader �ժ��� property ��� ������¹������
        }
    }

    private void EnableRenderers(bool enable)
    {
        foreach (var rend in allRenderers)
        {
            rend.enabled = enable;
        }
    }
}

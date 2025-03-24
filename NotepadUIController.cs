using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotepadUIController : MonoBehaviour
{
    [Header("Notepad UI")]
    [SerializeField] private RectTransform notepadRect;
    [SerializeField] private Image notepadImage;          // �Ҿ�����ʴ��ٻ Notepad
    [SerializeField] private Text contentText;            // ������� Notepad (���������ź�͡��)

    [Header("Notepad Move Settings")]
    [SerializeField] private Vector2 offScreenPos = new Vector2(0f, -500f);
    [SerializeField] private Vector2 centerPos = new Vector2(0f, 0f);
    [SerializeField] private float moveTime = 1f;

    [Header("Overlay UI (Dim Panel)")]
    [SerializeField] private Image overlayImage;           // Panel �ӷ�����ҧ�����ѧ
    [SerializeField] private float overlayFadeTime = 0.5f;
    [SerializeField] private float overlayAlphaTarget = 0.5f; // ��������ͧ�����ѧ���Ң��

    [Header("Audio Settings")]
    [SerializeField] private AudioSource notepadAudioSource;  // AudioSource ����Ѻ������§
    [SerializeField] private AudioClip paperOpenClip;         // ���§�͹�Դ
    [SerializeField] private AudioClip paperCloseClip;        // ���§�͹�Դ

    // ==============================
    // ���������
    private bool isVisible = false;   // �͹����Դ�������
    private bool isMoving = false;    // ���ѧ����͹ UI ������������

    // ���ʤ�Ի���������ҹ������Դ���ͻԴ
    public bool IsVisible { get { return isVisible; } }


    void Start()
    {
        // �����������͹ Notepad
        if (notepadRect != null)
            notepadRect.gameObject.SetActive(false);

        // ��͹ Overlay
        if (overlayImage != null)
        {
            Color c = overlayImage.color;
            c.a = 0f;
            overlayImage.color = c;
            overlayImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���ʹ����Ѻ�Դ Notepad ���Ѻ�����Ũҡ�ѵ�� 3D (Notepad3DObject) ���������Ҿ/������
    /// </summary>
    /// <param name="data">Notepad3DObject �ͧ��������ͧ����Դ</param>
    public void OpenNotepad(Notepad3DObject data)
    {
        // ��ҡ��ѧ����͹��������Դ�������� ��� return
        if (isMoving || isVisible) return;

        // 1) ��駤�� Sprite / ��ͤ������ç���������������Ժ
        if (notepadImage != null && data.notepadSprite != null)
        {
            notepadImage.sprite = data.notepadSprite;
        }
        if (contentText != null)
        {
            contentText.text = data.notepadContent;
        }

        // 2) ������§�Դ (�����)
        if (notepadAudioSource != null && paperOpenClip != null)
        {
            notepadAudioSource.PlayOneShot(paperOpenClip);
        }

        // 3) �ʴ���� Notepad �� Canvas
        notepadRect.gameObject.SetActive(true);

        // 4) �Դ Overlay + �� Fade-in
        if (overlayImage != null)
        {
            overlayImage.gameObject.SetActive(true);
            StartCoroutine(FadeOverlay(true));
        }

        // 5) ����͹ Notepad �ҡ�ʹ�ҹ��ҧ�ҷ���ҧ��
        StartCoroutine(MoveNotepad(offScreenPos, centerPos, true));
    }

    /// <summary>
    /// ���ʹ�Դ Notepad
    /// </summary>
    public void CloseNotepad()
    {
        // ��ҡ��ѧ����͹��������ѹ�Դ�������� ��� return
        if (isMoving || !isVisible) return;

        // ������§�Դ (�����)
        if (notepadAudioSource != null && paperCloseClip != null)
        {
            notepadAudioSource.PlayOneShot(paperCloseClip);
        }

        // Fade-out overlay
        if (overlayImage != null)
        {
            StartCoroutine(FadeOverlay(false));
        }

        // ����͹ Notepad �ҡ��ҧ�͡�Ѻŧ��ҧ
        StartCoroutine(MoveNotepad(centerPos, offScreenPos, false));
    }

    /// <summary>
    /// Coroutine ����͹ Notepad UI �ҡ startPos -> endPos
    /// </summary>
    private IEnumerator MoveNotepad(Vector2 startPos, Vector2 endPos, bool openState)
    {
        isMoving = true;
        notepadRect.anchoredPosition = startPos;

        float elapsed = 0f;
        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveTime);
            notepadRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        // ���������͹����
        isMoving = false;
        isVisible = openState;

        // ��һԴ���� ����͹ GameObject
        if (!openState)
        {
            notepadRect.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine �� Fade Overlay Panel
    /// fadeIn = true => �ҧ��Ҩ��֧ overlayAlphaTarget
    /// fadeIn = false => �ҧ�͡���֧ 0
    /// </summary>
    private IEnumerator FadeOverlay(bool fadeIn)
    {
        if (overlayImage == null) yield break;

        float startAlpha = overlayImage.color.a;
        float endAlpha = fadeIn ? overlayAlphaTarget : 0f;
        float elapsed = 0f;

        while (elapsed < overlayFadeTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / overlayFadeTime);
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);

            Color c = overlayImage.color;
            c.a = newAlpha;
            overlayImage.color = c;
            yield return null;
        }

        // ��Ҩҧ�͡�� 0 ���� ���Դ Overlay ����
        if (!fadeIn)
        {
            overlayImage.gameObject.SetActive(false);
        }
    }
}

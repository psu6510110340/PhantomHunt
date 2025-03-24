using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public AudioFader audioFader;
    public AudioSource uiAudioSource;
    public AudioClip startButtonClip;

    // �� pointer ��������Ҩҡ��¹͡
    public Texture2D customCursorTexture;
    // ��� hotSpot �� (0,0) ������¶֧��������¢ͧ�Ҿ
    public Vector2 hotSpot = Vector2.zero;

    private bool isGameStarting = false;

    void Start()
    {
        // ��駤�� custom cursor ���� hotSpot ��� (0, 0)
        Cursor.SetCursor(customCursorTexture, hotSpot, CursorMode.Auto);
    }

    public void PlayGame()
    {
        if (!isGameStarting)
        {
            isGameStarting = true;
            StartCoroutine(PlayStartSoundAndGo());
        }
    }

    private IEnumerator PlayStartSoundAndGo()
    {
        if (uiAudioSource != null && startButtonClip != null)
        {
            uiAudioSource.PlayOneShot(startButtonClip);
        }
        audioFader.FadeOutAndLoadMainHouseMap();
        yield break;
    }

    // �ѧ��ѹ����Ѻ���� Menu ���������§���Ǥ�������¹�չ
    public void GoToMenu()
    {
        if (!isGameStarting)
        {
            isGameStarting = true;
            StartCoroutine(PlayMenuSoundAndGo());
        }
    }

    private IEnumerator PlayMenuSoundAndGo()
    {
        if (uiAudioSource != null && startButtonClip != null)
        {
            uiAudioSource.PlayOneShot(startButtonClip);
        }
        audioFader.FadeOutAndLoadMenu();
        yield break;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

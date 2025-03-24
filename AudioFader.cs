using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioFader : MonoBehaviour
{
    // ��ҧ�ԧ AudioSource ����ͧ���Ŵ���§
    public AudioSource audioSource;
    // UI Image ����ͺ����˹�Ҩ�����Ѻ�� fade effect (������մ�)
    public Image fadeImage;
    // �������ҷ���ͧ������ fade (��� fade out ��� fade in)
    public float fadeDuration = 4f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // �ѧ��ѹ���¡������ fade out ������§���˹�Ҩ� ������Ŵ�չ "main house map"
    public void FadeOutAndLoadMainHouseMap()
    {
        StartCoroutine(FadeTransitionCoroutine("main house map"));
    }

    // �����ѧ��ѹ����Ѻ fade out ������Ŵ�չ "Menu"
    public void FadeOutAndLoadMenu()
    {
        StartCoroutine(FadeTransitionCoroutine("Menu"));
    }

    private IEnumerator FadeTransitionCoroutine(string sceneName)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        // ��Ǩ�ͺ��������� fadeImage �������������
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        // Fade out audio ������������״���Ѻ˹�Ҩ�
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / fadeDuration);

            // Ŵ�дѺ���§
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t);

            // ���������״���Ѻ˹�Ҩ�
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = t;
                fadeImage.color = c;
            }
            yield return null;
        }
        // �׹�ѹ�дѺ���§��Ф����״
        audioSource.volume = 0f;
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        // ��Ŵ�չ����
        SceneManager.LoadScene(sceneName);
        yield return null;

        // ���ѡ������ѧ��Ŵ�չ
        yield return new WaitForSeconds(0.1f);

        // Fade in �չ���� (Ŵ�����״�ҡ��������)
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / fadeDuration);
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = 1f - t;
                fadeImage.color = c;
            }
            yield return null;
        }
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }
}

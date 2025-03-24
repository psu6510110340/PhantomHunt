using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Lever : MonoBehaviour
{
    public float interactionDistance = 3f;
    public GameObject leverText;
    public string leverPullAnimName;
    public string containerCloseAnimName;

    [Header("Container Settings")]
    public GameObject container;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip leverPullSound;
    public AudioClip containerCloseSound;
    public AudioClip truckStartSound;

    [Header("Reference to GhostFilter")]
    [SerializeField] private GhostFilter ghostFilter;

    [SerializeField] private BloodPanelFlicker bloodPanelFlicker;

    private bool leverPulled = false;

    // ===== �����������ҧ�ԧ�֧ Notepad �Ѻ Book =====
    [Header("UI References")]
    public NotepadUIController notepadUI;
    public BookMover bookMover;

    private void Update()
    {
        // --- 1) ��� Notepad �Դ���� ���� BookMover �Դ���� => ���͹حҵ����� Lever ---
        if ((notepadUI != null && notepadUI.IsVisible) ||
            (bookMover != null && bookMover.IsVisible))
        {
            leverText.SetActive(false);
            return;
        }

        // --- 2) �ԧ Ray ���������С�á� Lever ---
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject.CompareTag("lever"))
            {
                // �ʴ���ͤ����͡��顴 E
                leverText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E) && !leverPulled)
                {
                    // �Դ�������͹��Ǣͧ����Ф�
                    CharController_Motor.canMove = false;

                    // ��� BookMover ������ => �ѧ�Ѻ�Դ�����͡
                    if (bookMover != null)
                    {
                        bookMover.ForceCloseAndLockBook();
                    }

                    Animator leverAnim = hit.collider.transform.root.GetComponent<Animator>();
                    if (leverAnim != null)
                    {
                        leverAnim.SetTrigger(leverPullAnimName);
                    }

                    if (audioSource && leverPullSound)
                    {
                        audioSource.PlayOneShot(leverPullSound);
                    }

                    if (container != null)
                    {
                        Animator containerAnim = container.GetComponent<Animator>();
                        if (containerAnim != null)
                        {
                            containerAnim.SetTrigger(containerCloseAnimName);
                        }

                        if (audioSource && containerCloseSound)
                        {
                            StartCoroutine(PlayContainerCloseAndTruckSequence());
                        }
                    }

                    leverPulled = true;
                }
            }
            else
            {
                leverText.SetActive(false);
            }
        }
        else
        {
            leverText.SetActive(false);
        }
    }

    private IEnumerator PlayContainerCloseAndTruckSequence()
    {
        audioSource.PlayOneShot(containerCloseSound);
        yield return new WaitForSeconds(containerCloseSound.length);

        if (truckStartSound)
        {
            audioSource.PlayOneShot(truckStartSound);
        }

        yield return new WaitForSeconds(5f);

        if (ghostFilter != null)
        {
            ghostFilter.StopHeartbeat();
        }

        if (bloodPanelFlicker != null)
        {
            bloodPanelFlicker.StopFlicker();
        }

        if (ghostFilter != null)
        {
            if (ghostFilter.isGhostGuessCorrect)
            {
                Debug.Log("Lever: �š�÷�¶١! ����¹�չ�� Win");
                SceneManager.LoadScene("Win");
            }
            else
            {
                Debug.Log("Lever: �š�÷�¼Դ! ����¹�չ�� Lose");
                SceneManager.LoadScene("Lose");
            }
        }
        else
        {
            Debug.LogWarning("Lever: ��辺�����ҧ�ԧ GhostFilter");
            SceneManager.LoadScene("Lose");
        }
    }
}

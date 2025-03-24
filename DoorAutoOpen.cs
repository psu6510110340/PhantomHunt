using UnityEngine;

public class DoorAutoOpen : MonoBehaviour
{
    public string doorOpenAnimName = "DoorOpen";
    public string doorCloseAnimName = "DoorClose";

    public float openDistance = 2f;  // ���з��͹حҵ����е��Դ
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    private Animator doorAnim;
    private bool isOpen = false;  // state ��һ�е��Դ�������ͻԴ

    void Start()
    {
        doorAnim = GetComponent<Animator>();
        if (doorAnim == null)
        {
            Debug.LogError("No Animator component found on door!");
        }
    }

    void Update()
    {
        // ���� Monster ������㹩ҡ����� tag "monster"
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
        bool shouldOpen = false;

        // ��Ǩ�ͺ Monster ���е�� ����յ���˹��������е��������
        foreach (GameObject monster in monsters)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance <= openDistance)
            {
                shouldOpen = true;
                break;
            }
        }

        // ����� Monster ��������е� ��л�е��ѧ�Դ���� ����Դ��е�
        if (shouldOpen && !isOpen)
        {
            if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorCloseAnimName))
            {
                doorAnim.ResetTrigger("close");
                doorAnim.SetTrigger("open");
                isOpen = true;

                if (audioSource && doorOpenSound)
                {
                    audioSource.PlayOneShot(doorOpenSound);
                }
            }
        }
        // �������� Monster ��������е� ��л�е��Դ���� ���Դ��е�
        else if (!shouldOpen && isOpen)
        {
            if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorOpenAnimName))
            {
                doorAnim.ResetTrigger("open");
                doorAnim.SetTrigger("close");
                isOpen = false;

                if (audioSource && doorCloseSound)
                {
                    audioSource.PlayOneShot(doorCloseSound);
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class BookOpenController : MonoBehaviour
{
    [Header("Prefab ������͹����¹��Ѻ�� Book (�Դ)")]
    public GameObject bookClosedPrefab;

    [Header("Prefab ����Ѻ BookWriten")]
    public GameObject bookWritenPrefab;

    [Header("Audio Settings")]
    public AudioClip paperWriteClip;  // ���§��¹��д��

    public GameObject SpawnClosedBook(Transform itemHolder)
    {
        // ����� BookOpen ��ǹ��
        Destroy(gameObject);

        // ���ҧ Book (�Դ) ���ͼ�����
        GameObject closedBook = Instantiate(bookClosedPrefab, itemHolder);
        closedBook.name = "Book";
        closedBook.tag = "Untagged"; // ���� ����ͧ�� Pickup
        closedBook.transform.localPosition = Vector3.zero;
        closedBook.transform.localRotation = Quaternion.identity;

        return closedBook;
    }

    // �ѧ��ѹ����� Timer ��������¹�� BookWriten
    public void StartBookWritenTimer()
    {
        StartCoroutine(BookWritenCoroutine());
    }

    private IEnumerator BookWritenCoroutine()
    {
        // ����������㹪�ǧ 5-10 �Թҷ�
        yield return new WaitForSeconds(Random.Range(5f, 10f));

        // ���ҧ BookWriten �����˹���С����ع���ǡѺ BookOpen
        GameObject bookWriten = Instantiate(bookWritenPrefab, transform.position, transform.rotation);
        bookWriten.name = "BookWriten";
        // ����¹ tag ����������ö��Ժ�� (����� "Pickup")
        bookWriten.tag = "Untagged";

        // ������§��¹��д�ɷ����˹觹��
        if (paperWriteClip != null)
        {
            AudioSource.PlayClipAtPoint(paperWriteClip, transform.position);
        }

        // ����� BookOpen
        Destroy(gameObject);
    }
}

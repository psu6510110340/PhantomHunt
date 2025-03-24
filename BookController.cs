using UnityEngine;

public class BookController : MonoBehaviour
{
    [Header("Prefab ������͹����¹�� BookOpen")]
    public GameObject bookOpenPrefab;

    public void SpawnOpenBook(Vector3 dropPosition, Quaternion dropRotation)
    {
        // ����� Book (�Դ) ��ǹ���͹
        Destroy(gameObject);

        // ���ҧ BookOpen ŧ���
        GameObject openObj = Instantiate(bookOpenPrefab, dropPosition, Quaternion.Euler(270, 0, 90));
        openObj.name = "BookOpen";
        openObj.tag = "Pickup";  // ����ͧ�Ѻ�����Ժ��

        // �����͹�: player drop Book � Room ����Դ�˵ء�ó�
        // ��� Ghost ����������� Evidence GhostWriting
        GhostEventManager gem = FindObjectOfType<GhostEventManager>();
        if (gem != null && gem.ghostRoom != null)
        {
            Collider roomCollider = gem.ghostRoom.GetComponent<Collider>();
            if (roomCollider != null && roomCollider.bounds.Contains(dropPosition)
                && gem.chosenGhost != null && gem.chosenGhost.requiredEvidences.Contains(EvidenceType.GhostWriting))
            {
                // ������͹䢤ú ������¡ Timer � BookOpenController ��������¹�� BookWriten ��ѧ�ҡ 3 �Թҷ�
                BookOpenController boc = openObj.GetComponent<BookOpenController>();
                if (boc != null)
                {
                    boc.StartBookWritenTimer();
                }
            }
        }
    }
}

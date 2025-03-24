using UnityEngine;

public class UVFlashlightChecker : MonoBehaviour
{
    public Light uvLight;
    public float checkRadius = 5f;   // ���� Overlap
    public float spotAngle = 30f;    // ��������

    [SerializeField] private GhostEventManager ghostEventManager;

    private HandBloodController[] allBloods;

    void Start()
    {
        // �֧ HandBloodController ������㹫չ
        allBloods = FindObjectsOfType<HandBloodController>();

        // ����������������ҧ (alpha = 0)
        foreach (var b in allBloods)
        {
            b.SetVisible(false);
        }
    }

    void Update()
    {
        if (uvLight == null) return;

        // ��� UV �Դ => �����ҧ������
        if (!uvLight.enabled)
        {
            foreach (var b in allBloods)
            {
                b.SetVisible(false);
            }
            return;
        }

        // ��ͧ�ѹ null
        if (ghostEventManager == null || ghostEventManager.chosenGhost == null)
        {
            // ��������Ҽ��繵���˹ => �Դ HandBlood ����
            foreach (var b in allBloods)
            {
                b.SetVisible(false);
            }
            return;
        }

        // ������ chosenGhost �� EvidenceType.Fingerprint ���
        bool hasFingerprint = ghostEventManager
            .chosenGhost
            .requiredEvidences
            .Contains(EvidenceType.Fingerprint);

        // �������� Fingerprint => ���������ʹ������
        if (!hasFingerprint)
        {
            foreach (var b in allBloods)
            {
                b.SetVisible(false);
            }
            return;
        }

        // ����������� Fingerprint => ��ͧ�����������ͧ�ç�Ѻ ghostRoom ����
        Room theGhostRoom = ghostEventManager.ghostRoom;  // ������ getter

        // ǹ�� HandBlood �����ѹ
        foreach (var b in allBloods)
        {
            if (b == null)
            {
                continue;
            }

            // ========== ������ѹ������ͧ���������������� ==========
            // ����� HandBloodController �������١ (child) ����� GameObject �ͧ Room
            // ����Ҩ getComponentInParent<Room> ��Ẻ���:
            Room bloodRoom = b.GetComponentInParent<Room>();
            // ����������ͧ���ǡѺ�� => ���������
            if (bloodRoom != theGhostRoom)
            {
                b.SetVisible(false);
                continue;
            }

            // ========== ��ǹ���: ����������ͧ���� => �����ʧ UV ==========
            Vector3 dir = b.transform.position - transform.position;
            float dist = dir.magnitude;

            if (dist <= checkRadius)
            {
                float angle = Vector3.Angle(transform.forward, dir);
                if (angle <= spotAngle / 2f)
                {
                    // ⴹ� UV => Fade In
                    b.SetVisible(true);
                }
                else
                {
                    b.SetVisible(false);
                }
            }
            else
            {
                b.SetVisible(false);
            }
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI; // ����� TextMeshPro �������¹�� using TMPro;

public class InteractNotepad : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public KeyCode interactKey = KeyCode.E;

    [Header("Notepad UI")]
    public NotepadUIController notepadUI;
    public BookMover bookMover;

    [Header("UI Message")]
    public TextMeshProUGUI interactMessage;
    // ������ TextMeshProUGUI ᷹�� ��:
    // public TextMeshProUGUI interactMessage;

    void Update()
    {
        // ��� Notepad �Դ���� => �� E �Դ�����
        if (notepadUI.IsVisible)
        {
            // ��͹��ͤ��� (���ͤ�ҧ����)
            if (interactMessage != null)
            {
                interactMessage.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(interactKey))
            {
                notepadUI.CloseNotepad();
            }
            return;
        }

        // ��� BookMover �Դ���� => �����Դ Notepad
        if (bookMover != null && bookMover.IsVisible)
        {
            // ��͹��ͤ���
            if (interactMessage != null)
            {
                interactMessage.gameObject.SetActive(false);
            }
            return;
        }

        // �ԧ Ray ������ Notepad
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            if (hit.collider.CompareTag("Notepad"))
            {
                // �ҡ�� Notepad => ����ͤ��� "Press E to read note"
                if (interactMessage != null)
                {
                    interactMessage.text = "Press 'E' to Read Note";
                    interactMessage.gameObject.SetActive(true);
                }

                // ����ͼ����蹡� E
                if (Input.GetKeyDown(interactKey))
                {
                    var noteData = hit.collider.GetComponent<Notepad3DObject>();
                    if (noteData != null)
                    {
                        notepadUI.OpenNotepad(noteData);
                    }
                }
            }
            else
            {
                // ��� Raycast �����ҧ��� => ��͹��ͤ���
                if (interactMessage != null)
                {
                    interactMessage.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // ��� Raycast ���ⴹ���� => ��͹��ͤ���
            if (interactMessage != null)
            {
                interactMessage.gameObject.SetActive(false);
            }
        }
    }
}

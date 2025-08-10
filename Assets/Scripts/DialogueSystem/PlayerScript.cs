using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public float interactDistance = 3f;          
    public DialogueSystem dialogueSystem;        
    private NPCscript nearestNPC = null;
    public GameObject hintUI;

    public float cancelDialogueDistance = 6f;


    void Update()
    {
        FindNearestNPC();

        if (nearestNPC != null)
        {
            hintUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!dialogueSystem.isDialogueActive)
                {
                    Debug.Log("������ ������� E � ��������� ������");
                    
                    hintUI.SetActive(false);
                }
                else
                {
                    // E ���� ������� ������
                    dialogueSystem.ShowNextMessage();
                }
            }
        }
        else
        {
            hintUI.SetActive(false);
        }

        if (dialogueSystem.isDialogueActive && nearestNPC != null)
        {
            float dist = Vector3.Distance(transform.position, nearestNPC.transform.position);
            if (dist > cancelDialogueDistance)
            {
                Debug.Log("����� ������ ������� ������ � ������ ��������.");
                dialogueSystem.EndDialogue();
            }
        }



    }


    void FindNearestNPC()
    {
        NPCscript[] allNPCs = GameObject.FindObjectsByType<NPCscript>(FindObjectsSortMode.None);
        Debug.Log("������� NPC: " + allNPCs.Length);

        float closestDist = interactDistance;
        nearestNPC = null;

        foreach (NPCscript npc in allNPCs)
        {
            float dist = Vector3.Distance(transform.position, npc.transform.position);
            Debug.Log($"NPC: {npc.name}, ����������: {dist}");

            if (dist <= closestDist)
            {
                closestDist = dist;
                nearestNPC = npc;
            }
        }

        if (nearestNPC != null)
            Debug.Log("��������� NPC: " + nearestNPC.name);
        else
            Debug.Log("NPC ����� �� ������!");
    }



}



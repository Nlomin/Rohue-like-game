using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [Header("ScriptableObject � ������� � ��������")]
    public Item item;
    public GameObject floatingGoldTextPrefab;

    private void OnTriggerEnter(Collider other)
    {
        // ��������, ��� � ������ ��� Player
        if (other.CompareTag("Player"))
        {
            // �������� ������ ������
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // ��������� ������
                item.ApplyEffect(player);

                // ���������� �������, ����� ������ ���� ����� ��� ��������
                Destroy(gameObject);
            }
        }
    }
}

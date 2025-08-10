using UnityEngine;

public interface IInteractable
{
    Transform GetTransform();          // ��� ����������
    void Interact();                   // ��� ���������� ��� ��������������
    void ShowInteractIcon(string keyText);     // �������� ���������
    void HideInteractIcon();                   // ������ ���������


}


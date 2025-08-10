using UnityEngine;
using UnityEngine.Events;

public class HitEvent : MonoBehaviour
{
    [System.Serializable]
    public class HitUnityEvent : UnityEvent<GameObject, int> { }

    public HitUnityEvent OnHit; // �������, ���������� ��� ���������

    public void InvokeHit(GameObject target, int damage)
    {
        OnHit?.Invoke(target, damage);
    }
}

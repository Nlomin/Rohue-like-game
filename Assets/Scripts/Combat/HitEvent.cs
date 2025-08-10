using UnityEngine;
using UnityEngine.Events;

public class HitEvent : MonoBehaviour
{
    [System.Serializable]
    public class HitUnityEvent : UnityEvent<GameObject, int> { }

    public HitUnityEvent OnHit; // Событие, вызываемое при попадании

    public void InvokeHit(GameObject target, int damage)
    {
        OnHit?.Invoke(target, damage);
    }
}

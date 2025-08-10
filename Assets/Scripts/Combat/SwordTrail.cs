using UnityEngine;

public class SwordTrail : MonoBehaviour
{
    public TrailRenderer trail;
    private void Start()
    {
        DisableTrail();
    }
    public void EnableTrail()
    {
        trail.emitting = true;
    }

    public void DisableTrail()
    {
        trail.emitting = false;
    }
}

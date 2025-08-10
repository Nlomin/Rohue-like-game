using UnityEngine;

public class BasikWeapom : MonoBehaviour
{
    [Range(1, 50)]
    public int damage = 1;
    public Transform firePoint;

    public void shot()
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 1000))
        {
            BasikHP hp = hit.transform.GetComponent<BasikHP>();

            if (hp != null)
                hp.hpDecrease(damage);
        }
    
    }
}

using UnityEngine;

public class SphereRotator : MonoBehaviour
{
    public Transform bossTransform;
    public Vector3 offset = Vector3.zero; 
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.position = bossTransform.position + offset;
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}


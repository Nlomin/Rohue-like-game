using Unity.Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("�������� ����������")]
    public Transform target; // �����
    public Vector3 offset = new Vector3(0, 10, -10); // �������� ������ �� ������
    public float followSpeed = 5f; // �������� ����������

    [Header("�������")]
    public CinemachineImpulseSource impulseSource;

    [Header("���������������")]
    private bool isInspecting = false;
    private bool returningToDefault = false;

    private Vector3 inspectTargetPos;
    private Quaternion inspectTargetRot;
    private float inspectMoveSpeed;

    private Quaternion defaultRotation;
    private Vector3 originalOffset;

    
    private void Start()
    {
        defaultRotation = transform.rotation;
        originalOffset = offset;
    }

    public void TriggerShake()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }

    private void LateUpdate()
    {
        if (isInspecting)
        {
            // ��������� � ������� �������
            transform.position = Vector3.Lerp(transform.position, inspectTargetPos, Time.deltaTime * inspectMoveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, inspectTargetRot, Time.deltaTime * inspectMoveSpeed);
        }
        else if (returningToDefault && target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, 0, target.position.z) + originalOffset;

            

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * followSpeed);

            if (Vector3.Distance(transform.position, desiredPosition) < 0.05f &&
                Quaternion.Angle(transform.rotation, defaultRotation) < 1f)
            {
                returningToDefault = false;
            }
        }

        else if (target != null)
        {
            // ������� ����������
            Vector3 desiredPosition = new Vector3(target.position.x, 0, target.position.z) + offset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }

    public void MoveToTarget(Vector3 position, Quaternion rotation, float speed)
    {
        isInspecting = true;
        returningToDefault = false;
        inspectTargetPos = position;
        inspectTargetRot = rotation;
        inspectMoveSpeed = speed;
    }

    // ����� �� ������ �������
    public void ReturnToPlayer(float speed)
    {
        isInspecting = false;
        returningToDefault = true;
        followSpeed = speed;
    }
}




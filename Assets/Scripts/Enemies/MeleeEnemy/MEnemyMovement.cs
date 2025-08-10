using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class MEnemyMovement : MonoBehaviour, IChase
{
    public Transform[] waypoints;
    private int ind = 0;
    private NavMeshAgent agent;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectRadius = 10f;
    private Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    public void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[ind].position) < 2f)
        {
            ind = (ind + 1) % waypoints.Length;
        }
        MoveTo(waypoints[ind].position);
    }

    public bool CanSeePlayer(out Transform playerTransform)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);
        if (colliders.Length > 0)
        {
            playerTransform = colliders[0].transform;
            return true;
        }
        playerTransform = null;
        return false;
    }

    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }
}


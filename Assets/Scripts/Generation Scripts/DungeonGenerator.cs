using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    public void GenerateDungeon()
    {
        navMeshSurface.BuildNavMesh(); // ������ bake
    }

    public void RemoveAndRebake()
    {
        navMeshSurface.RemoveData();
        
        StartCoroutine(DelayedRebake());
    }

    private IEnumerator DelayedRebake()
    {
        yield return null; // ��������� �� ����� �����
        navMeshSurface.BuildNavMesh();
    }
}


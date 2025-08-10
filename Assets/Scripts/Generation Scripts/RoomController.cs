using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.Cinemachine;


public class RoomController : MonoBehaviour
{
    [Header("Настройки спавна")]
    public Transform[] spawnPoints;      // Несколько точек спавна удобнее, чем одна
    public GameObject[] enemyPrefabs;    // Набор префабов врагов

    public bool hasTriggered = false;   // Игрок уже вошёл
    private bool enemiesDefeated = false; // Все враги побеждены
    private DoorController doorCtr;

    // --- Счётчик врагов ---
    private readonly List<GameObject> spawnedEnemies = new List<GameObject>();
    private int enemiesRemaining = 0;


    public float moveSpeed = 5f;
    private MergedRoomController mergedRoom;

    public GameObject minimapCellPrefab;
    private GameObject minimapCellInstance;

    public Collider roomBounds;

    [Range(1f, 20f)]
    public float spawnRadius = 5f;

    [Range(1, 10)]
    public int enemiesMaxCount = 5;
    [Range(1, 10)]
    public int enemiesMinCount = 2;
    private void Awake()
    { 
        doorCtr = FindAnyObjectByType<DoorController>();
        
    }
    private void Start()
    {
        if (roomBounds == null)
        {
            roomBounds = GetComponentInChildren<Collider>();
        }
        if (minimapCellPrefab != null)
        {
            minimapCellInstance = Instantiate(minimapCellPrefab, transform.position + Vector3.up * 10f, Quaternion.identity);
            minimapCellInstance.transform.localPosition = transform.position + Vector3.up * 15f;
            minimapCellInstance.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var confiner = FindAnyObjectByType<CinemachineConfiner3D>();
            if (confiner != null && roomBounds != null)
            {
                confiner.BoundingVolume = roomBounds;
            }

            
        }
        if (hasTriggered || enemiesDefeated) return;
        
        if (other.CompareTag("Player"))
        {
            
            MergedRoomController merged = GetComponentInParent<MergedRoomController>();
            if (merged != null)
            {
                merged.SpawnEnemiesInAllRooms();
            }
            else
            {
                SpawnEnemies(); // если это одиночная комната
            }

            Debug.Log("Doors are CLOSED");
        }
    }

    public void SpawnEnemies()
    {
        hasTriggered = true;
        if (minimapCellInstance != null)
        {
            var sr = minimapCellInstance.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color32(203, 141, 91, 255);
            }
        }
        SummonWithEffect summonUtil = FindFirstObjectByType<SummonWithEffect>();

        int count = Random.Range(enemiesMinCount, enemiesMaxCount); 
        enemiesRemaining = count;       
        Debug.Log($"Запланировано заспавнить врагов: {enemiesRemaining}");
        if(enemyPrefabs.Length != 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

                Vector3 spawnPosition = Vector3.zero;
                bool validPositionFound = false;
                int attempts = 10;

                while (!validPositionFound && attempts-- > 0)
                {
                    Vector2 offset2D = Random.insideUnitCircle * spawnRadius;
                    Vector3 randomPosition = point.position + new Vector3(offset2D.x, 0, offset2D.y);

                    if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
                    {
                        spawnPosition = hit.position;
                        validPositionFound = true;
                    }
                }

                if (validPositionFound)
                {
                    StartCoroutine(summonUtil.Summon(spawnPosition, prefab, (enemy) =>
                    {
                        spawnedEnemies.Add(enemy);
                        MEnemyStats stats = enemy.GetComponentInChildren<MEnemyStats>();
                        if (stats != null)
                        {
                            stats.roomOwner = this;
                            stats.OnDeath.AddListener(NotifyEnemyDead);
                        }

                        if (mergedRoom != null)
                        {
                            mergedRoom.RegisterEnemyCount(1);
                        }
                    }));
                }
            }
            doorCtr.RaiseAllDoors();
        }
        
        
        
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null)
            return;

        Gizmos.color = Color.yellow;
        foreach (Transform point in spawnPoints)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, spawnRadius);
            }
        }
    }

    public void NotifyEnemyDead()
    {
        enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);
        Debug.Log($"Осталось врагов: {enemiesRemaining}");

        if (mergedRoom != null)
        {
            mergedRoom.OnEnemyDefeated(); // Сообщаем родителю
        }

        if (enemiesRemaining == 0 && !enemiesDefeated && mergedRoom == null)
        {
            enemiesDefeated = true;
            doorCtr.LowerAllDoors();
            Debug.Log("Все враги повержены, двери открыты!");
        }
    }
    public void SetMergedRoom(MergedRoomController parent)
    {
        mergedRoom = parent;
    }

    public int GetEnemyCount()
    {
        return spawnedEnemies.Count;
    }

    public void OpenDoors()
    {
        if (!enemiesDefeated)
        {
            enemiesDefeated = true;
            doorCtr.LowerAllDoors();
            Debug.Log("MergedRoom открыл двери в комнате");
        }
    }

}






using UnityEngine;
using System.Collections;
using System;

public class SummonWithEffect : MonoBehaviour
{
    public GameObject summonCirclePrefab;
    public GameObject flashPrefab;

    public float circleTime = 1.5f;
    public float flashDelay = 0.2f;
    public float spawnDelay = 0.1f;

    public IEnumerator Summon(Vector3 position, GameObject enemyPrefab, Action<GameObject> onSpawned = null)
    {
        Vector3 spawnGroundPos = GetGroundPosition(position);

        // 1. Круг
        GameObject circle = Instantiate(summonCirclePrefab, spawnGroundPos, Quaternion.Euler(90f, 0f, 0f));

        SummonCircle fill = circle.GetComponent<SummonCircle>();
        if (fill != null)
            yield return fill.Fill(circleTime);
        else
            yield return new WaitForSeconds(circleTime);

        // 2. Вспышка —  теперь на основе позиции пола
        Vector3 flashPos = spawnGroundPos + Vector3.up * 1f;
        GameObject flash = Instantiate(flashPrefab, flashPos, Quaternion.identity);
        Destroy(flash, 1f);
        yield return new WaitForSeconds(flashDelay);

        // 3. Враг
        GameObject enemy = Instantiate(enemyPrefab, spawnGroundPos, Quaternion.identity);
        onSpawned?.Invoke(enemy);

        yield return new WaitForSeconds(spawnDelay);
        Destroy(circle);
    }



    private Vector3 GetGroundPosition(Vector3 originalPosition)
    {
        Vector3 origin = originalPosition + Vector3.up * 5f; // чтобы не застрял в полу
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }

        return originalPosition; // fallback
    }
}


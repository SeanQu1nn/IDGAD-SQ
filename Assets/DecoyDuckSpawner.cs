using UnityEngine;

public class DecoyDuckSpawner : MonoBehaviour
{
    public GameObject[] decoyPrefabs;
    public float CurrentSpawnRate = 3.0f;

    public Vector2 spawnXRange = new Vector2(-5f, 5f);
    public Vector2 spawnYRange = new Vector2(-3f, 3f);

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= CurrentSpawnRate)
        {
            timer = 0f;
            Spawn();
        }
    }

    public void SetSpawnRate(float rate)
    {
        CurrentSpawnRate = Mathf.Max(rate, 1.8f);
    }

    private void Spawn()
    {
        if (decoyPrefabs.Length == 0) return;

        GameObject prefab = decoyPrefabs[Random.Range(0, decoyPrefabs.Length)];
        Vector3 pos = new Vector3(
            Random.Range(spawnXRange.x, spawnXRange.y),
            Random.Range(spawnYRange.x, spawnYRange.y),
            0
        );

        Instantiate(prefab, pos, Quaternion.identity);
    }
}

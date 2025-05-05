using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance { get; private set; }

    public GameObject enemyPrefab;
    public Vector3 initialSpawnPosition = new Vector3(10, 0.5f, 10);
    public float spawnInterval = 15f;

    private float timer = 0f;
    private int initialCount = 5;
    private List<Enemy> enemyList = new List<Enemy>();

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start() {
        for (int i = 0; i < initialCount; i++) {
            TrySpawnEnemy(initialSpawnPosition + new Vector3(i * 1.5f, 0f, 0f));
        }
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= spawnInterval) {
            timer -= spawnInterval;
            TrySpawnEnemy(initialSpawnPosition + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)));
        }

        ApplyEnemyRepulsion();
    }

    void TrySpawnEnemy(Vector3 position) {
        Debug.Log("TrySpawnEnemy call");
        if (enemyPrefab == null) { Debug.LogWarning("EnemyManager: enemyPrefab is not assigned."); return; }
        GameObject obj = Instantiate(enemyPrefab, position, Quaternion.identity);
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null) { enemyList.Add(enemy); }
    }

    void ApplyEnemyRepulsion(float repulsionRadius = 1.5f, float strength = 5f) {
        for (int i = 0; i < enemyList.Count; i++) {
            for (int j = 0; j < enemyList.Count; j++) {
                if (i == j) continue;
                Enemy e1 = enemyList[i];
                Enemy e2 = enemyList[j];
                Vector3 dir = e1.transform.position - e2.transform.position;
                float dist = dir.magnitude;
                if (dist < repulsionRadius && dist > 0.01f) {
                    Vector3 push = dir.normalized * (repulsionRadius - dist) * strength * Time.deltaTime;
                    e1.transform.position += push;
                }
            }
        }
    }

    public void RemoveEnemy(Enemy enemy) {
        if (enemyList.Contains(enemy)) enemyList.Remove(enemy);
    }

    public List<Enemy> GetEnemyList() {
        return enemyList;
    }
}
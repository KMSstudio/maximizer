
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager {
    public int playerNumber;
    private List<Army> armyList = new List<Army>();

    public ArmyManager(int playerNumber) {
        this.playerNumber = playerNumber;
    }

    public void TrySpawnArmy(GameObject armyPrefab, Vector3 position) {
        GameObject obj = GameObject.Instantiate(armyPrefab, position, Quaternion.identity);
        Army army = obj.GetComponent<Army>();
        if (army != null) {
            armyList.Add(army);
            army.type = (playerNumber == 0) ? Army.ArmyType.Entity : Army.ArmyType.Enemy;
        }
    }

    public void ApplyRepulsion(float repulsionRadius = 1.5f, float strength = 5f) {
        for (int i = 0; i < armyList.Count; i++) {
            for (int j = 0; j < armyList.Count; j++) {
                if (i == j) continue;
                Army a1 = armyList[i];
                Army a2 = armyList[j];
                Vector3 dir = a1.transform.position - a2.transform.position;
                float dist = dir.magnitude;
                if (dist < repulsionRadius && dist > 0.01f) {
                    Vector3 push = dir.normalized * (repulsionRadius - dist) * strength * Time.deltaTime;
                    a1.transform.position += push;
                }
            }
        }
    }

    public Army GetNearestEnemy(Vector3 position, List<Army> others) {
        Army closest = null;
        float closestDist = float.MaxValue;

        foreach (Army other in others) {
            float dist = Vector3.Distance(position, other.transform.position);
            if (dist < closestDist) {
                closestDist = dist;
                closest = other;
            }
        }
        return closest;
    }

    public List<Army> GetArmyList() => armyList;
}

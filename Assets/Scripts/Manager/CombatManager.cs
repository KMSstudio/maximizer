using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {
    public static CombatManager Instance { get; private set; }

    public GameObject armyPrefab;

    public RectTransform selectionBoxUI;
    public Canvas canvas;
    public LayerMask groundMask;
    public LayerMask entityMask;

    public List<ArmyManager> armyManagers = new List<ArmyManager>();
    public PlayerManager playerManager => armyManagers[0] as PlayerManager;

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        var playerManager = new PlayerManager(0, groundMask, entityMask, selectionBoxUI, canvas);
        armyManagers.Add(playerManager);
        armyManagers.Add(new ArmyManager(1));
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            Vector3 spawnPosition = new Vector3(0, 0.5f, 0);
            playerManager.TrySpawnArmy(armyPrefab, spawnPosition);
        }

        if (Input.GetMouseButtonDown(0)) playerManager.OnMouseDown();
        if (Input.GetMouseButton(0)) playerManager.OnMouseDrag();
        if (Input.GetMouseButtonUp(0)) playerManager.OnMouseUp();

        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask)) {
                playerManager.HandleMoveCommand(hit.point);
            }
        }

        foreach (ArmyManager manager in armyManagers) {
            manager.ApplyRepulsion();
        }
    }

    public void AssignNearestTargets() {
        foreach (ArmyManager manager in armyManagers) {
            foreach (Army self in manager.GetArmyList()) {
                List<Army> enemies = new List<Army>();
                foreach (ArmyManager otherManager in armyManagers) {
                    if (otherManager == manager) continue;
                    enemies.AddRange(otherManager.GetArmyList());
                }
                Army nearest = manager.GetNearestEnemy(self.transform.position, enemies);
                if (nearest != null) {
                    // later: self.SetTarget(nearest);
                }
            }
        }
    }
}

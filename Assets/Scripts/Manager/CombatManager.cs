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

        Vector3[] spawnPositions = {
            new Vector3(10, 0.5f, 10),
            new Vector3(10, 0.5f, -10),
            new Vector3(-10, 0.5f, -10),
            new Vector3(-10, 0.5f, 10)
        };
        foreach (Vector3 pos in spawnPositions) {
            armyManagers[1].TrySpawnArmy(armyPrefab, pos);
        }
    }

    void OnEnable() {
        Army.OnArmyMoveComplete += HandleSingleTargeting;
    }

    void OnDisable() {
        Army.OnArmyMoveComplete -= HandleSingleTargeting;
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

    /// <summary>
    /// Find closest enemy army and set it as the target.
    /// </summary>
    /// <param name="army">The army that finished moving.</param>
    /// <param name="playerNumber">The player number of the army.</param>
    void HandleSingleTargeting(Army army, int playerNumber) {
        Vector3 pos = army.transform.position;
        Army nearest = null;
        float minDist = float.MaxValue;
        foreach (var manager in armyManagers) {
            if (manager.playerNumber == playerNumber) continue;
            Army candidate = manager.GetNearestEnemy(pos);
            if (candidate == null) continue;
            float dist = Vector3.Distance(pos, candidate.transform.position);
            if (dist < minDist) { minDist = dist; nearest = candidate; }
        }
        if (nearest != null) { army.SetTarget(nearest); }
    }

    private ArmyManager managerWith(int playerNumber) {
        foreach (var m in armyManagers)
            if (m.playerNumber == playerNumber)
                return m;
        return null;
    }
}
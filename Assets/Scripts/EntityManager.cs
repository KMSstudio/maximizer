using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityManager : MonoBehaviour {
    public static EntityManager Instance { get; private set; }

    [Header("Entity Control")]
    public GameObject entityPrefab;
    private List<Entity> entityList = new List<Entity>();
    private List<Entity> selectedEntities = new List<Entity>();

    [Header("Selection")]
    public LayerMask groundMask;
    public LayerMask entityMask;
    public RectTransform selectionBoxUI;
    public Canvas canvas;
    private Vector2 startMousePos;
    private Vector2 endMousePos;

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) { TrySpawnEntity(); }

        ApplyEntityRepulsion();

        if (Input.GetMouseButtonDown(0)) {
            startMousePos = Input.mousePosition;
            selectionBoxUI.gameObject.SetActive(true);
            TrySelectSingleEntity();
        } else if (Input.GetMouseButton(0)) {
            endMousePos = Input.mousePosition;
            DrawSelectionBox();
        } else if (Input.GetMouseButtonUp(0)) {
            selectionBoxUI.gameObject.SetActive(false);
            SelectUnitsInBox();
        }

        if (Input.GetMouseButtonDown(1) && selectedEntities.Count > 0) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask)) {
                foreach (Entity entity in selectedEntities) {
                    entity.MoveTo(hit.point);
                }
            }
        }
    }

    void TrySpawnEntity() {
        Debug.Log("TrySpawnEntity call");
        if (entityPrefab == null) { Debug.LogWarning("EntityManager: entityPrefab is not assigned."); return; }
        Vector3 spawnPosition = new Vector3(0, 0.5f, 0);
        GameObject obj = Instantiate(entityPrefab, spawnPosition, Quaternion.identity);
        Entity entity = obj.GetComponent<Entity>();
        if (entity != null) { entityList.Add(entity); }
    }

    void ApplyEntityRepulsion(float repulsionRadius = 1.5f, float strength = 5f) {
        for (int i = 0; i < entityList.Count; i++) {
            for (int j = 0; j < entityList.Count; j++) {
                if (i == j) continue;
                Entity e1 = entityList[i];
                Entity e2 = entityList[j];
                Vector3 dir = e1.transform.position - e2.transform.position;
                float dist = dir.magnitude;
                if (dist < repulsionRadius && dist > 0.01f) {
                    Vector3 push = dir.normalized * (repulsionRadius - dist) * strength * Time.deltaTime;
                    e1.transform.position += push;
                }
            }
        }
    }

    void TrySelectSingleEntity() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, entityMask)) {
            bool isCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            if (!isCtrl) { ClearSelection(); }
            Entity entity = hit.collider.GetComponent<Entity>();
            if (entity != null) {
                entity.isSelected = true;
                if (!selectedEntities.Contains(entity)) selectedEntities.Add(entity);
            }
        }
    }

    void DrawSelectionBox() {
        Vector2 start = startMousePos;
        Vector2 end = Input.mousePosition;
        Vector2 size = new Vector2(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y));
        Vector2 lowerLeft = new Vector2(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
        selectionBoxUI.position = lowerLeft;
        selectionBoxUI.sizeDelta = size;
    }

    void SelectUnitsInBox() {
        Debug.Log("SelectUnitsInBox call");
        Vector2 min = Vector2.Min(startMousePos, endMousePos);
        Vector2 max = Vector2.Max(startMousePos, endMousePos);
        Vector2 size = max - min;

        if (size.x * size.y < 10f) return;

        bool isCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (!isCtrl) { ClearSelection(); }

        foreach (Entity entity in entityList) {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(entity.transform.position);
            if (screenPos.z >= 0 &&
                screenPos.x >= min.x && screenPos.x <= max.x &&
                screenPos.y >= min.y && screenPos.y <= max.y) {
                entity.isSelected = true;
                if (!selectedEntities.Contains(entity)) { selectedEntities.Add(entity); }
            }
        }
    }

    void ClearSelection() {
        foreach (Entity entity in selectedEntities) {
            entity.isSelected = false;
        }
        selectedEntities.Clear();
    }

    public void RemoveEntity(Entity entity) {
        if (entityList.Contains(entity)) entityList.Remove(entity);
        if (selectedEntities.Contains(entity)) selectedEntities.Remove(entity);
    }
}
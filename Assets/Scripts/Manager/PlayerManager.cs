
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : ArmyManager {
    private List<Army> selectedArmies = new List<Army>();

    private Vector2 startMousePos;
    private Vector2 endMousePos;

    private LayerMask groundMask;
    private LayerMask entityMask;
    private RectTransform selectionBoxUI;
    private Canvas canvas;

    public PlayerManager(int playerNumber, LayerMask groundMask, LayerMask entityMask, RectTransform selectionBoxUI, Canvas canvas)
        : base(playerNumber) {
        this.groundMask = groundMask;
        this.entityMask = entityMask;
        this.selectionBoxUI = selectionBoxUI;
        this.canvas = canvas;
    }

    public void TrySelectSingleEntity() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, entityMask)) {
            bool isCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            if (!isCtrl) { ClearSelection(); }
            Army army = hit.collider.GetComponent<Army>();
            if (army != null) {
                army.isSelected = true;
                if (!selectedArmies.Contains(army)) selectedArmies.Add(army);
            }
        }
    }

    public void DrawSelectionBox() {
        Vector2 start = startMousePos;
        Vector2 end = Input.mousePosition;
        Vector2 size = new Vector2(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y));
        Vector2 lowerLeft = new Vector2(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
        selectionBoxUI.position = lowerLeft;
        selectionBoxUI.sizeDelta = size;
    }

    public void SelectUnitsInBox() {
        Vector2 min = Vector2.Min(startMousePos, endMousePos);
        Vector2 max = Vector2.Max(startMousePos, endMousePos);
        Vector2 size = max - min;

        if (size.x * size.y < 10f) return;

        bool isCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (!isCtrl) { ClearSelection(); }

        foreach (Army army in GetArmyList()) {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(army.transform.position);
            if (screenPos.z >= 0 &&
                screenPos.x >= min.x && screenPos.x <= max.x &&
                screenPos.y >= min.y && screenPos.y <= max.y) {
                army.isSelected = true;
                if (!selectedArmies.Contains(army)) selectedArmies.Add(army);
            }
        }
    }

    public void ClearSelection() {
        foreach (Army army in selectedArmies) {
            army.isSelected = false;
        }
        selectedArmies.Clear();
    }

    public void OnMouseDown() {
        startMousePos = Input.mousePosition;
        selectionBoxUI.gameObject.SetActive(true);
        TrySelectSingleEntity();
    }

    public void OnMouseDrag() {
        endMousePos = Input.mousePosition;
        DrawSelectionBox();
    }

    public void OnMouseUp() {
        selectionBoxUI.gameObject.SetActive(false);
        SelectUnitsInBox();
    }

    public void HandleMoveCommand(Vector3 position) {
        foreach (Army army in selectedArmies) {
            army.MoveTo(position);
        }
    }

    public List<Army> GetSelectedArmies() => selectedArmies;
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour {
    public LayerMask groundMask;
    public LayerMask entityMask;

    public RectTransform selectionBoxUI;
    public Canvas canvas;

    private Vector2 startMousePos;
    private Vector2 endMousePos;

    private List<Entity> selectedEntities = new List<Entity>();

    void Update() {
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

    void TrySelectSingleEntity() {
        Debug.Log("TrySelectSingleEntity call");
        
        bool isCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (!isCtrl) { ClearSelection(); }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, entityMask)) {
            Entity clickedEntity = hit.collider.GetComponent<Entity>();
            if (clickedEntity != null) {
                clickedEntity.isSelected = true;
                if (!selectedEntities.Contains(clickedEntity)) { selectedEntities.Add(clickedEntity); }
            }
        }
    }

    void DrawSelectionBox() {
        Vector2 start = startMousePos;
        Vector2 end = Input.mousePosition;

        Vector2 size = new Vector2(
            Mathf.Abs(end.x - start.x),
            Mathf.Abs(end.y - start.y)
        );

        Vector2 lowerLeft = new Vector2(
            Mathf.Min(start.x, end.x),
            Mathf.Min(start.y, end.y)
        );

        selectionBoxUI.position = lowerLeft;
        selectionBoxUI.sizeDelta = size;
    }

    void SelectUnitsInBox() {
        Debug.Log("SelectUnitsInBox call");

        Vector2 min = Vector2.Min(startMousePos, endMousePos);
        Vector2 max = Vector2.Max(startMousePos, endMousePos);
        Vector2 size = max - min;

        float area = size.x * size.y;
        Debug.Log($"SelectionBox size = ({size.x}, {size.y}), area = {area}");
        if (area < 10f) { return; }

        bool isCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (!isCtrl) { ClearSelection(); }
        
        foreach (Entity entity in FindObjectsOfType<Entity>()) {
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
        Debug.Log("ClearSelection call");
        foreach (Entity entity in selectedEntities) { entity.isSelected = false; }
        selectedEntities.Clear();
    }
}
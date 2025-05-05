using UnityEngine;

/// <summary>
/// Army 유닛의 렌더링 및 선택 상태 시각화를 담당합니다.
/// </summary>
public class ArmyController : Army {
    [Header("Color Settings")]
    public Color entityColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color enemyColor = Color.red;

    private Renderer rend;

    protected override void Start() {
        base.Start();
        rend = GetComponentInChildren<Renderer>();
        if (rend == null) { Debug.LogWarning("Renderer not found on " + gameObject.name); }
        else { UpdateColor(); }
    }

    protected override void Update() {
        base.Update();
        UpdateColor();
    }

    protected virtual void UpdateColor() {
        if (rend == null) { return; }
        if (type == ArmyType.Entity) { rend.material.color = isSelected ? selectedColor : entityColor; }
        else if (type == ArmyType.Enemy) { rend.material.color = enemyColor; }
    }
}
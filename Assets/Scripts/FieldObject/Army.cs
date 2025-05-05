using System;
using UnityEngine;

/// <summary>
/// Represents a controllable or enemy army unit.
/// Handles color, selection, and move-completion event.
/// </summary>
public class Army : GeneralArmy {
    public enum ArmyType { Entity, Enemy }

    [Header("Type & Color")]
    public ArmyType type = ArmyType.Entity;
    public Color entityColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color enemyColor = Color.red;

    [HideInInspector] public bool isSelected = false;

    public int playerNumber;

    public static event Action<Army, int> OnArmyMoveComplete;

    private Army currentTarget;

    protected override void UpdateColor() {
        if (rend == null) return;

        if (type == ArmyType.Entity) {
            rend.material.color = isSelected ? selectedColor : entityColor;
        } else if (type == ArmyType.Enemy) {
            rend.material.color = enemyColor;
        }
    }

    public override void MoveTo(Vector3 target) {
        Vector3 adjustedTarget = target + new Vector3(0f, 0.5f, 0f);
        StopAllCoroutines();
        StartCoroutine(MoveSmoothAndNotify(adjustedTarget));
    }

    private System.Collections.IEnumerator MoveSmoothAndNotify(Vector3 target) {
        while (Vector3.Distance(transform.position, target) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime);
            yield return null;
        }

        OnArmyMoveComplete?.Invoke(this, playerNumber);
    }

    public void SetTarget(Army target) {
        currentTarget = target;
        Debug.Log($"Target set to: {target.name} at position {target.transform.position}");
    }

    public Army GetTarget() => currentTarget;
}
using System;
using UnityEngine;

/// <summary>
/// General Class for Army. Someting Moving in FIeld
/// </summary>
public class Army : MonoBehaviour {
    public enum ArmyType { Entity, Enemy }

    [Header("Base Properties")]
    public ArmyType type = ArmyType.Entity;
    public int playerNumber;

    public bool isSelected = false;

    public static event Action<Army, int> OnArmyMoveComplete;

    private Army currentTarget;

    protected virtual void Start() {
        // ...
    }

    protected virtual void Update() {
        // ...
    }

    public virtual void MoveTo(Vector3 target) {
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

    public virtual void TakeDamage(float amount) {
        // ...
    }

    public virtual void SetTarget(Army target) {
        currentTarget = target;
        Debug.Log($"Target set to: {target.name} at position {target.transform.position}");
    }

    public Army GetTarget() => currentTarget;
}
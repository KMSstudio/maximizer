using UnityEngine;

public class Army : GeneralArmy {
    public enum ArmyType { Entity, Enemy }

    [Header("Type & Color")]
    public ArmyType type = ArmyType.Entity;
    public Color entityColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color enemyColor = Color.red;

    [HideInInspector] public bool isSelected = false;

    protected override void UpdateColor() {
        if (rend == null) return;

        if (type == ArmyType.Entity) {
            rend.material.color = isSelected ? selectedColor : entityColor;
        } else if (type == ArmyType.Enemy) {
            rend.material.color = enemyColor;
        }
    }
}
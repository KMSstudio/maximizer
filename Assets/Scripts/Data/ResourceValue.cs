using UnityEngine;

public enum ResourceType { Entity, Structure, Money }

[System.Serializable]
public class ResourceValue {
    public int entity;
    public int structure;
    public int money;

    public ResourceValue() { entity = 0; structure = 0; money = 0; }
    public ResourceValue(int entity, int structure, int money) {
        this.entity = entity; this.structure = structure; this.money = money;
    }

    public int this[ResourceType type] {
        get => type switch {
            ResourceType.Entity => entity,
            ResourceType.Structure => structure,
            ResourceType.Money => money,
            _ => 0
        };
        set {
            if (type == ResourceType.Entity) entity = value;
            else if (type == ResourceType.Structure) structure = value;
            else if (type == ResourceType.Money) money = value;
        }
    }

    public static ResourceValue operator +(ResourceValue a, ResourceValue b) => new(a.entity + b.entity, a.structure + b.structure, a.money + b.money);
    public static ResourceValue operator -(ResourceValue a, ResourceValue b) => new(a.entity - b.entity, a.structure - b.structure, a.money - b.money);

    public static ResourceValue operator *(float m, ResourceValue a) => new(Mathf.RoundToInt(m * a.entity), Mathf.RoundToInt(m * a.structure), Mathf.RoundToInt(m * a.money));
    public static ResourceValue operator *(ResourceValue a, float m) => m * a;

    public static bool operator >(ResourceValue a, ResourceValue b) => a.entity > b.entity && a.structure > b.structure && a.money > b.money;
    public static bool operator >=(ResourceValue a, ResourceValue b) => a.entity >= b.entity && a.structure >= b.structure && a.money >= b.money;
    public static bool operator <(ResourceValue a, ResourceValue b) => a.entity < b.entity && a.structure < b.structure && a.money < b.money;
    public static bool operator <=(ResourceValue a, ResourceValue b) => a.entity <= b.entity && a.structure <= b.structure && a.money <= b.money;
    public static bool operator ==(ResourceValue a, ResourceValue b) => a.entity == b.entity && a.structure == b.structure && a.money == b.money;
    public static bool operator !=(ResourceValue a, ResourceValue b) => !(a == b);

    public override bool Equals(object obj) => obj is ResourceValue other && this == other;
    public override int GetHashCode() => entity ^ structure ^ money;
    public override string ToString() => $"Entity: {entity}, Structure: {structure}, Money: {money}";
}
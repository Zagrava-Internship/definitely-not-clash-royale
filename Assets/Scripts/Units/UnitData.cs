using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Game/UnitData")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public GameObject prefab;
    public int health;
    public float speed;
    public UnitType type;
}

public enum UnitType
{
    Melee,
    Ranged,
    Flying
}
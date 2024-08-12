using UnityEngine;

[CreateAssetMenu(fileName = "New Fighter", menuName = "Combat/Fighter")]
public class FighterData : ScriptableObject
{
    public string FighterName;
    public float Health;
    public float AttackPower;
    public float AttackDuration;
    public float Armor;
    public float PreparationDuration;
    public GameObject FighterPrefab; // ������ ��� ������ (������ ��� ������)
    public float SpawnProbability;  // ����������� ������ (������ ��� ������)
}
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Persistence/Inventory")]
public class Inventory : ScriptableObject
{
    public int playerScore;
    public string playerName;
}

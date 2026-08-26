using UnityEngine;

public class Data : MonoBehaviour
{
    
    public static Data instance;
    public static int killCount = 0; // Total number of enemies killed
    public static int basicKillCount = 0; // Number of basic enemies killed
    public static int chargerKillCount = 0; // Number of charger enemies killed
    public static int shooterKillCount = 0; // Number of shooter enemies killed
    public static int tempKillCount = 0; // Temporary kill count for the current room
    public static int tempBasicKillCount = 0; // Temporary basic kill count for the current room
    public static int tempChargerKillCount = 0; // Temporary charger kill count for the current room
    public static int tempShooterKillCount = 0; // Temporary shooter kill count for the current room
    public static int totalCoinCount = 0; // Total number of coins collected
    public static int tempCoinCount = 0; // Temporary coin count for the current room
    public static int totalExperience = 0; // Total experience points collected

}

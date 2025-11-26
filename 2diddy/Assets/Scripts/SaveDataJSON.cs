using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float playerHealth;
    public float playerMaxHealth;
    public int experience;
    public int currentLevel;
    public int maxLevel;
    public float[] position;
    public List<int> weaponLevels;
    public int waveNumber;
    public float timerValue;
}

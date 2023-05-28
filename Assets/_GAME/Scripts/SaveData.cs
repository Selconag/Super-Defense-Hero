using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SaveData
{
    public int LevelIndex;
    public Vector3 PlayerPos;
    public bool TutorialPlayed;
    public List<EnemyData> Enemies;
}
[Serializable]
public class EnemyData
{
    public Enemy Enemy;
    public GameObject EnemyObject;
}
[Serializable]
public class PlayerData
{
    public Enemy Enemy;
    public GameObject EnemyObject;
}
[Serializable]
public class PlayerUpgrades
{

}
[Serializable]
public class WeaponTypes
{

}
[Serializable]
public class PlayerInventory
{

}
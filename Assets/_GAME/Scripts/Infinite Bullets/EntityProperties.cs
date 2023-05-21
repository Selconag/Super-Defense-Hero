using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityTypes { Enemy, Ally }

[CreateAssetMenu(fileName = "New Entity", menuName = "Entities/Humanoid", order = 1)]
public class EntityProperties : ScriptableObject
{
    [Tooltip("Defines how much health the enemy entity has.")]
    public int Health = 1;
    [Range(0, 10f)]
    [Tooltip("Defines the speed of the entity type.")]
    public float Speed = 1.0f;
    [Range(0, 1000f)]
    [Tooltip("Defines the experience of the entity give.")]
    public int Experience = 10;
    public EntityTypes EntityType;
    

}

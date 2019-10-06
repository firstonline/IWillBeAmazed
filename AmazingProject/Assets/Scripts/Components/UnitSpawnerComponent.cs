using UnityEngine;
using UnityEditor;
using Unity.Entities;

public struct UnitSpawnerComponent : IComponentData
{
	public int CountX;
	public int CountY;
	public float MaxCooldown;
	public float CurrentCooldown;
	public Entity UnitPrefab;
}
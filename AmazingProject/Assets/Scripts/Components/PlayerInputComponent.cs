using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[System.Serializable]
public struct PlayerInputComponent : IComponentData
{
	public bool RightClick;
	public bool LeftClick;
	public float3 MousePosition;
}

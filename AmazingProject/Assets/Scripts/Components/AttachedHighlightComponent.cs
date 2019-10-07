using UnityEngine;
using UnityEditor;
using Unity.Entities;

public struct AttachedHighlightComponent : IComponentData
{
	public Entity Highlight;
}
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

// look into RequireSubtractiveComponent
[UpdateAfter(typeof(UnitSelectionSystem))]
public class SelectionHighlightSystem : ComponentSystem
{
	EntityQuery m_selectedUnits;
	EntityQuery m_delectedUnits;
	EntityManager m_entityManager;

	protected override void OnCreate()
	{
		m_entityManager = World.Active.EntityManager;
		m_selectedUnits = GetEntityQuery(ComponentType.ReadOnly(typeof(HighlightSpawnerComponent)), ComponentType.ReadOnly(typeof(UnitSelectionComponent)), typeof(NeedsHighlighComponent));
		m_delectedUnits = GetEntityQuery(ComponentType.ReadOnly(typeof(HighlightSpawnerComponent)), ComponentType.ReadOnly(typeof(UnitSelectionComponent)), typeof(DeselectedTagComponent));
	}

	protected override void OnUpdate()
	{
		using (var selectedUnits = m_selectedUnits.ToEntityArray(Unity.Collections.Allocator.TempJob))
		{
			foreach (var selectedUnit in selectedUnits)
			{

				var prefab = m_entityManager.GetSharedComponentData<HighlightSpawnerComponent>(selectedUnit).Highlight;
				var instance = PostUpdateCommands.Instantiate(prefab);
				PostUpdateCommands.AddComponent(instance, new Parent { Value = selectedUnit });
				PostUpdateCommands.AddComponent(selectedUnit, new AttachedHighlightComponent { Highlight = instance });
				PostUpdateCommands.AddComponent(instance, typeof(LocalToParent));
				PostUpdateCommands.SetComponent(instance, new Translation { Value = new float3(0f, -0.8f, 0) });
				PostUpdateCommands.RemoveComponent<NeedsHighlighComponent>(selectedUnit);
			}
		}

		using (var deselectedUnits = m_delectedUnits.ToEntityArray(Unity.Collections.Allocator.TempJob))
		{
			foreach (var deselectedUnit in deselectedUnits)
			{
				var highlight = m_entityManager.GetComponentData<AttachedHighlightComponent>(deselectedUnit);
				PostUpdateCommands.DestroyEntity(highlight.Highlight);
				PostUpdateCommands.RemoveComponent(m_delectedUnits, typeof(AttachedHighlightComponent));
				PostUpdateCommands.RemoveComponent<DeselectedTagComponent>(deselectedUnit);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Unit : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
	[SerializeField] private float m_movementSpeed = 5f;
	[SerializeField] private GameObject m_highlight;

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		referencedPrefabs.Add(m_highlight);
	}

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		dstManager.AddSharedComponentData(entity, new HighlightSpawnerComponent { Highlight = conversionSystem.GetPrimaryEntity(m_highlight) });

		dstManager.AddComponent(entity, typeof(PlayerInputComponent));
		dstManager.AddComponent(entity, typeof(UnitSelectionComponent));
		dstManager.AddComponentData(entity, new MoveAgentComponent
		{
			MovementSpeed = m_movementSpeed
		});
	}
}

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class UnitSpawner : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
	[SerializeField] private GameObject m_unit;

	// Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		referencedPrefabs.Add(m_unit);
	}

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		dstManager.AddComponentData(entity, new UnitSpawnerComponent
		{
			CountX = 10,
			CountY = 10,
			UnitPrefab = conversionSystem.GetPrimaryEntity(m_unit)
		});
	}
	
}


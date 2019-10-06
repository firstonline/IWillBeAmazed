using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Unit : MonoBehaviour, IConvertGameObjectToEntity
{
	[SerializeField] private float m_movementSpeed = 5f;

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{

		dstManager.AddComponent(entity, typeof(PlayerInputComponent));
		dstManager.AddComponent(entity, typeof(UnitSelectionComponent));
		dstManager.AddComponentData(entity, new MoveAgentComponent
		{
			MovementSpeed = m_movementSpeed
		});
	}
}

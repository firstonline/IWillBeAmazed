using Unity.Entities;
using Unity.Mathematics;

public enum NavAgentStatus
{
	Idle,
	Moving,
}

public struct MoveAgentComponent : IComponentData
{
	public float3 FinalDestination;
	public NavAgentStatus Status;
	public float MovementSpeed;
}

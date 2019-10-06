using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

public class UnitSelectionSystem : JobComponentSystem
{
	EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;
	float cooldown;

	protected override void OnCreate()
	{
		// Cache the BeginSimulationEntityCommandBufferSystem in a field, so we don't have to create it every frame
		m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}

	private struct SelectionJob : IJobForEach<UnitSelectionComponent, PlayerInputComponent, Translation>
	{
		public EntityCommandBuffer.Concurrent commandBuffer;

		public void Execute(ref UnitSelectionComponent selection,
			[ReadOnly] ref PlayerInputComponent playerInput, [ReadOnly] ref Translation translation)
		{
			if (playerInput.LeftClick)
			{
				if (math.distance(playerInput.MousePosition, translation.Value) < 1f)
				{
					selection.IsSelected = true;
				}
				else
				{
					selection.IsSelected = false;
				}
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var job = new SelectionJob
		{
			commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
		}.Schedule(this, inputDeps);

		m_EntityCommandBufferSystem.AddJobHandleForProducer(job);
		return job;
	}
}

using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

public class UnitSelectionSystem : JobComponentSystem
{
	BeginSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;
	float cooldown;

	protected override void OnCreate()
	{
		// Cache the BeginSimulationEntityCommandBufferSystem in a field, so we don't have to create it every frame
		m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
	}

	private struct SelectionJob : IJobForEachWithEntity<UnitSelectionComponent, PlayerInputComponent, Translation>
	{
		public EntityCommandBuffer.Concurrent commandBuffer;

		public void Execute(Entity entity, int index, ref UnitSelectionComponent selection,
			[ReadOnly] ref PlayerInputComponent playerInput, [ReadOnly] ref Translation translation)
		{
			if (playerInput.LeftClick)
			{
				if (math.distance(playerInput.MousePosition, translation.Value) < 0.71f)
				{
					if (!selection.IsSelected)
					{
						commandBuffer.AddComponent(index, entity, typeof(NeedsHighlighComponent));
					}
					selection.IsSelected = true;
				}
				else
				{
					if (selection.IsSelected)
					{
						commandBuffer.AddComponent(index, entity, typeof(DeselectedTagComponent));
					}
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

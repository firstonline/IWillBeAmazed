using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

[UpdateAfter(typeof(UnitSelectionSystem))]
public class UnitMovementSystem : JobComponentSystem
{
	[Unity.Burst.BurstCompile]
	struct UnitMovementJob : IJobForEach<PlayerInputComponent, MoveAgentComponent, UnitSelectionComponent>
	{
		public void Execute([ReadOnly] ref PlayerInputComponent playerInput, ref MoveAgentComponent movementAgent, ref UnitSelectionComponent selection)
		{
			if (playerInput.RightClick && selection.IsSelected)
			{
				movementAgent.FinalDestination = playerInput.MousePosition;
				movementAgent.Status = NavAgentStatus.Moving;
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var job = new UnitMovementJob();
		return job.Schedule(this, inputDeps);
	}
}

using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;

[UpdateAfter(typeof(UnitMovementSystem))]
public class MoveAgentSystem : JobComponentSystem
{
	[BurstCompile]
	private struct MoveJob : IJobForEach<Translation, MoveAgentComponent>
	{
		[ReadOnly] public float deltaTime;

		public void Execute(ref Translation translation, ref MoveAgentComponent moveAgent)
		{
			float distance = math.distance(translation.Value, moveAgent.FinalDestination);

			if (moveAgent.Status == NavAgentStatus.Moving )
			{
				if (distance <= 0.05f)
				{
					moveAgent.Status = NavAgentStatus.Idle;
				}
				else
				{
					// we can cache this
					var direction = math.normalize(moveAgent.FinalDestination - translation.Value);
					translation.Value += deltaTime * direction * moveAgent.MovementSpeed;
				}
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var job = new MoveJob
		{
			deltaTime = Time.deltaTime
		};

		return job.Schedule(this, inputDeps);
	}
}

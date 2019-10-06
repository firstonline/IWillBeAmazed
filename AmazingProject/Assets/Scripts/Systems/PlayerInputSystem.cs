using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[UpdateBefore(typeof(UnitMovementSystem))]
public class PlayerInputSystem : JobComponentSystem
{
	struct PlayerInputJob : IJobForEach<PlayerInputComponent>
	{
		public bool RightClick;
		public bool LeftClick;
		public float3 MousePosition;

		public void Execute(ref PlayerInputComponent data)
		{
			data.RightClick = RightClick;
			data.LeftClick = LeftClick;
			data.MousePosition = MousePosition;
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var mousePos = Input.mousePosition;
		var ray = Camera.main.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			mousePos = new float3(hit.point.x, 0, hit.point.z);
		}
		var job = new PlayerInputJob
		{
			RightClick = Input.GetMouseButtonDown(1),
			LeftClick = Input.GetMouseButtonDown(0),
			MousePosition = mousePos
		};

		return job.Schedule(this, inputDeps);
	}
}

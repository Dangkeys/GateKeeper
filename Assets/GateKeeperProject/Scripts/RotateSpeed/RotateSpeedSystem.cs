using JetBrains.Annotations;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct RotatingSpeedSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotateSpeed>();
    }

    public void OnUpdate(ref SystemState state)
    {
        // foreach (var (localTransform, rotateSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>())
        // {
        //     localTransform.ValueRW = localTransform.ValueRO.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime);
        // }
        RotatingCubeJob rotatingCubeJob = new RotatingCubeJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        rotatingCubeJob.Schedule();
    }
    [BurstCompile]
    public partial struct RotatingCubeJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
        {

            localTransform = localTransform.RotateY(rotateSpeed.value * deltaTime);
        }
    }
}
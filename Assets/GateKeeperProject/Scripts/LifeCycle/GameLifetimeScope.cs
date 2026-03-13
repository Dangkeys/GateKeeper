using GateKeeperProject.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{

    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<AmmoDropSystem>();
        builder.RegisterComponentInHierarchy<AmmoSystem>();
        builder.RegisterComponentInHierarchy<GunSystem>();
        builder.RegisterComponentInHierarchy<RewardSystem>();
        builder.RegisterComponentInHierarchy<WaveHandler>();
        builder.RegisterComponentInHierarchy<BlessingUI>();
        builder.RegisterComponentInHierarchy<GameManager>();
    }
}

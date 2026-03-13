using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySizeConfigSO", menuName = "Scriptable Objects/EnemySizeConfigSO")]
public class EnemySizeConfigSO : ScriptableObject
{
    [field: SerializeField] public List<EnemyColliderConfig> ColliderConfigList { get; private set; }
    [field: SerializeField] public float AgentRadius { get; private set; }
    [field: SerializeField] public float AgentHeight { get; private set; }

}

public enum EnemyHandType
{
    Left,
    Right,
    None
}
public enum ColliderDirection
{
    X,
    Y,
    Z
}
[System.Serializable]
public class EnemyColliderConfig
{
    public List<string> ParentTransformNameList = new List<string>();
    public string GameObjectTag = "Enemy";
    public string GameObjectLayer = "Enemy";
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Center;
    public ColliderDirection EnemyColliderDirection = ColliderDirection.Y;
    public float Radius;
    public float Height;
    public bool IsTrigger = true;
    public bool ShouldDisableOnAwake = false;
}
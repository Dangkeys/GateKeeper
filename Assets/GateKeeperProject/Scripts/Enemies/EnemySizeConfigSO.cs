using System.Collections.Generic;
using UnityEditor.Build.Utilities;
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

[System.Serializable]
public class EnemyColliderConfig
{
    public EnemyHandType HandType = EnemyHandType.None;
    public List<string> ParentTransformNameList = new List<string>();
    public string GameObjectTag = "Enemy";
    public string GameObjectLayer = "Enemy";
    public Vector3 Position;
    public Vector3 Center;
    public float Radius;
    public float Height;
    public bool IsTrigger = true;
    public bool ShouldDisableOnAwake = false;
}
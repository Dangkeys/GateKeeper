using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPoint : MonoBehaviour
{
    public static List<SpawnPoint> SpawnPointList { get; private set; } = new List<SpawnPoint>();
    private void Awake()
    {
        SpawnPointList.Add(this);
    }

    private void OnDestroy()
    {
        SpawnPointList.Remove(this);
    }
    public static GameObject GetRandomWayPointGO()
    {
        if (SpawnPointList.Count == 0)
        {
            return null;
        }
        return SpawnPointList[Random.Range(0, SpawnPointList.Count)].gameObject;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
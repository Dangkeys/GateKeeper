using UnityEngine;
using MoreMountains.Feedbacks;

public class HitEffectHandler : MonoBehaviour
{
    private MMF_Player feedbacks;

    private void Awake()
    {
        feedbacks = GetComponent<MMF_Player>();
    }

    public static void Spawn(GameObject effectPrefab, Vector3 position, Vector3 normal)
    {
        if (effectPrefab == null)
        {
            Debug.LogWarning("[HitEffect] effectPrefab is null!");
            return;
        }

        if (normal == Vector3.zero)
        {
            Debug.LogWarning("[HitEffect] hit.normal is Vector3.zero — using Vector3.up as fallback");
            normal = Vector3.up;
        }

        Quaternion rotation = Quaternion.LookRotation(normal);
        GameObject instance = Instantiate(effectPrefab, position, rotation);

        Debug.Log($"[HitEffect] Spawned '{effectPrefab.name}' at {position}");

        HitEffectHandler handler = instance.GetComponent<HitEffectHandler>();
        if (handler == null)
        {
            Debug.LogWarning("[HitEffect] HitEffectHandler script missing on prefab!");
            Destroy(instance, 2f);
            return;
        }

        if (handler.feedbacks == null)
        {
            Debug.LogWarning("[HitEffect] MMF_Player is null on prefab!");
            Destroy(instance, 2f);
            return;
        }

        int feedbackCount = handler.feedbacks.FeedbacksList?.Count ?? 0;
        Debug.Log($"[HitEffect] Playing feedbacks — {feedbackCount} feedback(s) configured");

        handler.feedbacks.PlayFeedbacks();
        Destroy(instance, 2f);
    }
}
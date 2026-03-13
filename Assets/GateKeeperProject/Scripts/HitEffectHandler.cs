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
            return;
        }

        if (normal == Vector3.zero)
        {
            normal = Vector3.up;
        }

        Quaternion rotation = Quaternion.LookRotation(normal);
        GameObject instance = Instantiate(effectPrefab, position, rotation);


        HitEffectHandler handler = instance.GetComponent<HitEffectHandler>();
        if (handler == null)
        {
            Destroy(instance, 2f);
            return;
        }

        if (handler.feedbacks == null)
        {
            Destroy(instance, 2f);
            return;
        }

        int feedbackCount = handler.feedbacks.FeedbacksList?.Count ?? 0;

        handler.feedbacks.PlayFeedbacks();
        Destroy(instance, 2f);
    }
}
using System.Collections;
using UnityEngine;

public class BulletTrailHandler : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public static void Spawn(GameObject trailPrefab, Vector3 startPoint, Vector3 endPoint, float speed, float bulletSize)
    {
        GameObject instance = Instantiate(trailPrefab, startPoint, Quaternion.identity);
        BulletTrailHandler handler = instance.GetComponent<BulletTrailHandler>();

        if (handler != null)
        {
            handler.StartCoroutine(handler.MoveTrail(startPoint, endPoint, speed, bulletSize));
        }
    }

    private IEnumerator MoveTrail(Vector3 start, Vector3 end, float speed, float bulletSize)
    {
        trailRenderer.emitting = false;
        trailRenderer.Clear();

        // Apply bullet size to trail width
        trailRenderer.startWidth = bulletSize;
        trailRenderer.endWidth = 0f; // taper to point

        transform.position = start;
        yield return null;

        trailRenderer.emitting = true;

        float distance = Vector3.Distance(start, end);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(start, end, progress);
            yield return null;
        }

        transform.position = end;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(trailRenderer.time);
        Destroy(gameObject);
    }
}

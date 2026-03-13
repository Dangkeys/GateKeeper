using System.Collections;
using UnityEngine;

public class EnemyHitFlash : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private Color bodyHitColor = new Color(0.4f, 0.8f, 1f);
    [SerializeField] private Color headshotColor = new Color(1f, 0.1f, 0.1f);

    private Renderer[] renderers;
    private MaterialPropertyBlock propBlock;
    private Coroutine flashCoroutine;

    public void Initialize(GameObject visual)
    {
        renderers = visual.GetComponentsInChildren<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    public void Flash(bool isHeadshot)
    {
        if (renderers == null || renderers.Length == 0) return;
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashCoroutine(isHeadshot ? headshotColor : bodyHitColor));
    }

    private IEnumerator FlashCoroutine(Color flashColor)
    {
        SetColor(flashColor);
        yield return new WaitForSeconds(flashDuration);
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        foreach (Renderer r in renderers)
        {
            r.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor", color);
            propBlock.SetColor("_Color", color);     
            r.SetPropertyBlock(propBlock);
        }
    }
}
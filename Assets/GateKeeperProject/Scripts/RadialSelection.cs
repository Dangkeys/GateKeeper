using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class RadialSelection : MonoBehaviour
{
    public InputActionReference spawnAction;
    [Range(1, 10)]
    public int numberOfRadialPart;
    public GameObject radialPartPrefab;
    public Transform radialPartCanvas;
    public Transform handTranform;
    public float angleBetweenParts = 10f;
    public float spawnDistance = 10.0f;
    public Sprite[] icons;
    public float iconDistance = 100f;
    public float iconSize = 50f;
    public UnityEvent<int> onPartSelected;
    private List<GameObject> spawnedParts = new List<GameObject>();
    private int currentSelectedRadialPart = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spawnAction != null)
            spawnAction.action.Enable();
    }

    // Update is called once per frame
    void Update()
    { 
        if (spawnAction != null && spawnAction.action.WasPressedThisFrame())
        {
            SpawnRadialParts();
            radialPartCanvas.gameObject.SetActive(true);
        }
        if (spawnAction != null && spawnAction.action.WasReleasedThisFrame())
        {
            HideAndTriggerSelected();
        }
        if (radialPartCanvas.gameObject.activeSelf)
        {
            GetSelectedRadialPart();
        }
    }

    public void HideAndTriggerSelected()
    {
        onPartSelected.Invoke(currentSelectedRadialPart);
        radialPartCanvas.gameObject.SetActive(false);
    }

    public void GetSelectedRadialPart()
    {
        Vector3 centerToHand = handTranform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);
    
        if (angle < 0)
        {
            angle += 360f;
        }
        currentSelectedRadialPart = (int)(angle * numberOfRadialPart / 360f);

        for (int i = 0; i < spawnedParts.Count; i++)
        {
            if (i == currentSelectedRadialPart)
            {
                spawnedParts[i].GetComponent<Image>().color = Color.yellow;
                spawnedParts[i].transform.localScale = Vector3.one * 1.1f;
            }
            else
            {
                spawnedParts[i].GetComponent<Image>().color = Color.white;
                spawnedParts[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void SpawnRadialParts()
    {
        radialPartCanvas.gameObject.SetActive(true);
        radialPartCanvas.position = handTranform.position + handTranform.forward * spawnDistance;
        radialPartCanvas.rotation = handTranform.rotation;

        foreach (var item in spawnedParts)
        {
            Destroy(item);
        } 
        spawnedParts.Clear();

        for (int i = 0; i < numberOfRadialPart; i++)
        {
            float angle = - i * (360f / numberOfRadialPart) - angleBetweenParts/2f;
            Vector3 radialPartEulerAngles = new Vector3(0, 0, angle);

            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);
            spawnedRadialPart.transform.position = radialPartCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = radialPartEulerAngles;
            spawnedRadialPart.GetComponent<Image>().fillAmount = (1f / (float)numberOfRadialPart) - (angleBetweenParts/360f);

            if (icons != null && i < icons.Length && icons[i] != null)
            {
                GameObject iconObj = new GameObject("Icon", typeof(RectTransform), typeof(Image));
                iconObj.transform.SetParent(spawnedRadialPart.transform, false);

                float fillAngle = ((360f / numberOfRadialPart) - angleBetweenParts);
                float halfFillRad = (fillAngle / 2f) * Mathf.Deg2Rad;
                Vector2 iconLocalPos = new Vector2(Mathf.Sin(halfFillRad), Mathf.Cos(halfFillRad)) * iconDistance;

                RectTransform iconRect = iconObj.GetComponent<RectTransform>();
                iconRect.anchoredPosition = iconLocalPos;

                Rect spriteRect = icons[i].rect;
                float aspect = spriteRect.width / spriteRect.height;
                if (aspect >= 1f)
                    iconRect.sizeDelta = new Vector2(iconSize, iconSize / aspect);
                else
                    iconRect.sizeDelta = new Vector2(iconSize * aspect, iconSize);

                iconRect.localEulerAngles = new Vector3(0, 0, -angle);

                Image iconImage = iconObj.GetComponent<Image>();
                iconImage.sprite = icons[i];
                iconImage.preserveAspect = true;
                iconImage.raycastTarget = false;
            }

            spawnedParts.Add(spawnedRadialPart);
        }
    }
}

using UnityEngine;

public class AmmoGlowEffect : MonoBehaviour
{
    [Header("Light Settings")]
    public Color glowColor = new Color(1f, 0.4f, 0f);   
    public float lightIntensity = 2.5f;
    public float lightRange = 3f;

    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;          
    public float pulseAmount = 0.6f;       

    [Header("Bob Settings")]
    public bool enableBobbing = true;
    public float floatHeight = 0.5f;       
    public float bobHeight = 0.15f;        
    public float bobSpeed = 1.5f;

    [Header("Rotation")]
    public bool enableRotation = true;
    public float rotationSpeed = 90f;     

    private Light _light;
    private Vector3 _startPos;
    private float _timeOffset;

    void Start()
    {
        _startPos   = transform.position;
        _timeOffset = Random.Range(0f, Mathf.PI * 2f); 

        GameObject lightGO = new GameObject("GlowLight");
        lightGO.transform.SetParent(transform);
        lightGO.transform.localPosition = Vector3.zero;

        _light             = lightGO.AddComponent<Light>();
        _light.type        = LightType.Point;
        _light.color       = glowColor;
        _light.intensity   = lightIntensity;
        _light.range       = lightRange;
        _light.shadows     = LightShadows.None; 
    }

    void Update()
    {
        float t = Time.time + _timeOffset;

        float pulse    = Mathf.Sin(t * pulseSpeed) * 0.5f + 0.5f;
        _light.intensity = Mathf.Lerp(lightIntensity - pulseAmount,
                                      lightIntensity + pulseAmount,
                                      pulse);

        if (enableBobbing)
        {
            float newY = 0.3f + floatHeight + Mathf.Sin(t * bobSpeed) * bobHeight;
            transform.position = new Vector3(_startPos.x, newY, _startPos.z);
        }

        if (enableRotation)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class VRExpandingCircle : MonoBehaviour
{
    public float maxRadius = 10f;
    public float expandSpeed = 5f;

    private float currentRadius = 0f;
    private Material mat;
    private bool isMax = false;
    [SerializeField] private float timeBeforeVanish = 5f;
    private float currentTimeVanish = 0f;

    void Start()
    {
        mat = GetComponent<Renderer>().material;

        mat.SetFloat("_MaxRadius", maxRadius);
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        currentRadius = 0f;
        isMax = false;
        currentTimeVanish = 0f;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if(!isMax)
        {
            currentRadius += expandSpeed * Time.deltaTime;

            mat.SetFloat("_Radius", currentRadius);

            if (currentRadius >= maxRadius)
            {
                isMax = true;
            }   
        }
        else
        {
            if(currentTimeVanish <= timeBeforeVanish)
            {
                currentTimeVanish += Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            } 
        }
    }
}
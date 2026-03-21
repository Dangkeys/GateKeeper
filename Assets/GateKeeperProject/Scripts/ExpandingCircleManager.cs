using UnityEngine;

public class ExpandingCircleManager : MonoBehaviour
{
    [SerializeField] private VRExpandingCircle vRExpandingCirclePrefab;
    [SerializeField] private Transform expandingCircleTransform;
    [SerializeField] private int maxPooling = 50;
    private VRExpandingCircle[] vRExpandingCircles;
    private int currentIndex = 0;

    void Start()
    {
        vRExpandingCircles = new VRExpandingCircle[maxPooling];
        for(int i = 0; i < maxPooling; i++)
        {
            VRExpandingCircle vRExpandingCircle = Instantiate(vRExpandingCirclePrefab, expandingCircleTransform);
            vRExpandingCircles[i] = vRExpandingCircle;
        }
    }

    public void Spawn(Vector3 position)
    {
        vRExpandingCircles[currentIndex].Spawn(position);
        currentIndex = (currentIndex + 1) / maxPooling; 
    }
}
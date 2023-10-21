using UnityEngine;

public class SelfDestroyInSeconds : MonoBehaviour
{
    public float DestroyInSeconds = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyInSeconds);
    }
}

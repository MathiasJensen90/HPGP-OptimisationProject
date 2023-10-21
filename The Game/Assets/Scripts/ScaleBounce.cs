using UnityEngine;

/// <summary>
/// Shrinks a transform's scale and expands it back with a given frequency. 
/// </summary>
public class ScaleBounce : MonoBehaviour
{
    public float scaleShrinkage = 0.2f;
    public float frequency = 1f;
    
    private Vector3 _minScale;
    private Vector3 _maxScale;
        
    // Start is called before the first frame update
    void Start()
    {
        var localScale = transform.localScale;
        
        _minScale = localScale * (1f - scaleShrinkage);
        _maxScale = localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float sineBetweenZeroAndOne = (Mathf.Sin(Time.time * frequency) + 1f) / 2f;
        transform.localScale = Vector3.Lerp(_minScale, _maxScale, sineBetweenZeroAndOne);
    }
}

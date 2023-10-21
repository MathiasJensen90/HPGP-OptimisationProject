using System;
using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Enemy Target;
    public LineRenderer LineRenderer;
    public PlayerSettings PlayerSettings;
    public float AnimationLength = 1f;
    private float animationProgress = 0f;
    private float animationTime = 0f;
    private IEnumerator tweenCoroutine;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (animationProgress >= 1f && Target != null)
        {
            var target = Target.transform.position - transform.position + Vector3.up * 0.5f;
            LineRenderer.SetPositions(new []{ Vector3.zero, target});
        }
    }

    public void StartLightning(Enemy target)
    {
        Target = target;
        tweenCoroutine = TweenLineRenderer();
        StartCoroutine(tweenCoroutine);
    }

    IEnumerator TweenLineRenderer()
    {
        while(animationTime < AnimationLength)
        {
            animationProgress = animationTime / AnimationLength;
            LineRenderer.SetPositions(new []{ Vector3.zero, animationProgress * (Target.transform.position - transform.position  + Vector3.up * 0.5f)});
            animationTime += Time.deltaTime;
            
            // Animation finished
            if (animationTime > AnimationLength)
            {
                animationTime = AnimationLength;
                animationProgress = 1f;
                Target.isElectrocuted = true;
                Target.Die();
                Destroy(gameObject, 1f);
                
                // Instantiate ImpactFX (particles + sound)
                Instantiate(PlayerSettings.ImpactFXPrefab, Target.transform.position, Quaternion.identity);

                // Instantiate lightning propagator
                var propagator = Instantiate(PlayerSettings.LightningPropagator, Target.transform.position, Quaternion.identity);
                propagator.StartPropagation(Target);
            }
            yield return null;
        }
    }
}

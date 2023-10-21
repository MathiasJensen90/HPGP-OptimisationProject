using System.Collections.Generic;
using UnityEngine;

public class LightningPropagator : MonoBehaviour
{
    public PlayerSettings PlayerSettings;
    public GameObject LightningPrefab;
    public GameObject RadiusVisualizationPrefab;

    private Enemy Source;
    HashSet<Enemy> enemiseInRange = new();

    // Start is called before the first frame update
    void Start()
    {
        var diameter = PlayerSettings.LightningPropagationRadius * 2;
        RadiusVisualizationPrefab.transform.localScale = new Vector3(diameter, RadiusVisualizationPrefab.transform.localScale.y, diameter);
        Destroy(RadiusVisualizationPrefab, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Source != null)
        {
            transform.position = Source.transform.position + Vector3.up * 0.5f;
        } else
        {
            Destroy(gameObject); // Note(JCH): If the object we were making lightning from is entirely gone, we can just destroy this
        }
    }

    public void StartPropagation(Enemy source)
    {
        Source = source;
        FindTargetsInRadius(PlayerSettings.LightningPropagationRadius);
    }
    
    void FindTargetsInRadius(float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            var enemy = hitCollider.GetComponentInParent<Enemy>();

            if (enemy != null && enemy.gameObject != Source && !enemy.isElectrocuted)
            {
                enemiseInRange.Add(enemy);
            }
        }

        foreach (var enemy in enemiseInRange)
        {
            var lightning = Instantiate(LightningPrefab, transform);
            lightning.GetComponent<Lightning>().StartLightning(enemy);
        }
    }
}

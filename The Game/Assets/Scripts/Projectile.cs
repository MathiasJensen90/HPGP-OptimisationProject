using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float InitialImpulse = 100f;
    public PlayerSettings PlayerSettings;

    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * InitialImpulse);
        //Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Quaternion impactFXRotation = Quaternion.FromToRotation(collision.transform.position, collision.GetContact(0).normal);
        Instantiate(PlayerSettings.ImpactFXPrefab, collision.GetContact(0).point, impactFXRotation);
        
        var enemy = collision.gameObject.GetComponentInParent<Enemy>();
        
        if (enemy != null && !enemy.isElectrocuted)
        {
            enemy.isElectrocuted = true;
            var propagator = Instantiate(PlayerSettings.LightningPropagator, collision.transform.position, Quaternion.identity);
            propagator.StartPropagation(enemy);
            enemy.Die();
        }
        
        //Destroy(gameObject);
    }
}

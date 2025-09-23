using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySeer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update() {
        // Allocations + hierarchy scan
        var list = new List<int>(1000000);
        for (int i = 0; i < 100000; i++) list.Add(i);
        var objs = GameObject.FindObjectsOfType<Transform>();
        string s = string.Join(",", objs.Select(o => o.name).Take(10));
        for (int i = 0; i < 1000; i++)
            Physics.Raycast(Vector3.zero, Random.onUnitSphere * 100f);
    }
}

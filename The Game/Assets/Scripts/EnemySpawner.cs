using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 3;
    [SerializeField] private int _spawnCount = 10;
    [SerializeField] private List<SpawnSetting> _spawnSettings;
    private List<Transform> _transforms;
    private float _timeSinceLastSpawn;

    public int SpawnCount
    {
        get => _spawnCount;
        set => _spawnCount = value;
    }


    private void OnEnable()
    {
        _transforms = GetComponentsInChildren<EnemySpawnPoint>().Select(t=>t.transform).ToList();
    }

    private void Update()
    {
        if (SpawnCount <= 0)
        {
            return;
        }
        
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn>= _spawnInterval)
        {
            var positionIndex = Random.Range(0, _transforms.Count);
            var spawnPos = _transforms[positionIndex];
            var spawnSetting = GetSpawnSetting();
            Instantiate(spawnSetting.Enemy, spawnPos.position, spawnPos.rotation);
            SpawnCount--;
            _timeSinceLastSpawn = 0;
        }
    }

    private SpawnSetting GetSpawnSetting()
    {
        float cdf = 0f;
        foreach (var spawnSetting in _spawnSettings)
        {
            cdf += spawnSetting.Weight;
        }

        var num = Random.Range(0f, cdf);
        var w = _spawnSettings.OrderBy(n => n.Weight);
        
        foreach (var spawnSetting in w)
        {
            if (spawnSetting.Weight<num)
            {
                return spawnSetting;
            }
        }

        return _spawnSettings.LastOrDefault();
    }

    [Serializable]
    private class SpawnSetting
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private float _weight;

        public Enemy Enemy
        {
            get => _enemy;
            set => _enemy = value;
        }

        public float Weight
        {
            get => _weight;
            set => _weight = value;
        }
    }
}

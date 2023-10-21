using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance = null;

    AIController enemyController = new AIController();

    public bool mockupEnemies = false;

    public Action PlayerWasKilled;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            AIController.Instance = enemyController;
            enemyController.OnEnemyDeregistered += ScoreManager.Instance.EnemyDeregistered;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        PlayerWasKilled += () =>
        {
            SceneManager.LoadScene("DeathScoreScreen");
        };
    }

    void Start()
    {
        if (mockupEnemies)
        {
            foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            {
                enemyController.RegisterEnemy(enemy);
            }
        }
        if (enemyController.target == null)
        {
            var target = FindObjectOfType<PlayerController>();

            enemyController.SetTarget(target != null ? target.transform : Camera.main.transform);
        }
    }

    void Update()
    {
        if (enemyController.target == null)
        {
            enemyController.target = FindObjectOfType<PlayerController>()?.transform;
        } else
        {
            enemyController.Tick(Time.deltaTime);
        }
    }
}

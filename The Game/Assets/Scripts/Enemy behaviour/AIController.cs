using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    public Enemy[] enemies = new Enemy[256];
    public int enemyCount = 0;
    
    public Transform target = null;

    public static AIController Instance;

    public Action<Enemy> OnEnemyDeregistered;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Tick(float dt)
    {
        foreach (var unit in enemies)
        {
            if (unit == null || unit.isKillingPlayer)
            {
                continue;
            }
            var pos = unit.transform.position;
            var targetPos = target.position;

            var direction = targetPos - pos;
            
            if (direction.sqrMagnitude < 0.04)
            {
                unit.StartKillingPlayer();

                continue;
            }

            direction = direction == Vector3.zero ? unit.transform.forward : direction;
            var newPosition = Vector3.MoveTowards(pos, targetPos, unit.Velocity * dt);

            if (direction != Vector3.zero)
            {
                unit.transform.LookAt(target);
                unit.transform.position = newPosition;
            }

            // TODO: Make enemies do something if they get close enough to the player.
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemyCount++;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                enemies[i] = enemy;

                break;
            }
        }

        // TODO: Resize dynamically or increase size beyond 256 manually if need be.
    }

    public void DeregisterEnemy(Enemy enemy)
    {
        enemyCount--;
        OnEnemyDeregistered.Invoke(enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == enemy)
            {
                enemies[i] = null;
                break;
            }
        }
    }
}

﻿using UnityEngine;
using WSP.Map;

namespace WSP.Units.Enemies
{
    public class EnemySpawner
    {
        Level level;
        UnitController unitController;
        int frequency;
        int maxEnemies;

        int spawnTimer;

        int spawnedEnemies;

        public EnemySpawner(int frequency, int maxEnemies, UnitController unitController, Level level)
        {
            this.frequency = frequency;
            this.maxEnemies = maxEnemies;
            this.unitController = unitController;
            this.level = level;

            spawnTimer = frequency / 2;
        }

        public void SpawnEnemies()
        {
            if (spawnTimer > 0)
            {
                spawnTimer--;
                return;
            }

            if (spawnedEnemies >= maxEnemies) return;

            spawnedEnemies++;
            spawnTimer = frequency;
            var enemy = Object.Instantiate(unitController);
            level.AddUnit(enemy);
            enemy.Unit.Movement.SetPosition(level.Map.GetRandomRoom().GetRandomPosition());
            enemy.Unit.OnDeath += () => { spawnedEnemies--; };
        }
    }
}
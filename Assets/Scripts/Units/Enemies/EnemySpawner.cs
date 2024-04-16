using UnityEngine;
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

        public EnemySpawner(int frequency, int maxEnemies, UnitController unitController, Level level)
        {
            this.frequency = frequency;
            this.maxEnemies = maxEnemies;
            this.unitController = unitController;
            this.level = level;
        }

        public void SpawnEnemies()
        {
            if (spawnTimer > 0)
            {
                spawnTimer--;
                return;
            }

            if (level.Units.Count >= maxEnemies) return;

            spawnTimer = frequency;
            var enemy = Object.Instantiate(unitController);
            level.AddUnit(enemy);
            enemy.Unit.Movement.SetPosition(level.Map.GetRandomRoom().GetRandomPosition());
        }
    }
}
using Actor;
using Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntrySide
{
    Left,
    Right
}

public class LevelEnemySpawner
{
    private List<EnemySpawn> spawnList = new List<EnemySpawn>();


    public Queue<EnemySpawn> GetSpawnQueue()
    {
        // Spawn objects by time
        spawnList.Sort((x, y) => x.spawnTime.CompareTo(y.spawnTime));
        return new Queue<EnemySpawn>(spawnList);
    }

    public void Add(EnemySpawn es)
    {
        spawnList.Add(es);
    }

    public void CleanQueue()
    {
        spawnList = new List<EnemySpawn>();
    }

    public void CurvedWave(int numShips, float startTime, GameObject shipType, Vector3 startPos, EntrySide entrySide, int numRows = 1, float offset = 0.5f)
    {
        var sign = (entrySide == EntrySide.Left) ? -1 : 1;
        Func<int, int, Vector3> spawnerLambda = (col, row) => {
            return new Vector3(startPos.x + (sign * col * offset), startPos.y - (row * offset), startPos.z);
        };

        SpawnEnemyHelper(numShips,
                         startTime,
                         shipType,
                         entrySide,
                         numRows,
                         spawnerLambda);
    }

    public void TopWave(int numShips, float startTime, GameObject shipType, Vector3 startPos, EntrySide entrySide, int numRows = 1, float offset = 0.5f)
    {
        var sign = (entrySide == EntrySide.Left) ? -1 : 1;
        Func<int, int, Vector3> spawnerLambda = (col, row) => {
            return new Vector3(startPos.x + (sign * row * offset), startPos.y, startPos.z);
        };

        SpawnEnemyHelper(numShips,
                         startTime,
                         shipType,
                         entrySide,
                         numRows,
                         spawnerLambda);
    }

    public void SpawnEnemyHelper(int numShips,
                                 float startTime,
                                 GameObject shipType,
                                 EntrySide entrySide,
                                 int numRows,
                                 Func<int, int, Vector3> spawnFunction)
    {
        var sign = (entrySide == EntrySide.Left) ? -1 : 1;
        for (int i = 0; i < numShips; ++i)
        {
            var spawnTime = 0.2f * i + startTime;
            for (int j = 0; j < numRows; ++j)
            {
                var spawnPos = spawnFunction(i, j);
                spawnList.Add(new EnemySpawn{enemy = shipType, spawnPosition = spawnPos, spawnTime = spawnTime});
            }
        }
    }

}

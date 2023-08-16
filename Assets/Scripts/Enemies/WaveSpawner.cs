using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting };
    [System.Serializable]
    public class Wave //Organizes it in inspector
    {
        public Transform enemy; //Position of enemy
        public int count; //Number of enemies
        public float rate; //1/the rate they spawn
    }


    public Wave[] waves; //How many waves
    private int nextWave = 0;
    public Transform[] spawnPoints; //Where enemies spawn

    public string nextStage;

    [Header("Wave Stats")]
    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.Counting;

    public static int enemyCount;


    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {

        if (state == SpawnState.Waiting)
        {
            //print(EnemyisAlive());
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                WaveCompleted();
                return;//Begin new wave
            }
            else
            {
                return;
            }
        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.Spawning)
            {
                //Start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave _wave) //Spawn the enemies
    {
        state = SpawnState.Spawning;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        //spawn

        state = SpawnState.Waiting;

        yield break;
    }

    void WaveCompleted() //Execute when the wave is over
    {

        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;
        enemyCount = 0;

        if (nextWave + 1 > waves.Length - 1)
        {
            //nextWave = 0;
            print("All waves Complete...Looping");
            SceneManager.LoadScene(nextStage);
        }
        else
        {
            nextWave++;
        }
    }

    void SpawnEnemy(Transform _enemy) //Spawn an enemy
    {

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, new Vector3(_sp.position.x, _sp.position.y, 0), Quaternion.identity);

    }

}

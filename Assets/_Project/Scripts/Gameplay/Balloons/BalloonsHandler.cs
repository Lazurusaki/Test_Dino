using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

public class BalloonsHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> balloons = new();

    [SerializeField]
    private ParticleHandler _particleHandler;
    private SoundHandler _soundHandler;
    private PuzzleHandler _puzzleHandler;

    [SerializeField]
    private int balloonsCount = 30;
    [SerializeField]
    private int oneTimeSpawnCount = 6;
    [SerializeField]
    private float spawnDeltaTime = 0.1f;
    [SerializeField]
    private float minSpeed = 4;
    [SerializeField]
    private float maxSpeed = 7;
    [SerializeField]
	// Через сколько
    private float timeBeforeSpawn = 4;
    private int countOnScene;
    private float edgeX,
        edgeY;
    
    private bool isFirst = true;
    private bool firstUse = true;

    public bool UseMouse => YG2.envir.isDesktop;
    public float MinSpeed { get => minSpeed; }
    public float MaxSpeed { get => maxSpeed; }
    public bool FirstUse { get => firstUse; }

    public event Action BalloonHit;

    public void Run()
    {
        //StartCoroutine(WaitAndStart());
        StartSpawn();
    }
    
    private void Start()
    {
        countOnScene = balloonsCount;
        edgeX = Camera.main.orthographicSize * Camera.main.aspect;
        edgeY = Camera.main.orthographicSize;
    }
    
    public void SpawnBalloons(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject balloon = balloons[Random.Range(0, balloons.Count)];
            Vector2 spawnPosition = new Vector2(Random.Range(-edgeX, edgeX), -edgeY - 2);
            Instantiate(balloon, spawnPosition, Quaternion.identity);
        }
    }

    public void PlayObjectParticle(Vector2 position)
    {
        _particleHandler.PlayParticle(position);
    }

    public void DecreaseBalloonsCount()
    {
        if (countOnScene != 0)
        {
            countOnScene--;
            
            BalloonHit?.Invoke();
        }
    }

    public void IncreaseBalloonsCount()
    {
        if (!firstUse) 
            countOnScene++;
    }

    public IEnumerator SpawnObjects(int spawnCount)
    {
        if (isFirst)
        {
            isFirst = false;
        }
            
        int count = 0;
        while(count < spawnCount)
        {
            SpawnBalloons(oneTimeSpawnCount);
            count += oneTimeSpawnCount;
            yield return new WaitForSeconds(spawnDeltaTime);
        }

        if (count - spawnCount < 0)
            SpawnBalloons(spawnCount - count);

        firstUse = false;
    }

    private IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(timeBeforeSpawn);
        StartSpawn();
    }

    private void StartSpawn()
    {
        StartCoroutine(SpawnObjects(balloonsCount));
    }
}

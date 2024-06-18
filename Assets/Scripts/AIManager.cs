using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{

    [SerializeField] private OrderStand[] orderSpots;
    [SerializeField] private List<AICharacter> aiPool;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] despawnPoints;

    private int _poolIndex = 0;
    private int _score = 0;

    public event Action<int> OnScoreUpdated;

    public int Score
    {
        get => _score;
        set
        {
            if (value != _score)
            {
                _score = value;

                if (value > PlayerPrefs.GetInt("BestScore", 0))
                {
                    PlayerPrefs.SetInt("BestScore", value);
                }

                OnScoreUpdated?.Invoke(value);
            }
        }

    }

    private void Start()
    {
        aiPool.Shuffle();

        CreateVisitors();
    }

    private void CreateVisitors()
    {
        foreach (var orderSpot in orderSpots)
        {
            if (!orderSpot.IsBusy)
            {
                var character = aiPool[_poolIndex];

                Debug.Log(character.gameObject.name);

                var spawnPoint = UnityEngine.Random.Range(0, spawnPoints.Length);
                character.transform.SetPositionAndRotation(spawnPoints[spawnPoint].position, spawnPoints[spawnPoint].rotation);
                character.gameObject.SetActive(true);

                character.OrderAt(orderSpot);
                character.OnOrderGrabbed += OnOrderGrabbed;

                _poolIndex++;

                if (_poolIndex >= aiPool.Count)
                {
                    _poolIndex = 0;

                    //aiPool.Shuffle();
                }
            }
        }
    }

    private void OnOrderGrabbed(AICharacter character)
    {
        character.OnOrderGrabbed -= OnOrderGrabbed;

        Score++;

        character.SetAgentDestination(despawnPoints[UnityEngine.Random.Range(0, despawnPoints.Length)].position, (character) =>
        {
            Debug.Log("AgentReachedDestination");
            
            character.gameObject.SetActive(false);
        });

        CreateVisitors();
    }
}

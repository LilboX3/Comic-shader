using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cashPrefabs = new GameObject[3];
    [SerializeField]
    List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnCashAtPoint(spawnPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if(spawnPoint.childCount == 0)
            {
                SpawnNewCashAt(spawnPoint);
            }
        }
    }

    public void SpawnNewCashAt(Transform spawnPoint)
    {
        if(PlayerDistanceAway(10f, spawnPoint)) //Only spawn new cash when player is far away
        {
            SpawnCashAtPoint(spawnPoint);
        }
    }

    private void SpawnCashAtPoint(Transform spawnPoint)
    {
        GameObject cashPrefab = ChoosePrefab();
        GameObject spawned = Instantiate(cashPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
    }

    private bool PlayerDistanceAway(float distance, Transform spawnPoint)
    {
        return Vector3.Distance(spawnPoint.position, player.position) > distance;
    }

    private GameObject ChoosePrefab()
    {
        int random = Random.Range(1, 11);
        if(random == 1)
        {
            return cashPrefabs[2]; //1/10, most valuable: diamond
        }
        if(random <= 4)
        {
            return cashPrefabs[1]; //3/10, gold ingot
        }
        return cashPrefabs[0]; //6/10, cash bundle

    }
}

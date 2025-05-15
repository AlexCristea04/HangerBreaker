using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseClass;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveManagementSystem : MonoBehaviour
{
    [FormerlySerializedAs("enemyTypes")] [Header("Enemies")]
    public List<GameObject> enemyGameObjects;

    private GameObject[] possibleSpawnPoint;
    [Tooltip("This is a field that modifies the health of all enemies")]
    public int difficulty;

    private int waveNumber = 1;
    public TextMeshProUGUI wave;
    private static System.Random random = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        possibleSpawnPoint = GameObject.FindGameObjectsWithTag("Spawnpoint");
        StartWave();
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>().currentHealth =
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>().maxHealth;
        List<GameObject> spawnForRound = FindExactSum(enemyGameObjects, difficulty);
        GameObject[] allTransform = SelectRandomGameObjects(possibleSpawnPoint, spawnForRound.Count);
        for (int i = 0; i < allTransform.Length; i++)
        {
            GameObject transform = allTransform[i];
            transform.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(
                SpawnEnemy(
                    spawnForRound[i],
                    transform.transform.position
                    )
                );
        }
        //TODO Read if we continue the game
        //StartCoroutine(AddHealth());

    }

    public void CheckNumberOfEnemy()
    {
        EnemyAi[] allEnemies = FindObjectsOfType<EnemyAi>();
        Debug.Log(allEnemies.Length);
        if (allEnemies.Length-1 <= 0)
        {
            //WAVE CLEARED
            difficulty += 1;
            waveNumber += 1;
            wave.text = "WAVE "+ waveNumber.ToString();
            StartWave();
        }
    }
    public GameObject[] SelectRandomGameObjects(GameObject[] allGameObjects, int count)
    {
        if (count > allGameObjects.Length)
        {
            Debug.LogError("Requested count is greater than the number of available game objects.");
            return null;
        }

        // Create a copy of the list to avoid modifying the original list
        List<GameObject> shuffledGameObjects = new List<GameObject>(allGameObjects);

        // Shuffle the list using Fisher-Yates algorithm
        for (int i = 0; i < shuffledGameObjects.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, shuffledGameObjects.Count);
            GameObject temp = shuffledGameObjects[i];
            shuffledGameObjects[i] = shuffledGameObjects[randomIndex];
            shuffledGameObjects[randomIndex] = temp;
        }

        // Select the first 'count' elements from the shuffled list
        GameObject[] selectedGameObjects = shuffledGameObjects.GetRange(0, count).ToArray();

        return selectedGameObjects;
    }


    private List<GameObject> FindExactSum(List<GameObject> enemies, int target)
    {
        List<GameObject> result = new List<GameObject>();
        if (FindCombination(enemies, target, result))
        {
            return result;
        }
        else
        {
            Debug.Log($"No combination found to sum to {target}");
            return null;
        }
    }

    private bool FindCombination(List<GameObject> enemies, int target, List<GameObject> currentCombination)
    {
        if (target == 0)
        {
            return true;
        }
        if (target < 0 || enemies.Count == 0)
        {
            return false;
        }

        int randomIndex = random.Next(enemies.Count);
        GameObject chosenEnemy = enemies[randomIndex];
        EnemyAi chosenEnemyAi = chosenEnemy.GetComponent<EnemyAi>();

        if (chosenEnemyAi == null)
        {
            Debug.LogError("Enemy GameObject does not have an EnemyAi component.");
            return false;
        }

        List<GameObject> newEnemies = new List<GameObject>(enemies);
        //newEnemies.RemoveAt(randomIndex);

        currentCombination.Add(chosenEnemy);
        if (FindCombination(newEnemies, target - chosenEnemyAi.enemyDifficulty, currentCombination))
        {
            return true;
        }

        currentCombination.RemoveAt(currentCombination.Count - 1);
        return FindCombination(newEnemies, target, currentCombination);
    }

    IEnumerator SpawnEnemy(GameObject enemyToSpawn, Vector3 position)
    {
        yield return new WaitForSeconds(3);
        Instantiate(enemyToSpawn).transform.position = position;
    }
    IEnumerator AddHealth()
    {
        yield return new WaitForSeconds(3);
        EnemyAi[] allEnemies = FindObjectsOfType<EnemyAi>();
        foreach (var enemy in allEnemies)
        {
            enemy.hp += Random.Range(0, difficulty-3);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int enemiesAlive = 0;
    private GameObject original;
    private Enemy originalClass;
    public int wave = 1;
    public int enemiesToSpawn = 0;
    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        original = GameObject.FindWithTag("Skeleton");
        originalClass = original.GetComponent<Enemy>();
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (enemiesAlive == 0 && timer > 3)
        {
            enemiesToSpawn += wave;
            wave++;
        }
        if (enemiesToSpawn > 0 && timer > 1)
        {
            SpawnEnemy();
            enemiesToSpawn--;
            timer = 0;
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = GameObject.Instantiate(original);
        newEnemy.AddComponent<Enemy>();
        newEnemy.transform.position = original.transform.position;
        newEnemy.tag = "Enemy";
        newEnemy.GetComponent<Enemy>().self = newEnemy;
        enemiesAlive++;
    }



    IEnumerator waiter(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int enemiesAlive = 0;
    private GameObject originalEnemy;
    private GameObject originalMage;
    private GameObject originalKnight;
    private GameObject originalWarrior;
    public int wave = 0;
    public int enemiesToSpawn = 0;
    public float timer = 0;
    public int lives = 5;

    private Dictionary<GameObject, int> towerRanges = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, bool> towerIsRanged = new Dictionary<GameObject, bool>();
    private Dictionary<GameObject, int> towerDamages = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;



        originalEnemy = GameObject.FindWithTag("Skeleton");


        originalMage = GameObject.FindWithTag("Mage");
        originalKnight = GameObject.FindWithTag("Knight");
        originalWarrior = GameObject.FindWithTag("Warrior");

        towerRanges.Add(originalMage, 150);
        towerIsRanged.Add(originalMage, true);
        towerDamages.Add(originalMage, 10);

        towerRanges.Add(originalKnight, 50);
        towerIsRanged.Add(originalKnight, false);
        towerDamages.Add(originalKnight, 20);

        towerRanges.Add(originalWarrior, 60);
        towerIsRanged.Add(originalWarrior, false);
        towerDamages.Add(originalWarrior, 30);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (enemiesAlive == 0 && timer > 3)
        {
            wave++;
            enemiesToSpawn += wave;
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
        GameObject newEnemy = GameObject.Instantiate(originalEnemy);
        newEnemy.AddComponent<Enemy>();
        newEnemy.transform.position = originalEnemy.transform.position;
        newEnemy.tag = "Enemy";
        newEnemy.GetComponent<Enemy>().self = newEnemy;
        enemiesAlive++;
    }

    void CreateTower(GameObject towerType)
    {
        GameObject newTower = GameObject.Instantiate(towerType);
        newTower.AddComponent<Towers>();
        newTower.transform.position = new Vector3(0,0,0);
        newTower.tag = "Player";
        newTower.GetComponent<Towers>().self = newTower;
        newTower.GetComponent<Towers>().atkRange = towerRanges[towerType];
        newTower.GetComponent<Towers>().isRanged = towerIsRanged[towerType];
        newTower.GetComponent<Towers>().damage = towerDamages[towerType];
    }


    IEnumerator waiter(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }

    private void OnGUI()
    {
        GUI.color = Color.white;
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(50, 30, 240, 60), "Lives: " + lives.ToString());
        GUI.Label(new Rect(50, 100, 240, 60), "Enemies Left: " + enemiesAlive.ToString());
        GUI.Label(new Rect(50, 170, 240, 60), "Wave: " + wave.ToString());
        if (GUI.Button(new Rect(300, 30, 240, 60), "Spawn Mage")){ CreateTower(originalMage); }
        if (GUI.Button(new Rect(300, 100, 240, 60), "Spawn Knight")) { CreateTower(originalKnight); }
        if (GUI.Button(new Rect(300, 170, 240, 60), "Spawn Warrior")) { CreateTower(originalWarrior); }
    }
}
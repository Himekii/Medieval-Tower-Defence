using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private Vector3 mousePos;

    private GameObject originalEnemy;
    private GameObject originalMage;
    private GameObject originalKnight;
    private GameObject originalWarrior;

    private GameObject targetTower;
    private GameObject towerToBeMade;

    public int wave = 0;
    public int enemiesAlive = 0;
    public int enemiesToSpawn = 0;
    public int lives = 5;
    private float timer = 0;
    public bool placingTower = false;

    public int gold = 30;
    private int goldTemp;

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
        towerDamages.Add(originalMage, 20);

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
        if (placingTower == true)
        {
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.y = 0;
            if (Input.GetMouseButtonDown(0))
            {
                placingTower = false;
                CreateTower(towerToBeMade, mousePos);
                Destroy(targetTower);
                gold -= goldTemp;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                placingTower = false;
                Destroy(targetTower);
            }
            else
            {
                targetTower.transform.position = mousePos;
            }
        }


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

    void CreateTower(GameObject towerType, Vector3 pos)
    {
        GameObject newTower = GameObject.Instantiate(towerType);
        newTower.AddComponent<Towers>();
        newTower.transform.position = pos;
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
        GUI.Label(new Rect(50, 240, 240, 60), "Gold: " + gold.ToString());
        if (GUI.Button(new Rect(300, 30, 240, 60), "Mage (10g)"))
        {
            if (placingTower == false && gold >= 10)
            {
                goldTemp = 10;
                towerToBeMade = originalMage;
                targetTower = GameObject.Instantiate(originalMage);
                placingTower = true;
            }
        }
        if (GUI.Button(new Rect(300, 100, 240, 60), "Knight (15g)")) 
        {
            if (placingTower == false && gold >= 15)
            {
                goldTemp = 15;
                towerToBeMade = originalKnight;
                targetTower = GameObject.Instantiate(originalKnight); 
                placingTower = true;
            }
        }
        if (GUI.Button(new Rect(300, 170, 240, 60), "Warrior (20g)"))
        {
            if (placingTower == false && gold >= 20)
            {
                goldTemp = 20;
                towerToBeMade = originalWarrior;
                targetTower = GameObject.Instantiate(originalWarrior); 
                placingTower = true;
            }
        }
    }
}
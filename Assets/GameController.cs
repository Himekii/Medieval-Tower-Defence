using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject gameOverUI;

    private Vector3 mousePos;

    private GameObject originalEnemy;
    private GameObject originalMage;
    private GameObject originalKnight;
    private GameObject originalWarrior;

    private GameObject targetTower;
    private GameObject towerToBeMade;

    private GameObject path;
    private GameObject[] paths;
    private LineRenderer lineRenderer;

    private int wave = 0;
    public int enemiesAlive = 0;
    private int enemiesToSpawn = 0;
    public int lives = 5;
    private float timer = 0;
    private bool placingTower = false;
    public bool isDead = false;

    public int gold = 30;
    private int goldTemp;

    private Dictionary<GameObject, int> towerRanges = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, bool> towerIsRanged = new Dictionary<GameObject, bool>();
    private Dictionary<GameObject, int> towerDamages = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;

        paths = GameObject.FindGameObjectsWithTag("Path");

        originalEnemy = GameObject.FindWithTag("Skeleton");


        originalMage = GameObject.FindWithTag("Mage");
        originalKnight = GameObject.FindWithTag("Knight");
        originalWarrior = GameObject.FindWithTag("Warrior");

        towerRanges.Add(originalMage, 150);
        towerIsRanged.Add(originalMage, true);
        towerDamages.Add(originalMage, 20);

        towerRanges.Add(originalKnight, 50);
        towerIsRanged.Add(originalKnight, false);
        towerDamages.Add(originalKnight, 25);

        towerRanges.Add(originalWarrior, 60);
        towerIsRanged.Add(originalWarrior, false);
        towerDamages.Add(originalWarrior, 35);

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
        if (enemiesAlive == 0 && timer > 3 && !isDead)
        {
            int i = Random.Range(0, paths.Length - 1);
            path = paths[i];
            lineRenderer = path.GetComponent<LineRenderer>();
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
        newEnemy.GetComponent<Enemy>().line = path;
        newEnemy.GetComponent<Enemy>().lineRenderer = lineRenderer;
        newEnemy.GetComponent<Enemy>().self = newEnemy;
        newEnemy.transform.position = path.transform.position + lineRenderer.GetPosition(0);

        newEnemy.tag = "Enemy";
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

    public void gameOver()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject tower in towers)
        {
            tower.SetActive(false);
        }
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
        gameOverUI.SetActive(true);
    }
    public void play()
    {
        SceneManager.LoadScene("Game");
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void quit()
    {
        Application.Quit();
    }

    private void OnGUI()
    {
        GameObject livesui = GameObject.Find("Lives");
        GameObject enemiesui = GameObject.Find("Enemies Alive");
        GameObject waveui = GameObject.Find("Wave");
        GameObject goldui = GameObject.Find("Gold");

        livesui.GetComponent<TMPro.TextMeshProUGUI>().text = "Lives: " + lives.ToString();
        enemiesui.GetComponent<TMPro.TextMeshProUGUI>().text = "Enemies Left: " + enemiesAlive.ToString();
        waveui.GetComponent<TMPro.TextMeshProUGUI>().text = "Wave: " + wave.ToString();
        goldui.GetComponent<TMPro.TextMeshProUGUI>().text = "Gold: " + gold.ToString();

    }

    public void spawnMage()
    {
        if (placingTower == false && gold >= 10)
        {
            goldTemp = 10;
            towerToBeMade = originalMage;
            targetTower = GameObject.Instantiate(originalMage);
            placingTower = true;
        }
    }

    public void spawnKnight()
    {
        if (placingTower == false && gold >= 15)
        {
            goldTemp = 15;
            towerToBeMade = originalKnight;
            targetTower = GameObject.Instantiate(originalKnight);
            placingTower = true;
        }
    }
    
    public void spawnWarrior()
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
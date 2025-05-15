using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Math;

public class GameController : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject pauseUI;

    private Vector3 mousePos;

    public Camera cam;

    private readonly Vector2 targetAspectRatio = new(16, 9);
    private readonly Vector2 rectCenter = new(0.5f, 0.5f);

    private Vector2 lastResolution;

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
    private float fpsTimer = 0;
    private float tempTimer = 0;
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

        Debug.Log(Application.platform);


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

    public void LateUpdate()
    {
        var currentScreenResolution = new Vector2(Screen.width, Screen.height);

        // Don't run all the calculations if the screen resolution has not changed
        if (lastResolution != currentScreenResolution)
        {
            CalculateCameraRect(currentScreenResolution);
        }

        lastResolution = currentScreenResolution;
    }

    private void CalculateCameraRect(Vector2 currentScreenResolution)
    {
        var normalizedAspectRatio = targetAspectRatio / currentScreenResolution;
        var size = normalizedAspectRatio / Mathf.Max(normalizedAspectRatio.x, normalizedAspectRatio.y);
        cam.rect = new Rect(default, size) { center = rectCenter };
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

        if (enemiesAlive == 0 && !isDead)
        {
            if (Input.GetKeyDown("escape"))
            {
                Debug.Log("THERE");
                pauseUI.SetActive(true);
                Time.timeScale = 0;
                tempTimer = timer;
                timer = 0;
            }
            if (timer > 3)
            {
                int i = Random.Range(0, paths.Length - 1);
                path = paths[i];
                lineRenderer = path.GetComponent<LineRenderer>();
                wave++;
                enemiesToSpawn += wave;
            }
        }

        timer += Time.deltaTime;
        
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

    public void resume()
    {


        timer = tempTimer;
        pauseUI.SetActive(false);
        Time.timeScale = 1;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            e.GetComponent<Enemy>().timer = e.GetComponent<Enemy>().tempTimer;
        }
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
        GameObject fpsui = GameObject.Find("FPS");

        livesui.GetComponent<TMPro.TextMeshProUGUI>().text = "Lives: " + lives.ToString();
        enemiesui.GetComponent<TMPro.TextMeshProUGUI>().text = "Enemies Left: " + enemiesAlive.ToString();
        waveui.GetComponent<TMPro.TextMeshProUGUI>().text = "Wave: " + wave.ToString();
        goldui.GetComponent<TMPro.TextMeshProUGUI>().text = "Gold: " + gold.ToString();

        fpsTimer += Time.deltaTime;

        if (fpsTimer > 2)
        {
            fpsTimer = 0;
            fpsui.GetComponent<TMPro.TextMeshProUGUI>().text = "FPS: " + System.Math.Round(1.0f / Time.deltaTime, 1).ToString();
        }
        



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
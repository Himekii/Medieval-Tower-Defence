using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject line;
    public GameObject self;

    private float moveSpeed = 0.05f;
    public float timer = 0;
    public float tempTimer = 0;
    private int ind = 0;
    private Vector3 destination;
    public LineRenderer lineRenderer;
    private int numPoints;
    public int health = 100;
    private GameController gameController;
    private bool isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        numPoints = lineRenderer.positionCount;
        gameController = GameObject.FindWithTag("MainCamera").GetComponent<GameController>();
        destination = line.transform.position + lineRenderer.GetPosition(ind);
    }


    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if (Input.GetKeyDown("escape") && !gameController.pauseUI.activeSelf)
            {
                tempTimer = timer;
                gameController.pauseUI.SetActive(true);
                Time.timeScale = 0;
                timer = 0;

            }

            timer += Time.deltaTime * moveSpeed;


            if (transform.position != destination)
            {
                transform.LookAt(destination);
                transform.position = Vector3.MoveTowards(transform.position, destination, timer);
            }
            else
            {
                if (ind < numPoints-1)
                {
                    ind++;
                    destination = line.transform.position + lineRenderer.GetPosition(ind);
                }
                else
                {
                    gameController.lives--;

                    if (gameController.lives == 0 && !isDead)
                    {
                        isDead = true;
                        gameController.isDead = true;
                        gameController.gameOver();
                    }

                    gameController.enemiesAlive--;
                    Destroy(self);
                    //ind = 1;
                    //self.transform.position = line1.transform.position + lineRenderer.GetPosition(0);
                    //updateDest();
                }
            }
        }else
        {
            gameController.gold += 3;
            gameController.enemiesAlive--;
            Destroy(self);
        }
    }
}


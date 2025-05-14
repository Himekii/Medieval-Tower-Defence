using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject[] lines;
    private GameObject line;
    public GameObject self;

    private float moveSpeed = 0.05f;
    private float timer = 0;
    private int ind = 0;
    private Vector3 destination;
    private LineRenderer lineRenderer;
    private int numPoints;
    public int health = 100;
    private GameController gameController;


    // Start is called before the first frame update
    void Start()
    {
        lines = GameObject.FindGameObjectsWithTag("Path");
        int i = Random.Range(0,lines.Length-1);
        line = lines[i];
        lineRenderer = line.GetComponent<LineRenderer>();
        numPoints = lineRenderer.positionCount;
        gameController = GameObject.FindWithTag("MainCamera").GetComponent<GameController>();
        destination = line.transform.position + lineRenderer.GetPosition(ind);
    }


    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            
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


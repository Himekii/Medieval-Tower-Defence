using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject line1;
    public GameObject self;

    public float moveSpeed = 0.05f;
    public float timer;
    static Vector3 destination;
    public int ind;
    private LineRenderer lineRenderer;
    private int numPoints;
    public int health = 100;
    private GameController gameController;


    // Start is called before the first frame update
    void Start()
    {
        line1 = GameObject.Find("Path 1");
        ind = 1;
        timer = 0;
        updateDest();
        gameController = GameObject.FindWithTag("MainCamera").GetComponent<GameController>();
    }

    void updateDest()
    {
        LineRenderer lineRenderer = line1.GetComponent<LineRenderer>();
        destination = line1.transform.position + lineRenderer.GetPosition(ind);
    }


    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            LineRenderer lineRenderer = line1.GetComponent<LineRenderer>();
            int numPoints = lineRenderer.positionCount;
            timer += Time.deltaTime * moveSpeed;


            if (self.transform.position != destination)
            {
                self.transform.position = Vector3.MoveTowards(self.transform.position, destination, timer);
            }
            else
            {
                if (ind < numPoints - 1)
                {
                    ind++;
                    updateDest();
                }
                else
                {
                    ind = 1;
                    self.transform.position = line1.transform.position + lineRenderer.GetPosition(0);
                    updateDest();
                }
            }
        }else
        {
            gameController.enemiesAlive--;
            Destroy(self);
        }
    }
}


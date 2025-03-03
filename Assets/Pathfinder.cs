using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private GameObject line1;
    private GameObject enemy;
    public float moveSpeed;
    public float timer;
    static Vector3 destination;
    public int ind;
    private LineRenderer lineRenderer;
    private int numPoints;



    // Start is called before the first frame update
    void Start()
    {
        line1 = GameObject.Find("Path 1");
        enemy = GameObject.FindWithTag("Enemy");
        ind = 1;
        timer = 0;
        updateDest();
        
    }

    void updateDest()
    {
        LineRenderer lineRenderer = line1.GetComponent<LineRenderer>();
        timer = 0;
        destination = line1.transform.position + lineRenderer.GetPosition(ind);
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lineRenderer = line1.GetComponent<LineRenderer>();
        int numPoints = lineRenderer.positionCount;
        timer += Time.deltaTime * moveSpeed;
        
        
        if (enemy.transform.position != destination)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, destination, timer);
        }
        else
        {
            if (ind < numPoints-1)
            {
                ind++;
                updateDest();
            }
            else
            {
                ind = 1;
                enemy.transform.position = line1.transform.position + lineRenderer.GetPosition(0);
                updateDest();
            }
        }
    }
}


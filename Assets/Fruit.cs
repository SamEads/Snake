using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Snake snake;
    public Collider2D bounds;

    // Start is called before the first frame update
    void Start()
    {
        ChangePosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangePosition()
    {
        int rangeX = (int) Random.Range(bounds.bounds.min.x, bounds.bounds.max.x);
        int rangeY = (int)Random.Range(bounds.bounds.min.y, bounds.bounds.max.y);
        transform.position = new Vector2(rangeX-0.5f, rangeY-0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            ChangePosition();
            snake.Eat();
        }
    }
}

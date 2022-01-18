using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Transform snakeSegment;
    public Collider2D bounds;
    private Vector2 movementDirection = new Vector2(1, 0);
    private float tick;
    private List<Transform> snakeSegments;

    void Start()
    {
        snakeSegments = new List<Transform>();
        snakeSegments.Add(transform);
    }

    public void Eat()
    {
        var newSegment = Instantiate(snakeSegment);
        newSegment.position = snakeSegments[snakeSegments.Count - 1].position;
        snakeSegments.Add(newSegment);
    }

    // Update is called once per frame
    void Update()
    {
        // Y - axis movement
        if (Input.GetAxisRaw("Vertical") < -0.3f || Input.GetAxisRaw("Vertical") > 0.3f)
        {
            movementDirection = new Vector2(0, Mathf.Sign(Input.GetAxisRaw("Vertical")));
        }
        // X - axis movement
        else if (Input.GetAxisRaw("Horizontal") > 0.3f || Input.GetAxisRaw("Horizontal") < -0.3f)
        {
            movementDirection = new Vector2(Mathf.Sign(Input.GetAxisRaw("Horizontal")), 0);
        }

        // Tick movement, so the snake is moving in an unsmooth "grid-like" motion
        var maxTick = 0.1f;
        tick += Time.deltaTime;
        if (tick > maxTick)
        {
            // In case for whatever reason the framerate is bad enough and enough is added to deltaTime, tick until done
            while (tick > maxTick)
            {
                // Move body parts
                if (snakeSegments.Count > 1)
                {
                    for (int i = snakeSegments.Count-1; i > 0; i--)
                    {
                        snakeSegments[i].position = snakeSegments[i-1].position;
                    }
                }
                // Move head
                transform.Translate(movementDirection);
                // Decrease tick
                tick -= maxTick;
            }
        }
        // Wrap-around
        Vector2 myTransform = transform.position;
        if (myTransform.y < bounds.bounds.min.y + 0.5f)
        {
            myTransform.y = bounds.bounds.max.y - 0.5f;
        }
        else if (myTransform.y > bounds.bounds.max.y - 0.5f)
        {
            myTransform.y = bounds.bounds.min.y + 0.5f;
        }
        else if (myTransform.x < bounds.bounds.min.x + 0.5f)
        {
            myTransform.x = bounds.bounds.max.x - 0.5f;
        }
        else if (myTransform.x > bounds.bounds.max.x - 0.5f)
        {
            myTransform.x = bounds.bounds.min.x + 0.5f;
        }
        transform.position = myTransform;
    }
}

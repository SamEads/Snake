using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    public Transform explosion;
    public int startLength = 3;
    public Transform snakeSegment;
    public Collider2D bounds;
    public Animator transAnimator;
    public Animator snakeAnimator;
    public AudioClip deadSound;
    public bool alive = true;
    private AudioSource deadSource;
    private Vector2 movementDirection = new Vector2(1, 0);
    private float tick;
    private List<Transform> snakeSegments;
    private Vector2 lastPosition;

    void Start()
    {
        snakeSegments = new List<Transform>();
        snakeSegments.Add(transform);
        Application.targetFrameRate = 60;
        deadSource = gameObject.AddComponent<AudioSource>();
        for (int i = 0; i < startLength-1; i ++)
        {
            Eat();
        }
    }

    public void Eat()
    {
        var newSegment = Instantiate(snakeSegment);
        newSegment.position = snakeSegments[snakeSegments.Count - 1].position;
        snakeSegments.Add(newSegment);
    }

    void FixedUpdate()
    {
        if (alive)
        {
            // Tick movement, so the snake is moving in an unsmooth "grid-like" motion
            var maxTick = 0.1f;
            if (Input.GetButton("Jump"))
                maxTick /= 4;
            tick += Time.deltaTime;
            if (tick > maxTick)
            {
                // In case for whatever reason the framerate is bad enough and enough is added to deltaTime, tick until done
                while (tick > maxTick)
                {
                    var bodyNewPosition = new Vector2(transform.position.x, transform.position.y) + movementDirection;
                    // Move body parts
                    if (snakeSegments.Count > 1)
                    {
                        for (int i = snakeSegments.Count - 1; i > 0; i--)
                        {
                            if (new Vector2(snakeSegments[i].position.x, snakeSegments[i].position.y) == bodyNewPosition)
                            {
                                if (alive)
                                {
                                    var _explosion = Instantiate(explosion);
                                    Vector3 explosionPosition = bodyNewPosition;
                                    explosionPosition.z = -4;
                                    _explosion.position = explosionPosition;
                                    alive = false;
                                    deadSource.PlayOneShot(deadSound, 0.5f);
                                    for (int j = 0; j < snakeSegments.Count; j ++)
                                    {
                                        var segment = snakeSegments[j];
                                        Color snakeColor = Color.black;
                                        snakeColor.g = 0.25f;
                                        segment.GetComponent<SpriteRenderer>().color = snakeColor;
                                    }
                                }
                            }
                            snakeSegments[i].position = snakeSegments[i - 1].position;
                        }
                    }
                    // Move head
                    lastPosition = transform.position;
                    transform.Translate(movementDirection);
                    // Decrease tick
                    tick -= maxTick;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transAnimator.SetBool("SceneEnded", !alive);
        snakeAnimator.SetBool("Dead", !alive);
        if (transAnimator.GetCurrentAnimatorStateInfo(0).IsName("TransOut"))
        {
            if (transAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                SceneManager.LoadScene("Menu");
            }
        }
        if (alive)
        {
            Debug.Log(Input.GetAxisRaw("Vertical"));
            if (Input.GetAxisRaw("Vertical") < -0.3f || Input.GetAxisRaw("Vertical") > 0.3f)
            {
                bool canMove = true;
                if (snakeSegments.Count > 1)
                {
                    var behindSegment = snakeSegments[1];
                    if (((behindSegment.position.y < transform.position.y) && (Input.GetAxisRaw("Vertical") < 0))
                        || ((behindSegment.position.y > transform.position.y) && (Input.GetAxisRaw("Vertical") > 0)))
                    {
                        canMove = false;
                    }
                }
                if (canMove)
                    movementDirection = new Vector2(0, Mathf.Sign(Input.GetAxisRaw("Vertical")));
            }
            // X - axis movement
            else if (Input.GetAxisRaw("Horizontal") > 0.3f || Input.GetAxisRaw("Horizontal") < -0.3f)
            {
                bool canMove = true;
                if (snakeSegments.Count > 1)
                {
                    var behindSegment = snakeSegments[1];
                    if (((behindSegment.position.x < transform.position.x) && (Input.GetAxisRaw("Horizontal") < 0))
                        || ((behindSegment.position.x > transform.position.x) && (Input.GetAxisRaw("Horizontal") > 0)))
                    {
                        canMove = false;
                    }
                }
                if (canMove)
                    movementDirection = new Vector2(Mathf.Sign(Input.GetAxisRaw("Horizontal")), 0);
            }
            // Wrap
        }
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

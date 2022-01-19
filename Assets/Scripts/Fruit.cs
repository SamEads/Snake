using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Collider2D bounds;
    public AudioClip fruitSound;
    public Transform particle;
    AudioSource fruitSource;

    // Start is called before the first frame update
    void Start()
    {
        ChangePosition();
        fruitSource = gameObject.AddComponent<AudioSource>();
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
            var _particle = Instantiate(particle);
            var newPos = transform.position;
            newPos.z -= 3;
            _particle.position = newPos;
            fruitSource.PlayOneShot(fruitSound);
            ChangePosition();
            collider.gameObject.GetComponent<Snake>().Eat();
        }
    }
}

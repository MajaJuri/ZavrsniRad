using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGroundScript : MonoBehaviour
{
    public float moveX;
    public float moveY;
    public float speed;
    private Vector2 position; // pozicija od koje se krece 
    private Vector2 target; // pozicija prema kojoj se krece
    private bool moveBack = false;

    // Start is called before the first frame update
    void Start()
    {
        // pocetna pozicija
        position.x = transform.position.x;
        position.y = transform.position.y;
        // pozicija prema kojoj se krece
        target.x = position.x + moveX;
        target.y = position.y + moveY;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (!moveBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, step);
            if(transform.position.x == target.x && transform.position.y == target.y)
            {
                moveBack = true;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, position, step);
            if (transform.position.x == position.x && transform.position.y == position.y)
            {
                moveBack = false;
            }
        }
    }
}

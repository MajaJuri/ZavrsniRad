using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    // Use this for initialization
    void Start()
    {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var left = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        var right = Camera.main.ViewportToWorldPoint(Vector3.one).x;
        var top = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        var bottom = Camera.main.ViewportToWorldPoint(Vector3.one).y;
        float x = transform.position.x, y = transform.position.y;
        objectWidth = transform.GetComponent<Renderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = transform.GetComponent<Renderer>().bounds.extents.y; //extents = size of height / 2
        if (transform.position.x <= left + objectWidth)
        {
            x = left + objectWidth;
        }
        else if (transform.position.x >= right - objectWidth)
        {
            x = right - objectWidth;
        }
        /*if (transform.position.y <= top + objectHeight)
        {
            y = top + objectHeight;
        }
        else if (transform.position.y >= bottom - objectHeight)
        {
            y = bottom - objectHeight;
        }*/
        transform.position = new Vector3(x, y, transform.position.z);
    }
}

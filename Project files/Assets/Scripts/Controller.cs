using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	public float speed = 7;

	private float screenHalfWidthInWorldUnits;
	public Color opaque;

	// Use this for initialization
	void Start()
	{
		float halfPlayerWidth = transform.localScale.x / 2f;
		screenHalfWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize + halfPlayerWidth;
	}

	// Update is called once per frame
	void Update()
	{
		float inputX = Input.GetAxisRaw("Horizontal");
		float velocity = inputX * speed;
		transform.Translate(Vector2.right * velocity * Time.deltaTime);

		if (transform.position.x < -screenHalfWidthInWorldUnits)
		{
			transform.position = new Vector2(screenHalfWidthInWorldUnits, transform.position.y);
		}

		if (transform.position.x > screenHalfWidthInWorldUnits)
		{
			transform.position = new Vector2(-screenHalfWidthInWorldUnits, transform.position.y);
		}

		if(transform.position.y < -(Camera.main.orthographicSize + transform.localScale.y / 2f))
        {
			FindObjectOfType<SpawnGameManager>().PlayWrong();
			SpawnSame();
		}
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		string itemName = gameObject.name;
		string kontejnerName = collision.gameObject.name;
		if (itemName.ToLower().Contains(kontejnerName.ToLower())) // correct
		{
			FindObjectOfType<SpawnGameManager>().Correct();
			Destroy(this); // unisti skriptu, ali gameobject i dalje postoji -> vise ga ne mozemo micati, ali se i dalje vidi -> dobro
		}
		else // incorrect
		{
			FindObjectOfType<SpawnGameManager>().Wrong();
			collision.enabled = false;
			collision.gameObject.GetComponent<Image>().color = opaque;
			FindObjectOfType<SpawnGameManager>().SpawnSame(gameObject);
			Destroy(gameObject); // unisti game object jer stvori novi iste vrste
		}
	}

	private void SpawnSame()
    {
		Vector2 spawnPosition = new Vector2(Random.Range(-(new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize)).x, (new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize)).x), (new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize)).y);
		GameObject newObject = (GameObject)Instantiate(gameObject, spawnPosition, Quaternion.Euler(Vector3.forward));
		newObject.GetComponent<Rigidbody2D>().gravityScale = PlayerPrefs.GetFloat("brzina");
		Destroy(this);
	}

}

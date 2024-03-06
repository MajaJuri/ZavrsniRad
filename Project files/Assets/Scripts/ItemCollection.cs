using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemCollection : MonoBehaviour
{
    private int called = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        called++;
        if (collision.GetComponent<Collider2D>().GetType().ToString() == "UnityEngine.CircleCollider2D")
        {
            Destroy(this.gameObject);
            if (SceneManager.GetActiveScene().name == "LevelA")
            {
                if (called <= 1)
                {
                    FindObjectOfType<GameManagerScript>().Collect(gameObject.name);
                }
            }
            else if (SceneManager.GetActiveScene().name == "InstructionsSceneA")
            {
                if (called <= 1)
                {
                    FindObjectOfType<TutorialManagerScript>().Collect();
                }
            }
        }
    }
}

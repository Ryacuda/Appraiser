using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private BoxCollider2D coll;
    [SerializeField] private int health;
    [SerializeField] private List<Effect> effects;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Traps")) 
        {
            // lose health
            health -= 10;

            // death
            if(health <= 0)
            {
                SceneManager.LoadScene("Start Menu");
            }
        }
    }
}

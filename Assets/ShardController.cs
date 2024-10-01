using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardController : MonoBehaviour
{

    KeyShardsSystem keyShardsSystem;
    // Start is called before the first frame update
    void Start()
    {
        keyShardsSystem = FindObjectOfType<KeyShardsSystem>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            keyShardsSystem.IncreaseScore(1);

            Destroy(gameObject);
        }
    }
}

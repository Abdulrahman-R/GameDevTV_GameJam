using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{

    [SerializeField] GameObject flameffect;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().TakeRangeDamage(1, "Fire");
            Instantiate(flameffect, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

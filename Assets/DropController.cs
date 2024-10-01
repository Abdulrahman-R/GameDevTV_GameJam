using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    bool canDrop;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canDrop = true;
    }

    private void Update()
    {
        //to change the dairection of the drop point
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) )
        {
            transform.localPosition = new Vector2(-0.5f, 0.225f);
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.localPosition = new Vector2(0.5f, 0.225f);
        }

        if (canDrop)
        {
            spriteRenderer.color = new Color(0f, 1f, 0f, 155f / 255f);
        }
        else
        {
            spriteRenderer.color = new Color(1f, 0f, 0f, 155f / 255f);
        }
    }
    public bool GetCanDrop()
    {
        return canDrop;
    }
    public void SetCanDrop(bool newValue)
    {
        canDrop = newValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle") || collision.CompareTag("Enemy"))
        {
            canDrop = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Enemy"))
        {
            canDrop = true;
        }
    }
}

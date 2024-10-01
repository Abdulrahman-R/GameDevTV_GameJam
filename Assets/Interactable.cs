using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    GameObject player;
    bool interactable;
    [SerializeField] GameObject[] interactionCanvas;
    [SerializeField] GameObject objToSpawn;
    bool canInteract;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        interactable = true;
        canInteract = true;
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    
    // Update is called once per frame

    
    void Update()
    {
        if (player != null)
        {
            canInteract = player.GetComponent<InteractionSystem>().canInteract;

            if (canInteract)
            {
                interactionCanvas[0].SetActive(true);
                interactionCanvas[1].SetActive(false);
            }
            else
            {
                interactionCanvas[1].SetActive(true);
                interactionCanvas[0].SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                player.GetComponent<InteractionSystem>().PickUp(gameObject);
                interactionCanvas[1].SetActive(false);
                interactionCanvas[0].SetActive(false);
                interactable = false;
            }

        }

        if (interactable)
        {
            anim.enabled = true;
        }
        else
        {
            anim.Rebind();
            anim.enabled = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!interactable) return;
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.GetComponent<InteractionSystem>().dropDownPoint
                .GetComponent<DropController>().SetCanDrop(false);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<InteractionSystem>().dropDownPoint
                .GetComponent<DropController>().SetCanDrop(true);
            player = null;
            interactionCanvas[1].SetActive(false);
            interactionCanvas[0].SetActive(false);
        }
    }

    public void DropedDown(Transform dropPos)
    {
        Instantiate(objToSpawn, dropPos.position , Quaternion.identity);
        Destroy(gameObject);
    }


    public void setInteractable(bool newValue)
    {
        interactable = newValue;
    }
}

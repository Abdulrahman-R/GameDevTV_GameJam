using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] Transform pickUpPoint;
    [SerializeField] Transform releasePoint;
    public Transform dropDownPoint;
    [SerializeField] GameObject pickedObj;
    DropController dropController;
    PlayerStats playerStats;

    [HideInInspector] public bool canInteract;
    [SerializeField] bool isPicked;

    CameraShake cameraShake;

    SpriteRenderer playerGFXRenderer;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        canInteract = true;
        isPicked = false;
        pickedObj = null;

        playerStats = GetComponent<PlayerStats>();
        dropController = dropDownPoint.GetComponent<DropController>();
        cameraShake = FindObjectOfType<CameraShake>();

        playerGFXRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isPicked || playerStats.effectApplied)
        {
            canInteract = false;

            // Check if the "E" key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropDown();
            }
        }
        else
        {
            canInteract = true;
        }
    }


    public void PickUp(GameObject obj)
    {
        if (!isPicked)
        {
            audioManager.PlaySound("drop");
            // you can trigger animation here
            isPicked = true;
            pickedObj = obj;
            pickedObj.transform.position = pickUpPoint.position;
            pickedObj.transform.parent = transform.GetChild(0).transform;
            pickedObj.transform.localPosition = pickedObj.transform.localPosition + new Vector3(0f, 0f, -11);
            CinemachineShake.Instance.ShakeCamera(2f, 2f, 0.5f);
            playerStats.ChangePlayerSpeed(0.75f);
            dropDownPoint.GetComponent<SpriteRenderer>().enabled = true;
            playerGFXRenderer.sprite = playerStats.playerGFXSats[1];

            //pick up
        }
    }


    public void DropDown()
    {
        if (isPicked)
        {
            if (dropController.GetCanDrop())
            {
                audioManager.PlaySound("drop");
                //drop the object
                CinemachineShake.Instance.ShakeCamera(2f, 2f, 0.5f);
                playerStats.ChangePlayerSpeed(1f);
                pickedObj.GetComponent<Interactable>().DropedDown(dropDownPoint);
                dropDownPoint.GetComponent<SpriteRenderer>().enabled = false;
                pickedObj = null;
                isPicked = false;
                playerGFXRenderer.sprite = playerStats.playerGFXSats[0];
            }
        }
    }

    public void Release()
    {
        if (!isPicked) return;
        CinemachineShake.Instance.ShakeCamera(2f, 2f, 0.5f);
        playerStats.ChangePlayerSpeed(1f);
        pickedObj.transform.position = releasePoint.position;
        pickedObj.transform.localPosition = pickedObj.transform.localPosition + new Vector3(0f, 0f, -11);
        pickedObj.transform.parent = null;
        pickedObj.GetComponent<Interactable>().setInteractable(true);
        dropDownPoint.GetComponent<SpriteRenderer>().enabled = false;
        pickedObj = null;
        isPicked = false;
        playerGFXRenderer.sprite = playerStats.playerGFXSats[0];
        audioManager.PlaySound("drop");
    }


    public bool GetIsPicked()
    {
        return isPicked;
    }
}

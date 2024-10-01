using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomTemplates : MonoBehaviour
{
    AstarPath astarPath;
    CameraManager cameraManager;
    [SerializeField] CinemachineVirtualCamera playerCam;

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;

    public List<GameObject> rooms; // List to keep track of spawned rooms
    public int maxRooms = 10; // Maximum number of rooms to be generated

    public float waitTime;
    private bool spawnedPlayer;
    public GameObject player;

    void Start()
    {
        astarPath = FindObjectOfType<AstarPath>();
        cameraManager = FindAnyObjectByType<CameraManager>();
        rooms = new List<GameObject>();
        Invoke("CloseOpenRooms", 5f); // Call the method to close open rooms after generation
    }

    void Update()
    {

        if (waitTime <= 0 && spawnedPlayer == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    GameObject playerReff = Instantiate(player, rooms[i].transform.position, Quaternion.identity);
                    playerCam.Follow = playerReff.transform;
                    if(cameraManager != null)
                    {
                        cameraManager.SwitchCamera(playerCam);
                    }
                    spawnedPlayer = true;
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    void CloseOpenRooms()
    {
        foreach (GameObject room in rooms)
        {
            if (room != null)
            {
                RoomSpawner[] spawners = room.GetComponentsInChildren<RoomSpawner>();
                foreach (RoomSpawner spawner in spawners)
                {
                    if (!spawner.spawned)
                    {
                        Instantiate(closedRoom, spawner.transform.position, Quaternion.identity);
                        spawner.spawned = true;
                    }
                }
            }
        }
        astarPath.Scan();// to be changed
    }
}

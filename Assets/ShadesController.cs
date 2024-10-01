using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadesController : MonoBehaviour
{

    [SerializeField] GameObject[] shades;

    bool shadesLocked;

    private void Start()
    {
        shadesLocked = false;
    }


    public void LockAllShades()
    {
        if (shadesLocked) return;
        if (shades.Length <= 0) return;
        shadesLocked = true;
        for (int i = 0; i < shades.Length; i++)
        {
            shades[i].SetActive(false);
        }
    }

    public void UnlockAllShades()
    {
        if (!shadesLocked) return;
        if (shades.Length <= 0) return;
        shadesLocked = false;
        for (int i = 0; i < shades.Length; i++)
        {
            shades[i].SetActive(true);
        }
    }
}

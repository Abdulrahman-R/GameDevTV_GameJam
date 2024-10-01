using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    private void Start()
    {
        Invoke("SwicthOff", 4f);
    }
    void OnTriggerEnter2D(Collider2D other){
		Destroy(other.gameObject);
		
	}

    void SwicthOff()
    {
        gameObject.SetActive(false);
    }
}

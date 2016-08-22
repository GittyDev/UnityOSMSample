using UnityEngine;
using System.Collections;

public class Pave : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<MeshRenderer> ().sortingLayerName = "Pave";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

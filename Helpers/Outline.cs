using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Outline : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<MeshRenderer> ().sortingLayerName = "Outline";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

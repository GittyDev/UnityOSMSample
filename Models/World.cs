using System;
using System.Collections.Generic;
using Assets;
using Assets.Helpers;
using Assets.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Models.Factories;

public class World : MonoBehaviour
{
    [SerializeField] private Settings _settings;
    private TileManager _tileManager;	//This is actually Dynamic Tile Manager object in this scene.
    private BuildingFactory _buildingFactory;
    private RoadFactory _roadFactory;

	//GPS infos
	public float mLatitude;
	public float mLongitude;
	public float mAltitude;
	public float mHorizontalAccuracy;
	public double mTimestamp;
	public bool mGpsInited = false;
	public Text dOut;
	public Text dOut1;
	public Text dOut2;

	public bool mIsTest = false;

	public GameObject gPlayer;
	public GameObject gNewPos;

	public Transform gMonster1;
	public Transform gMonster2;
	public Transform gMonster3;

	private float mTick = 0f;

    void Start ()
    {
        _buildingFactory = GetComponentInChildren<BuildingFactory>();
        _roadFactory = GetComponentInChildren<RoadFactory>();
	    
        _tileManager = GetComponent<TileManager>();	//Obtain extended class: DynamicTilemanager instead of base class: TileManager


		if (mIsTest) {
			_tileManager.Init(_buildingFactory, _roadFactory, _settings);
			setPlayerPostoStart();
			DOUT ("La:" + _settings.Lat + "  Lo:" + _settings.Long);
		} else {
			StartCoroutine (startGpsService ());
		}
	}

	void setPlayerPostoStart(){
		var v2 = GM.LatLonToMeters(_settings.Lat, _settings.Long);
		Vector3 nPos = new Vector3 (v2.x - _tileManager.CenterInMercator.x, 0, v2.y - _tileManager.CenterInMercator.y);
		gPlayer.GetComponent<Transform> ().position = nPos;
		gNewPos.GetComponent<Transform> ().position = nPos;
		gMonster1.GetComponent<Transform> ().position = new Vector3 (nPos.x + UnityEngine.Random.Range (-100, 100), 0, nPos.y + UnityEngine.Random.Range (-100, 100));
		gMonster2.GetComponent<Transform> ().position = new Vector3 (nPos.x + UnityEngine.Random.Range (-100, 100), 0, nPos.y + UnityEngine.Random.Range (-100, 100));
		gMonster3.GetComponent<Transform> ().position = new Vector3 (nPos.x + UnityEngine.Random.Range (-100, 100), 0, nPos.y + UnityEngine.Random.Range (-100, 100));
	}

	void Update () {
		mTick += Time.deltaTime;
		if (mTick >= 1) {
			if (mGpsInited) {
				getGPS ();
				var v2 = GM.LatLonToMeters (mLatitude, mLongitude);
				Vector3 nPos = new Vector3 (v2.x - _tileManager.CenterInMercator.x, 0, v2.y - _tileManager.CenterInMercator.y);
				gNewPos.GetComponent<Transform> ().position = nPos;
				dOut1.text = "Nx:" + gPlayer.GetComponent<Transform> ().position.x + "  y:" + gPlayer.GetComponent<Transform> ().position.y + "  z:" + gPlayer.GetComponent<Transform> ().position.z;
				dOut2.text = "Px:" + gNewPos.GetComponent<Transform> ().position.x + "  y:" + gNewPos.GetComponent<Transform> ().position.y + "  z:" + gNewPos.GetComponent<Transform> ().position.z;
			}
			mTick = 0;
		}
		gMonster1.LookAt(gPlayer.GetComponent<Transform> ().position);
		gMonster2.LookAt(gPlayer.GetComponent<Transform> ().position);
		gMonster3.LookAt(gPlayer.GetComponent<Transform> ().position);
	}

    [Serializable]
    public class Settings
    {
        [SerializeField]
        public float Lat = 41.77282f;
        [SerializeField]
        public float Long = 123.4743f;
        [SerializeField]
        public int Range = 3;
        [SerializeField]
        public int DetailLevel = 16;
        [SerializeField]
        public bool LoadImages = false;
    }

	IEnumerator startGpsService(){
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break;

		// Start service before querying location
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			DOUT("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			DOUT("Unable to determine device location");
			yield break;
		}
		else
		{
			// Access granted and location value could be retrieved
			mGpsInited = true;
			getGPS ();

			_settings.Lat = mLatitude;
			_settings.Long = mLongitude;

			//moved from start
			_tileManager.Init(_buildingFactory, _roadFactory, _settings);
			setPlayerPostoStart();
		}

	}

	void getGPS(){
		mLatitude = Input.location.lastData.latitude;
		mLongitude = Input.location.lastData.longitude;
		mAltitude = Input.location.lastData.altitude;
		mHorizontalAccuracy = Input.location.lastData.horizontalAccuracy;
		mTimestamp = Input.location.lastData.timestamp;
		DOUT("Location: " + mLatitude + " " + mLongitude + " " + mAltitude + " " + mHorizontalAccuracy + " " + mTimestamp);
	}

	void exitService(){
		Input.location.Stop ();
		DOUT ("EndGpsService");
	}

	public void DOUT(string str){
		Debug.Log (str);
		if (dOut != null) {
			dOut.text = str;
		}
	}
}

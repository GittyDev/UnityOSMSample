using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GPSposition : MonoBehaviour {

	public float mLatitude;
	public float mLongitude;
	public float mAltitude;
	public float mHorizontalAccuracy;
	public double mTimestamp;

	public bool mGpsInited = false;

	public Text dOut;

	// Use this for initialization
	void Start () {
		DOUT ("Getting GPS");
		StartCoroutine (startGpsService());
	}

	// Update is called once per frame
	void Update () {
		if (mGpsInited) {
			getGPS ();
		}
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
            DOUT("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
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

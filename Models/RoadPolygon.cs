using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets
{
    public enum RoadType
    {
        Path,
        Rail,
        MinorRoad,
        MajorRoad,
        Highway,
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    internal class RoadPolygon : MonoBehaviour
    {
        public string Id { get; set; }
        public RoadType Type { get; set; }
        private List<Vector3> _verts;
        
        public void Initialize(string id, Vector3 tile, List<Vector3> verts, string kind)
        {
            Id = id;
            Type = kind.ToRoadType();
			int itype = (int)Type;
			if (itype == 0)
				itype++;
            _verts = verts;

            for (int index = 1; index < _verts.Count; index++)
            {
				{
					var roadPlane = Instantiate (Resources.Load<GameObject> ("RoadQuad"));
					//roadPlane.GetComponentInChildren<MeshRenderer> ().material = Resources.Load<Material> ("Road");
					Vector3 apos = (tile + verts [index] + tile + verts [index - 1]) / 2;
					roadPlane.transform.position = apos;

					roadPlane.transform.SetParent (transform, true);
					Vector3 scale = roadPlane.transform.localScale;
					scale.z = Vector3.Distance (verts [index], verts [index - 1]) / 10;
					scale.x = ((float)(int)itype + 1) / 4;

					scale.z += ((float)(int)itype + 1) / 10;

					roadPlane.transform.localScale = scale;
					roadPlane.transform.LookAt (tile + verts [index - 1]);

					apos = roadPlane.transform.position;
					apos.Set (apos.x, apos.y + 0.1f, apos.z);
					roadPlane.transform.position = apos;
				}
				{
					var roadPlane = Instantiate (Resources.Load<GameObject> ("RoadQuad_out"));
					//roadPlane.GetComponentInChildren<MeshRenderer> ().material = Resources.Load<Material> ("Road_Out");
					roadPlane.transform.position = (tile + verts [index] + tile + verts [index - 1]) / 2;
					roadPlane.transform.SetParent (transform, true);
					Vector3 scale = roadPlane.transform.localScale;
					scale.z = Vector3.Distance (verts [index], verts [index - 1]) / 10;
					scale.x = ((float)(int)itype + 1) / 4 + 0.3f;

					scale.z += ((float)(int)itype + 1) / 10;

					roadPlane.transform.localScale = scale;
					roadPlane.transform.LookAt (tile + verts [index - 1]);
				}
            }
        }
    }
}

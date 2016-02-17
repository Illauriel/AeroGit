using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

	public Vector3 cur_wind;
	public Transform wind_zone;
	public float[] heights;
	public Vector3[] streams;
	public Color[] cols;
	public float[] wind_prob;
	public float[] b_scale;
	public Color[] scale_cols;
	public bool enabled;
	WindZone zone;

	void Start(){
		cols = new Color[heights.Length];
		if (wind_prob.Length<13){
			Debug.LogError ("The length of wind gauge is faulty");
		}
		float e_prob = 0;
		foreach(float x in wind_prob){
			e_prob += x;
		}
		// generate weather forecast;

		for (int i = 0; i < heights.Length; i++) {
			float chance = Random.Range(0, e_prob);
			float cur_prob = 0;
			for (int j = 0; j < 13; j++) {
				cur_prob += wind_prob[j];
				if (chance < cur_prob){
					float angle = Random.Range(0,360) * Mathf.Deg2Rad;

					float x = Mathf.Sin (angle);
					float z = Mathf.Cos (angle);
					float mag = 1;
					if (j == 0){
						mag = Random.Range(0, b_scale[0]);
					}
					else if (j == 12){
						mag = Random.Range(b_scale[12], b_scale[12]+8);
					}
					else {
						mag = Random.Range(b_scale[j-1], b_scale[j]);
					}
					streams[i] = new Vector3(x * mag, heights[i], z * mag);
					Debug.Log(i+". Wind of strength " + j + " with magnitude "+mag+" results in "+streams[i]);

					cols[i] = scale_cols[j];
					break;
				}
			}

			
		}

		zone = wind_zone.GetComponent<WindZone>();
	}
	void Update(){
		zone.windMain = cur_wind.magnitude;
		//zone.
		wind_zone.transform.rotation = Quaternion.LookRotation(new Vector3(cur_wind.x, 0, cur_wind.z));
	}

	public Vector3 GetWindAtAltitude(float alt){
		Vector3 result = Vector3.zero;
		if (alt < streams[0].y){
			result = new Vector3(streams[0].x, 0, streams[0].z);
			//Debug.Log("Low alt "+Quaternion.LookRotation(result).eulerAngles+" and "+result);
		}
		else if (alt > streams[streams.Length-1].y){
			result = new Vector3(streams[streams.Length-1].x, 0, streams[streams.Length-1].z);
			//Debug.Log("High alt "+Quaternion.LookRotation(result).eulerAngles+" and "+result);
		}
		else{
			for (int i = 1; i < streams.Length; i++) {
				if (alt < streams[i].y){
					float x = Mathf.Lerp (streams[i-1].x, streams[i].x, (alt-streams[i-1].y)/(streams[i].y-streams[i-1].y));
					float z = Mathf.Lerp (streams[i-1].z, streams[i].z, (alt-streams[i-1].y)/(streams[i].y-streams[i-1].y));
					result = new Vector3(x, 0, z);

					//Debug.Log("Norm alt "+i+": "+Quaternion.LookRotation(result).eulerAngles+" and "+result+ " and "+streams[i-1].z + " ?? " +streams[i].z+ " ?? " +  ((alt-streams[i-1].y)/(streams[i].y-streams[i-1].y)));
					break;
				}
			}
		}

		return result;
	}
}

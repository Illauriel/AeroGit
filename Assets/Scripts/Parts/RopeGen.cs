using UnityEngine;
using System.Collections;

public class RopeGen : MonoBehaviour {

	public int joint_no;
	public Material mat;
	public Transform target;
	public float length;
	public GameObject marker;
	public PhysicMaterial phm;
	public float toughess;
	DrawRope draw;
	// Use this for initialization
	void Start () {
		LineRenderer lr = gameObject.AddComponent<LineRenderer>();
		lr.material = mat;
		lr.SetWidth(0.03f, 0.03f);

		draw = gameObject.AddComponent<DrawRope>();
		draw.links = new Transform[joint_no+1];

		Generate(joint_no);
		//Physics.IgnoreLayerCollision(8, 8);
	}

	void Generate(int no){
		float unit = 0;
		Vector3 vec = Vector3.zero; // find a normalized unit vector between my transform and target
		if (target != null){
			vec = target.position - transform.position;
			vec = vec / vec.magnitude;

			unit = Vector3.Distance(target.position, transform.position) / no;
		}
		else if (length > 0){
			vec = transform.position + (Vector3.down*length) - transform.position;
			vec = vec / vec.magnitude;
			
			unit = length / no;
		}
		else {
			Debug.LogError("ACHTUNG no rope ending");
		}
		for (int i = 0; i < no+1; i++) {
			GameObject joint = new GameObject("joint"+i);
			joint.layer = 8;
			joint.transform.position = transform.position + vec * unit * i;
			draw.links[i] = joint.transform;
			//Instantiate (marker, joint.transform.position, Quaternion.identity);

			Rigidbody rigidbody = joint.AddComponent<Rigidbody>();
			ConfigurableJoint cj = joint.AddComponent<ConfigurableJoint>();
			if (target != null && i == no){
				//FixedJoint fj = joint.AddComponent<FixedJoint>();
				//fj.connectedBody = target.GetComponent<Rigidbody>();
				ConfigurableJoint jjj = target.gameObject.AddComponent<ConfigurableJoint>();
				jjj.connectedBody = rigidbody;
				Configure(jjj);
				SetBreakPrc(jjj, 20);

			}
			rigidbody.mass = 0.2f;
			rigidbody.drag = 0.5f;
			rigidbody.angularDrag = 0.5f;
			rigidbody.useGravity = true;


			Configure(cj);

			if (i == 0){
				Rigidbody root = gameObject.GetComponent<Rigidbody>();
				if (root == null){
					root = gameObject.AddComponent<Rigidbody>();
				}
				//root.useGravity = false;
				//root.isKinematic = true;
				joint.transform.parent = transform;
				cj.connectedBody = root;
				SetBreakPrc(cj, 20);
				/*FixedJoint fj = gameObject.AddComponent<FixedJoint>();
				fj.connectedBody = gameObject.transform.parent.GetComponent<Rigidbody>();*/
				if (cj.connectedBody == null){
					Debug.LogError("warning, no rigidbody");
				}
			}
			else{
				joint.transform.parent = draw.links[i-1];
				cj.connectedBody = joint.transform.parent.gameObject.GetComponent<Rigidbody>();
				/*if (i != no){
					SphereCollider coll = joint.AddComponent<SphereCollider>();
					//coll.height = unit * 0.9f;
					coll.center = Vector3.down * (unit/2);
					coll.radius = 0.03f;
					coll.material = phm;

				}*/

			}


		}
		/*SphereCollider[] colls = gameObject.GetComponentsInChildren<SphereCollider>();
		foreach(Collider x in colls){
			for (int i = 0; i < colls.Length; i++) {
				Physics.IgnoreLayerCollision();	
			}

		}*/
	}
	void Configure (ConfigurableJoint cj){
		//cj.autoConfigureConnectedAnchor = false;
		//cj.connectedAnchor = Vector3.zero;
		//cj.anchor = Vector3.zero;

		cj.xMotion = ConfigurableJointMotion.Locked;
		cj.yMotion = ConfigurableJointMotion.Locked;
		cj.zMotion = ConfigurableJointMotion.Locked;

		cj.projectionMode = JointProjectionMode.PositionAndRotation;
		//cj.enableCollision = true;
		/*if(Random.Range(0,100) < 20){
			cj.breakForce = 500 + Random.Range(0,200);
		}*/
	}
	void SetBreakPrc(ConfigurableJoint joint, float prc){
		if(Random.Range(0,100) < prc){
			joint.breakForce = toughess - 300 + Random.Range(0,200);
		}
		else{
			joint.breakForce = toughess;
		}
	}
}

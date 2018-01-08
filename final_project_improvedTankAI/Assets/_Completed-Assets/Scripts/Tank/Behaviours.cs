using UnityEngine;
using NPBehave;
using System.Collections.Generic;

namespace Complete
{
    public partial class TankAI : MonoBehaviour
    {
		//private Blackboard ownBlackboard = new Blackboard();

        private Root CreateBehaviourTree() {

            switch (m_Behaviour) {

                case 1:
					//Enemy travel around the map and seems harmless.
					return LostBehaviour(); 
					

                default:
                    return new Root (new Action(()=> Turn(0.1f)));
            }
        }


        /* My Actions */

		//From coursework 1
        private Node RandomFire() {
            return new Action(() => Fire(UnityEngine.Random.Range(0.0f, 1.0f)));
        }


		/*My methods*/

		//Uses 6 sensors to create awareness of the the surrounding.
		private void Awareness(){
			//Debug.Log("EYE OF SIGHT METHOD:");
			Vector3 localPos = this.transform.position;
			Vector3 obsticle;
			Vector3 collHit;
			Vector3 rightEye = Quaternion.Euler(0,30,0) * this.transform.forward;
			Vector3 leftEye = Quaternion.Euler(0,-30,0) * this.transform.forward;
			//Vector3 rightEye = this.transform.position.new Vector3(localPos.x*0.5f, 0, 0);
			RaycastHit hit;	

			//Eyes forward
			Ray eyesFwd = new Ray(this.transform.position, this.transform.forward);
			//Eyes forward 45 degrees to the right
			Ray eyesFwdRight = new Ray(this.transform.position, rightEye);
			//Eyes forward 45 degrees to the left
			Ray eyesFwdLeft = new Ray(this.transform.position, leftEye);
			//Eyes to the back
			Ray eyesBck= new Ray(this.transform.position, -this.transform.forward);
			//Eyes to back 45 on the right
			Ray eyesBckRight = new Ray(this.transform.position, -leftEye);
			//Eyes to the back 45 on the left
			Ray eyesBckLeft= new Ray(this.transform.position, -rightEye);

			//Debug rays
			Debug.DrawRay(this.transform.position, this.transform.forward * 7, Color.red);

			Debug.DrawRay(this.transform.position, rightEye * 7, Color.green);
			Debug.DrawRay(this.transform.position, leftEye * 7, Color.blue);
			Debug.DrawRay(this.transform.position, -this.transform.forward * 7, Color.black);
			Debug.DrawRay(this.transform.position, -rightEye * 7, Color.white);
			Debug.DrawRay(this.transform.position, -leftEye * 7, Color.yellow);

			//Debug.DrawRay(eyesRight * 10, Color.red);
			if (Physics.Raycast (eyesFwd, out hit, 10) && hit.collider.gameObject.name != "CompleteTank(Clone)") {
				collHit = hit.point;
				obsticle = hit.transform.position;

				//Debug.Log("Too close!!" + hit.collider);
				//Debug.Log("Object hit: " + obsticle);
				//Debug.Log("Coll hit: " + collHit);
				//Debug.Log(collHit.z-obsticle.z);
			
				Debug.Log ("Too close in front!!" + hit.collider);
				blackboard ["obsticleInFront"] = true;
			} else {
				blackboard["obsticleInFront"]= false;
			}
			if (Physics.Raycast (eyesFwdRight, out hit, 10) && hit.collider.gameObject.name != "CompleteTank(Clone)") {
				collHit = hit.point;
				obsticle = hit.transform.position;

				//Debug.Log("Too close!!" + hit.collider);
				//Debug.Log("Object hit: " + obsticle);
				//Debug.Log("Coll hit: " + collHit);
				//Debug.Log(collHit.z-obsticle.z);

				Debug.Log ("Too close to the right!!" + hit.collider);
				blackboard ["obsticleInFrontRight"] = true;
			} else {
				blackboard["obsticleInFrontRight"]= false;
			}
			if (Physics.Raycast (eyesFwdLeft, out hit, 10) && hit.collider.gameObject.name != "CompleteTank(Clone)") {
				collHit = hit.point;
				obsticle = hit.transform.position;

				//Debug.Log("Too close!!" + hit.collider);
				//Debug.Log("Object hit: " + obsticle);
				//Debug.Log("Coll hit: " + collHit);
				//Debug.Log(collHit.z-obsticle.z);

				Debug.Log ("Too close to the left!!" + hit.collider);
				blackboard ["obsticleInFrontLeft"] = true;
			} else {
				blackboard["obsticleInFrontLeft"]= false;
			}
			if (Physics.Raycast (eyesBck, out hit, 7) && hit.collider.gameObject.name != "CompleteTank(Clone)") {
				collHit = hit.point;
				obsticle = hit.transform.position;

				//Debug.Log("Too close!!" + hit.collider);
				//Debug.Log("Object hit: " + obsticle);
				//Debug.Log("Coll hit: " + collHit);
				//Debug.Log(collHit.z-obsticle.z);

				Debug.Log ("Too close to the left!!" + hit.collider);
				blackboard ["obsticleInBack"] = true;
			} else {
				blackboard["obsticleInBack"]= false;
			}

			if (Physics.Raycast (eyesBckRight, out hit, 7) && hit.collider.gameObject.name != "CompleteTank(Clone)") {
				collHit = hit.point;
				obsticle = hit.transform.position;

				//Debug.Log("Too close!!" + hit.collider);
				//Debug.Log("Object hit: " + obsticle);
				//Debug.Log("Coll hit: " + collHit);
				//Debug.Log(collHit.z-obsticle.z);

				Debug.Log ("Too close to the left!!" + hit.collider);
				blackboard ["obsticleInBackRight"] = true;
			} else {
				blackboard["obsticleInBackRight"]= false;
			}

			if (Physics.Raycast (eyesBckLeft, out hit, 7) && hit.collider.gameObject.name != "CompleteTank(Clone)") {
				collHit = hit.point;
				obsticle = hit.transform.position;

				//Debug.Log("Too close!!" + hit.collider);
				//Debug.Log("Object hit: " + obsticle);
				//Debug.Log("Coll hit: " + collHit);
				//Debug.Log(collHit.z-obsticle.z);

				Debug.Log ("Too close to the left!!" + hit.collider);
				blackboard ["obsticleInBackLeft"] = true;
			} else {
				blackboard["obsticleInBackLeft"]= false;
			}

		}


        /* My Behaviour Tree */

		private Root LostBehaviour() {
			return new Root(
				new Service(0.2f, UpdatePerception,
					//Steering tree
					new Selector(
						//Reached dead end
						new BlackboardCondition("deadEnd", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Sequence( 
								//Stop or reverse
								new Selector(
									new NPBehave.Random(0.5f, new Action(() => Move(0f))), 
									new NPBehave.Random(1f, new TimeMin(1f,new Action(() => Move(-0.3f))) )
								),
								//Turn left or right
								new Selector(
									new NPBehave.Random(0.5f, new TimeMin(4f, new Action(() => Turn(0.5f)))), 
									new NPBehave.Random(1f, new TimeMin(4f,new Action(() => Turn(-0.5f))) ) 
								)
							)
						),
						//Slows down and Perform sharp left turn
						new BlackboardCondition("turnLeft", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Sequence( 
								new Action(()=> Move(0.3f)), 
								new Action(() => Turn(-0.5f))
							)
						),
						//Slows down and perform sharp right turn
						new BlackboardCondition("turnRight", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Sequence( 
								new Action(()=> Move(0.3f)), 
								new Action(() => Turn(0.5f))
							)
						),
						//Obsticle in front turn left or right
						new BlackboardCondition("obsticleInFront", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Selector(
								new TimeMin(0.3f, new NPBehave.Random(0.5f, new Action(() => Turn(-0.5f)))), 
								new TimeMin(0.3f, new NPBehave.Random(1f, new Action(() => Turn(0.5f))))
							)
						),
						//Perform smooth left turn
						new BlackboardCondition("smoothTurnLeft", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Selector(
								new NPBehave.Random(0.5f, new Action(() => Turn(-0.1f))),new NPBehave.Random(1f, new Action(() => Turn(-0.3f)))
							)
						),
						//Perform smooth right turn
						new BlackboardCondition("smoothTurnRight", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Selector(
								new NPBehave.Random(0.5f, new Action(() => Turn(0.1f))),
								new NPBehave.Random(1f, new Action(() => Turn(0.3f)))
							)
						),
						//Path is clear Go straight ahead
						new Sequence(
							new Action(() => Turn(0f)), 
							new Action(() => Move(0.5f))
						)
					)
				)
			); 
		}

        private void UpdatePerception() {
			//Update sensors blackboards
			Awareness();
            
			/*From course behaviour script*/
			Vector3 targetPos = TargetTransform().position;
            Vector3 localPos = this.transform.InverseTransformPoint(targetPos);
            Vector3 heading = localPos.normalized;
            blackboard["targetDistance"] = localPos.magnitude;
            blackboard["targetInFront"] = heading.z > 0;
            blackboard["targetOnRight"] = heading.x > 0;
            blackboard["targetOffCentre"] = Mathf.Abs(heading.x);


			//My additions
			//Path is clear
			if (!blackboard.Get<bool> ("obsticleInFront") && !blackboard.Get<bool> ("obsticleInFrontRight") && !blackboard.Get<bool> ("obsticleInFrontLeft")) {
				blackboard ["deadEnd"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["turnRight"] = false;
				blackboard ["smoothTurnLeft"] = false;
				blackboard ["smoothTurnRight"] = false;
				Debug.Log ("Clear");
			} 
			//Deadend reached
			if(blackboard.Get<bool>("obsticleInFront") && blackboard.Get<bool>("obsticleInFrontRight") && blackboard.Get<bool>("obsticleInFrontLeft")){
				blackboard ["deadEnd"] = true;

				blackboard ["turnRight"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["smoothTurnLeft"] = false;
				blackboard ["smoothTurnRight"] = false;
				Debug.Log ("DeadEnd");
			}
			//Obstiles to the front and to the right
			if(blackboard.Get<bool>("obsticleInFront") && blackboard.Get<bool>("obsticleInFrontRight") && !blackboard.Get<bool>("obsticleInFrontLeft")){
				blackboard ["turnLeft"] = true;

				blackboard ["deadEnd"] = false;
				blackboard ["turnRight"] = false;
				blackboard ["smoothTurnLeft"] = false;
				blackboard ["smoothTurnRight"] = false;
				Debug.Log ("Left");
			}
			//Obstiles to the front and to the left
			if (blackboard.Get<bool> ("obsticleInFront") && !blackboard.Get<bool> ("obsticleInFrontRight") && blackboard.Get<bool> ("obsticleInFrontLeft")) {
				blackboard ["turnRight"] = true;

				blackboard ["deadEnd"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["smoothTurnLeft"] = false;
				blackboard ["smoothTurnRight"] = false;
				Debug.Log ("Right");
			} 
			//Obsticles to the right
			if (!blackboard.Get<bool> ("obsticleInFront") && blackboard.Get<bool> ("obsticleInFrontRight") && !blackboard.Get<bool> ("obsticleInFrontLeft")) {
				blackboard ["smoothTurnLeft"] = true;

				blackboard ["deadEnd"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["turnRight"] = false;
				blackboard ["smoothTurnRight"] = false;
				Debug.Log ("smooth left");
			}
			//Obstices to the left
			if (!blackboard.Get<bool> ("obsticleInFront") && !blackboard.Get<bool> ("obsticleInFrontRight") && blackboard.Get<bool> ("obsticleInFrontLeft")) {
				blackboard ["smoothTurnRight"] = true;

				blackboard ["deadEnd"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["turnRight"] = false;
				blackboard ["smoothTurnLeft"] = false;
				Debug.Log ("smooth right");
			}
			//Obsticles to the left and right
			if (!blackboard.Get<bool> ("obsticleInFront") && blackboard.Get<bool> ("obsticleInFrontRight") && blackboard.Get<bool> ("obsticleInFrontLeft")) {
				blackboard ["deadEnd"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["turnRight"] = false;
				blackboard ["smoothTurnLeft"] = false;
				blackboard ["smoothTurnRight"] = false;
				Debug.Log ("Tunnel");
			}



			//blackboard["obsticleBehind"]
        }
    }
}
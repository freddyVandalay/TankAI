using UnityEngine;
using NPBehave;
using System.Collections.Generic;

namespace Complete
{
    /*
    Example behaviour trees for the Tank AI.  This is partial definition:
    the core AI code is defined in TankAI.cs.

    Use this file to specifiy your new behaviour tree.
     */
    public partial class TankAI : MonoBehaviour
    {
		//private Blackboard ownBlackboard = new Blackboard();

        private Root CreateBehaviourTree() {

            switch (m_Behaviour) {

                case 1:
                    return SpinBehaviour(-0.05f, 1f);
                case 2:
                    return TrackBehaviour();
				case 3:
					return ImprovedBehaviour(); 

                default:
                    return new Root (new Action(()=> Turn(0.1f)));
            }
        }

        /* Actions */

        private Node StopTurning() {
            return new Action(() => Turn(0));
        }

        private Node RandomFire() {
            return new Action(() => Fire(UnityEngine.Random.Range(0.0f, 1.0f)));
        }

		/*My methods*/

		/*Eye of sight*/
		//Acts like a sensor for the AI tank
		private void Awareness(){
			blackboard["obsticleInFront"]= false;
			blackboard["obsticleInFrontRight"]= false;
			blackboard["obsticleInFrontLeft"]= false;
			blackboard["obsticleInFrontLeft"]= false;
			//blackboard["obsticleInFrontLeft"]= false;
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
			Debug.DrawRay(this.transform.position, rightEye * 10, Color.green);
			Debug.DrawRay(this.transform.position, leftEye * 10, Color.blue);
			Debug.DrawRay(this.transform.position, -this.transform.forward * 10, Color.black);
			Debug.DrawRay(this.transform.position, -rightEye * 10, Color.white);
			Debug.DrawRay(this.transform.position, -leftEye * 10, Color.yellow);

			//Debug.DrawRay(eyesRight * 10, Color.red);
			if (Physics.Raycast (eyesFwd, out hit, 10) && hit.collider.gameObject.name != "CompleteTank(Clone)") {
				collHit = hit.point;
				obsticle = hit.transform.position;

				//Debug.Log("Too close!!" + hit.collider);
				//Debug.Log("Object hit: " + obsticle);
				//Debug.Log("Coll hit: " + collHit);
				//Debug.Log(collHit.z-obsticle.z);

				//blackboard["obsticleOnRight"] = collHit.z - obsticle.z > 0;
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

				//blackboard["obsticleOnRight"] = collHit.z - obsticle.z > 0;
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

				//blackboard["obsticleOnRight"] = collHit.z - obsticle.z > 0;
				Debug.Log ("Too close to the left!!" + hit.collider);
				blackboard ["obsticleInFrontLeft"] = true;
			} else {
				blackboard["obsticleInFrontLeft"]= false;
			}
			/*
			if (Physics.Raycast(eyesFwdRight, out hit, 10) && hit.collider.gameObject.name != "CompleteTank(Clone)")
			{
				collHit = hit.point;
				obsticle = hit.transform.position;

				//Debug.Log("Too close!!" + hit.collider);
				//Debug.Log("Object hit: " + obsticle);
				//Debug.Log("Coll hit: " + collHit);
				//Debug.Log(collHit.z-obsticle.z);

				//blackboard["obsticleOnRight"] = collHit.z - obsticle.z > 0;
				Debug.Log("Too the right!!" + hit.collider);
				blackboard["obsticleToTheRight"]= true;
			}
			*/
		}


        /* Example behaviour trees */

        // Constantly spin and fire on the spot 
        private Root SpinBehaviour(float turn, float shoot) {
            return new Root(new Sequence(
                        new Action(() => Turn(turn)),
                        new Action(() => Fire(shoot))
                    ));
        }

        // Turn to face your opponent and fire
        private Root TrackBehaviour() {
            return new Root(
                new Service(0.2f, UpdatePerception,
                    new Selector(
                        new BlackboardCondition("targetOffCentre",
                                                Operator.IS_SMALLER_OR_EQUAL, 0.1f,
                                                Stops.IMMEDIATE_RESTART,
                            // Stop turning and fire
                            new Sequence(StopTurning(),
                                        new Wait(2f),
                                        RandomFire())),
                        new BlackboardCondition("targetOnRight",
                                                Operator.IS_EQUAL, true,
                                                Stops.IMMEDIATE_RESTART,
                            // Turn right toward target
                            new Action(() => Turn(0.2f))),
                            // Turn left toward target
                            new Action(() => Turn(-0.2f))
                    )
                )
            );
        }
	
		private Root ImprovedBehaviour() {
			return new Root(
				new Service(0.2f, UpdatePerception,
					new Selector(
						new BlackboardCondition("deadEnd", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Sequence( new Action(()=> Move(-0.5f)), new Action(() => Turn(0.5f))
							)
						),
						new BlackboardCondition("turnLeft", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Sequence( new Action(()=> Move(0f)), new Action(() => Turn(-0.5f))
							)
						),
						new BlackboardCondition("turnRight", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Sequence( new Action(()=> Move(0f)), new Action(() => Turn(0.5f))
							)
						),
						new BlackboardCondition("obsticleInFront", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
							new Selector(
								new BlackboardCondition("obsticleOnRight", Operator.IS_EQUAL,true, Stops.IMMEDIATE_RESTART,
									new Sequence(new Action (() => Turn(-0.5f)), new Action (() => Move(0f))
									)
								),
								new Action(() => Turn(0.5f))
							)
						),
						new Sequence(new Action(() => Turn(0f)), new Action(() => Move(0.5f))
						)
					)
				)
			); 
		}

        private void UpdatePerception() {
	
			Awareness();
            
			Vector3 targetPos = TargetTransform().position;
            Vector3 localPos = this.transform.InverseTransformPoint(targetPos);
            Vector3 heading = localPos.normalized;
            blackboard["targetDistance"] = localPos.magnitude;
            blackboard["targetInFront"] = heading.z > 0;
            blackboard["targetOnRight"] = heading.x > 0;
            blackboard["targetOffCentre"] = Mathf.Abs(heading.x);
			//Debug.Log (blackboard.Get<bool>("obsticleInFront"));
			//My additions
			if(blackboard.Get<bool>("obsticleInFront") && blackboard.Get<bool>("obsticleInFrontRight") && blackboard.Get<bool>("obsticleInFrontLeft")){
				blackboard ["turnRight"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["deadEnd"] = true;
				Debug.Log ("DeadEnd");
			}
			if(blackboard.Get<bool>("obsticleInFront") && blackboard.Get<bool>("obsticleInFrontRight") && !blackboard.Get<bool>("obsticleInFrontLeft")){
				blackboard ["deadEnd"] = false;
				blackboard ["turnRight"] = false;
				blackboard ["turnLeft"] = true;
				Debug.Log ("Left");
			}
			if (blackboard.Get<bool> ("obsticleInFront") && !blackboard.Get<bool> ("obsticleInFrontRight") && blackboard.Get<bool> ("obsticleInFrontLeft")) {
				blackboard ["deadEnd"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["turnRight"] = true;
				Debug.Log ("Right");
			} 
			if (!blackboard.Get<bool> ("obsticleInFront") && !blackboard.Get<bool> ("obsticleInFrontRight") && !blackboard.Get<bool> ("obsticleInFrontLeft")) {
				blackboard ["deadEnd"] = false;
				blackboard ["turnLeft"] = false;
				blackboard ["turnRight"] = false;
				Debug.Log ("Clear");
			} 



			//blackboard["obsticleBehind"]
        }
    }
}
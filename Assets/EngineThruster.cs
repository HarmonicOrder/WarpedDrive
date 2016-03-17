/*using UnityEngine;
using System.Collections;

public class EngineThruster : MonoBehaviour {
	
	public float idleRotateSpeed = 10f;
	public float engagedRotateSpeed = 20f;
	public float boostRotateSpeed = 40f;
	public float spinAcceleration = 1f;

	private float currentRotateSpeed = 0f;

	public ParticleSystem MainParticles;
	public ParticleSystem AfterburnerParticles;

	public enum EngineState { Idle, Engaged, Boost }

	public EngineState StartState = EngineState.Idle;
	private EngineState CurrentState;

	private float maxRotateSpeed {
		get{
			switch (CurrentState) {
			case EngineState.Engaged:
				return engagedRotateSpeed;
			case EngineState.Boost:
				return boostRotateSpeed;
			case EngineState.Idle:
				goto default;
			default:
				return idleRotateSpeed;
			}
		}
	}
	// Use this for initialization
	void Start () {
		CurrentState = StartState;
	}

	private float speedDiff;
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, currentRotateSpeed * Time.deltaTime);	

		speedDiff = maxRotateSpeed - currentRotateSpeed;
		if (speedDiff > 0f){
			currentRotateSpeed = Mathf.Min(currentRotateSpeed + spinAcceleration, maxRotateSpeed);
		} else if (speedDiff < 0f){
			currentRotateSpeed = Mathf.Max(currentRotateSpeed - spinAcceleration, idleRotateSpeed);
		}
	}

	public void ChangeEngineState(EngineState newState){
		CurrentState = newState;
		
		switch (CurrentState) {
		case EngineState.Engaged:
			MainParticles.enableEmission = true;
			AfterburnerParticles.enableEmission = false;
			break;
		case EngineState.Boost:
			MainParticles.enableEmission = true;
			AfterburnerParticles.enableEmission = true;
			break;
		case EngineState.Idle:
			goto default;
		default:
			MainParticles.enableEmission = false;
			AfterburnerParticles.enableEmission = false;
			break;
		}
	}
}
*/
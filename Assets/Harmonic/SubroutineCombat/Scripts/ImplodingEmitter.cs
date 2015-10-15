using UnityEngine;
using System.Collections;

public class ImplodingEmitter : MonoBehaviour {

	public ParticleSystem partSys;
	public bool isAttracting = true;
	public int numParticles = 30;
	public float pullTime = 2f;
	public float delayRange = 1f;

	private float cycleTime;

	void Start() {
		StartCoroutine(Implode());
	}
	
	private IEnumerator Implode() {
		partSys.Clear();
		if (!partSys.isPlaying) {
			partSys.Play();
		}

		while(isAttracting)
		{
			partSys.Emit(numParticles);
			yield return StartCoroutine(Attract());
			//once loop is finished, clear particles
			partSys.Clear();
		}

	}

	private IEnumerator Attract()
	{
		//allocate reference array
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[partSys.particleCount];
		Vector3[] startPosition = new Vector3[partSys.particleCount];
		float[] delays = new float[partSys.particleCount];

		int count = partSys.GetParticles(particles);
		float maxDelay = -1f;
		for (int i=0; i<count; i++) {
			startPosition[i] = particles[i].position;
			delays[i] = i * Random.value * delayRange;
			if (delays[i] > maxDelay)
				maxDelay = delays[i];
		}
		cycleTime = maxDelay + pullTime*2;

		//this loop executes over several frames
		// - update each particle's position
		// - set particle list
		// - wait one frame
		float currentTime = 0f;
		while (currentTime < cycleTime) {

			float lerpAmount = currentTime / pullTime;
			for (int j=0; j<count; j++) {
				particles[j].position = Vector3.Lerp(startPosition[j], transform.position, Mathf.Clamp(lerpAmount - delays[j], 0f, 1f));
			}

			partSys.SetParticles(particles, count);

			currentTime += Time.deltaTime;

			yield return null;
		}
	}
}

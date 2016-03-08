using UnityEngine;
using System.Collections;

public class ImplodingEmitter : MonoBehaviour {

	public ParticleSystem partSys;

	internal int numParticles = 30;
	internal float pullTime = 1f;
	internal float delayRange = .05f;
	internal bool isAttracting = false;
    internal GameObject particleSystemObj;

	private float cycleTime;

	void Start() {
        particleSystemObj = partSys.gameObject;
	}

    public void BeginAttraction()
    {
        isAttracting = true;
		StartCoroutine(Implode());
    }

    public void EndAttraction()
    {
        isAttracting = false;
    }
	
	private IEnumerator Implode() {
        if (particleSystemObj != null)
        {
		    partSys.Clear();
		    if (!partSys.isPlaying) {
                print("starting to play!");
			    partSys.Play();
		    }
		    while(isAttracting)
		    {
			    partSys.Emit(numParticles);
			    yield return StartCoroutine(Attract());
			    //once loop is finished, clear particles

                if (particleSystemObj != null)
                {
			        partSys.Clear();
                }

                isAttracting = false;
		    }
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
		for (int i = 0; i < count; i++) {
			startPosition[i] = particles[i].position;
			delays[i] = i * Random.value * delayRange;
			if (delays[i] > maxDelay)
				maxDelay = delays[i];
		}
        cycleTime = maxDelay + pullTime;

		//this loop executes over several frames
		// - update each particle's position
		// - set particle list
		// - wait one frame
		float currentTime = 0f;
		while (isAttracting && (currentTime < cycleTime) && (particleSystemObj != null)) {

			float lerpAmount = currentTime / pullTime;
			for (int j=0; j<count; j++) {
                float clamp = Mathf.Clamp(lerpAmount - delays[j], 0f, 1f);
                if (clamp < 1f)
                    particles[j].size = .5f + (clamp * 2) * (clamp * 2);
                else
                    particles[j].size = 0f;
                particles[j].position = Vector3.Lerp(startPosition[j], this.transform.position, clamp);
			}

			partSys.SetParticles(particles, count);

			currentTime += InterruptTime.deltaTime;

			yield return null;
		}
	}
}

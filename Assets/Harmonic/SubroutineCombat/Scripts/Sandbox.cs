using UnityEngine;
using System.Collections;

public class Sandbox : SubroutineFunction
{
    public float LookAtSpeed = 2f;
    public Transform SandboxVisualization;

    // Use this for initialization
    void Start()
    {
        this.Parent.Info.DamagePerHit = 0f;
        this.Parent.Info.FireRate = 5f;
        this.Parent.Info.CoreCost += 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Parent.IsActive)
        {
            bool canFire = false;
            if (CooldownRemaining <= 0f)
            {
                canFire = true;
            }
            else
            {
                CooldownRemaining -= Time.deltaTime;
            }

            if (TrackEnemy && !isFiring)
            {
                if (this.Parent.LockedTarget != null)
                {
                    Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
                    this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * LookAtSpeed);
                    float angle = Quaternion.Angle(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos));

                    if ((angle < 5f) && canFire)
                    {
                        FireAtEnemy(this.Parent.LockedTarget.position - this.transform.position);
                    }
                }
            }
        }
    }

    private void FireAtEnemy(Vector3 relativePos)
    {
        isFiring = true;
        CooldownRemaining = this.Parent.Info.FireRate;
        this.Parent.lockedMalware.TakeDamage(this.Parent.Info.DamagePerHit);
        StartCoroutine(this.WaitAndStopLaser());
    }


    private IEnumerator WaitAndStopLaser()
    {
        yield return new WaitForSeconds(1f);
        isFiring = false;
    }
}

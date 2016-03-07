using UnityEngine;
using System.Collections;

public class Sandbox : SubroutineFunction
{
    public float LookAtSpeed = 2f;
    public Transform SandboxVisualization;
    public override bool OnlyTrackActiveViruses { get { return true; } }

    private Transform CurrentSandboxViz;

    // Use this for initialization
    void Start()
    {
        this.Parent.Info.Cooldown = 8f;
        this.Parent.Info.CoreCost += 2;
        this.Parent.Info.HitChance += 100f;
        this.Parent.MyActorInfo.FunctionHitChance = 100f;
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
                CooldownRemaining -= InterruptTime.deltaTime;
            }

            if (TrackEnemy && !isFiring)
            {
                if ((this.Parent.LockedTarget != null) && (this.Parent.lockedVirus != null))
                {
                    Vector3 relativePos = this.Parent.LockedTarget.position - this.transform.position;
                    this.Parent.FunctionRoot.rotation = Quaternion.Slerp(this.Parent.FunctionRoot.rotation, Quaternion.LookRotation(relativePos), InterruptTime.deltaTime * LookAtSpeed);
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
        print("sandboxing with " + this.SandboxVisualization);
        isFiring = true;
        CooldownRemaining = this.Parent.Info.Cooldown;
        CurrentSandboxViz = (Transform)Instantiate(this.SandboxVisualization, this.Parent.lockedMalware.transform.position, Quaternion.identity);
        this.Parent.lockedVirus.ForceImmobilization();
        this.Parent.lockedVirus.IsSandboxed = true;
        StartCoroutine(this.WaitAndStopLaser());
    }


    private IEnumerator WaitAndStopLaser()
    {
        yield return new WaitForSecondsInterruptTime(5f);
        UnSandbox();
        isFiring = false;
    }

    void OnDestroy()
    {
        UnSandbox();
    }

    private void UnSandbox()
    {
        if (this.Parent.lockedVirus != null)
        {
            this.Parent.lockedVirus.UnforceImmobilization();
            this.Parent.lockedVirus.IsSandboxed = false;
        }
        if (CurrentSandboxViz != null)
        {
            print("destroying sandbox viz");
            print("name:" + CurrentSandboxViz.gameObject.name);
            GameObject.Destroy(CurrentSandboxViz.gameObject);
        }
    }
}

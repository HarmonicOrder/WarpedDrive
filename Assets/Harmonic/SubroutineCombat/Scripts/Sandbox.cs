using UnityEngine;
using System.Collections;
using System;

public class Sandbox : SubroutineFunction
{
    public Transform SandboxVisualization;
    public override bool OnlyTrackActiveViruses { get { return true; } }
    internal static float SandboxingTime = 4f;
    private Transform CurrentSandboxViz;
    private LineRenderer LineRenderer;
    private const float OneColorDuration = .5f;
    private float ColorDuration;
    private Color CurrentColor;

    // Use this for initialization
    void Start()
    {
        this.LineRenderer = this.transform.Find("FunctionRoot/Sandbox").GetComponent<LineRenderer>();
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
                this.HighlightTarget(this.Parent.LockedTarget.position);
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
                        FireAtEnemy(relativePos);
                    }
                    else
                    {
                        this.HighlightTarget(this.Parent.LockedTarget.position);
                    }
                }
                else
                {
                    this.Unhighlight();
                }
            }
            else
            {
                this.Unhighlight();
            }
        }
    }

    private void Unhighlight()
    {
        this.LineRenderer.SetPosition(1, Vector3.zero);
    }

    private void FireAtEnemy(Vector3 relativePos)
    {
        isFiring = true;
        CooldownRemaining = this.Parent.Info.Cooldown;
        CurrentSandboxViz = (Transform)Instantiate(this.SandboxVisualization, this.Parent.lockedMalware.transform.position, Quaternion.identity);
        this.Parent.lockedVirus.ForceImmobilization();
        this.Parent.lockedVirus.IsSandboxed = true;
        StartCoroutine(this.WaitAndStopLaser());
    }


    private IEnumerator WaitAndStopLaser()
    {
        this.Unhighlight();
        yield return new WaitForSecondsInterruptTime(SandboxingTime);
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
            GameObject.Destroy(CurrentSandboxViz.gameObject);
        }
    }

    private void ToggleColor()
    {
        if (CurrentColor == Color.red)
            CurrentColor = Color.blue;
        else
            CurrentColor = Color.red;

        this.LineRenderer.SetColors(CurrentColor, CurrentColor);
    }

    private void HighlightTarget(Vector3 targetPos)
    {
        this.LineRenderer.SetPosition(1, 
            //-relativePos
            this.LineRenderer.transform.InverseTransformPoint(targetPos)
            //Vector3.forward * relativePos.z * -relativePos.magnitude
            );

        if (this.ColorDuration <= 0f)
        {
            this.ToggleColor();
            this.ColorDuration = OneColorDuration;
        }
        else
        {
            this.ColorDuration -= InterruptTime.deltaTime;
        }

    }
}

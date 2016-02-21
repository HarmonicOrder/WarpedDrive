using UnityEngine;
using System.Collections;

public class BunnyBomb : VirusAI
{

    public override VirusAI.VirusType Type { get { return VirusAI.VirusType.Wabbit; } }

    public BunnyBomb Partner { get; set; }
    public Transform Hourglass;

    protected override void OnAwake()
    {
        //this will fail to add the virus to the virus list
        base.OnAwake();
        Hourglass.gameObject.SetActive(false);
        this.Info = new ActorInfo()
        {
            Name = "Wabbit",
            FireRate = 0f,
            HitChance = 0,
            SaveChance = 5,
        };

        this.machineCenter = this.transform.root.position;
        this.TimeTilNextProvision = Random.Range(3, 7);
    }

    public void SetPartner(BunnyBomb part)
    {
        if (this.transform.root == null)
            Debug.LogWarning("no root! you need to be under a machine!");

        this.Partner = part;
        ActiveSubroutines.AddVirus(this);
    }

    private float LookAtSpeed = 5f;
    private float CreatePartnerTime = 3f;
    private float CurrentCreatePartnerTime = 0f;
    private Vector3 RandomMoveDirection;
    private float RandomMoveTime = 0f;
    private float CurrentRandomMoveTime = 1f;
    private float moveSpeed = 10f;
    private Vector3 machineCenter;
    private bool IsOccupyingCore = false;
    private const float OccupationDuration = 3f;
    private float TimeTilNextProvision = 2f;
    private float CurrentlyProvisioningTime = 0f;

    protected override void OnUpdate()
    {
        if (Partner == null)
        {
            if (CurrentCreatePartnerTime > CreatePartnerTime)
            {
                Hourglass.gameObject.SetActive(false);
                Partner = GameObject.Instantiate<GameObject>(this.gameObject).GetComponent<BunnyBomb>();
                Partner.transform.SetParent(this.transform.parent);
                //this MUST be after setParent
                Partner.SetPartner(this);
                Partner.transform.position = this.transform.position + Vector3.forward * 10;
                Partner.transform.localRotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0, 180, 0), 180);
                CurrentCreatePartnerTime = 0f;
            }
            else
            {
                if (CurrentCreatePartnerTime == 0)
                    Hourglass.gameObject.SetActive(true);
                CurrentCreatePartnerTime += InterruptTime.deltaTime;
            }
        }
        else
        {
            if (TimeTilNextProvision < 0f)
            {
                if (IsOccupyingCore)
                {
                    if (CurrentlyProvisioningTime > OccupationDuration)
                    {
                        CyberspaceBattlefield.Current.ReclaimCores(1, true);
                        Hourglass.gameObject.SetActive(false);
                        TimeTilNextProvision = (float)Random.Range(3, 7);
                        CurrentlyProvisioningTime = 0f;
                        IsOccupyingCore = false;
                    }
                    else
                    {
                        CurrentlyProvisioningTime += InterruptTime.deltaTime;
                    }
                }
                else
                {
                    IsOccupyingCore = CyberspaceBattlefield.Current.ProvisionCores(1, true);
                    if (IsOccupyingCore)
                    {
                        Hourglass.gameObject.SetActive(true);
                        CurrentlyProvisioningTime = 0f;
                    }
                }
            }
            else if (CurrentRandomMoveTime > RandomMoveTime)
            {
                if (Vector3.Distance(this.transform.position, this.machineCenter) > 150f)
                {
                    RandomMoveDirection = this.machineCenter - this.transform.position;
                }
                else
                {
                    RandomMoveDirection = HarmonicUtils.RandomVector(-1f, 1f).normalized;// + Vector3.forward * 3f;
                }
                RandomMoveTime = Random.Range(3, 8);
                CurrentRandomMoveTime = 0f;
                TimeTilNextProvision -= InterruptTime.deltaTime;
            }
            else
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(RandomMoveDirection), InterruptTime.deltaTime * LookAtSpeed);

                this.transform.Translate(0, 0, InterruptTime.deltaTime * this.moveSpeed, Space.Self);

                CurrentRandomMoveTime += InterruptTime.deltaTime;
                TimeTilNextProvision -= InterruptTime.deltaTime;
            }
        }
    }
    protected override void OnVirusDead()
    {
        if (IsOccupyingCore)
        {
            CyberspaceBattlefield.Current.ReclaimCores(1);
            CyberspaceBattlefield.Current.StolenCores -= 1;
        }
        GameObject.Instantiate(this.ExplosionPrefab, this.transform.position, Quaternion.identity);
        base.OnVirusDead();
        GameObject.Destroy(this.gameObject);
    }
}

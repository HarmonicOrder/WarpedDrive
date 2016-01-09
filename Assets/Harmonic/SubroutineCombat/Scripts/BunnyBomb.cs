﻿using UnityEngine;
using System.Collections;

public class BunnyBomb : VirusAI
{

    public override string DisplayNameSingular { get { return "Wabbit"; } }
    public override string DisplayNamePlural { get { return "Wabbits"; } }

    public BunnyBomb Partner { get; set; }
    public Transform Hourglass;

    protected override void OnAwake()
    {
        base.OnAwake();
        Hourglass.gameObject.SetActive(false);
        this.Info = new ActorInfo()
        {
            Name = "Rabbit",
            DamagePerHit = 0f,
            FireRate = 0f,
            HitPoints = 1f,
            ArmorPoints = 0f
        };

        this.machineCenter = this.transform.root.position;
    }

    private float LookAtSpeed = 5f;
    private float CreatePartnerTime = 3f;
    private float CurrentCreatePartnerTime = 0f;
    private Vector3 RandomMoveDirection;
    private float RandomMoveTime = 0f;
    private float CurrentRandomMoveTime = 1f;
    private float moveSpeed = 10f;
    private Vector3 machineCenter;

    protected override void OnUpdate()
    {
        if (Partner == null)
        {
            if (CurrentCreatePartnerTime > CreatePartnerTime)
            {
                Hourglass.gameObject.SetActive(false);
                Partner = GameObject.Instantiate<GameObject>(this.gameObject).GetComponent<BunnyBomb>();
                Partner.transform.SetParent(this.transform.parent);
                Partner.transform.position = this.transform.position + Vector3.forward * 10;
                Partner.transform.localRotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0, 180, 0), 180);
                Partner.Partner = this;
                CurrentCreatePartnerTime = 0f;
            }
            else
            {
                if (CurrentCreatePartnerTime == 0)
                    Hourglass.gameObject.SetActive(true);
                CurrentCreatePartnerTime += Time.deltaTime;
            }
        }
        else
        {
            if (CurrentRandomMoveTime > RandomMoveTime)
            {
                if (Vector3.Distance(this.transform.position, this.machineCenter) > 150f)
                {
                    RandomMoveDirection = this.machineCenter - this.transform.position;
                }
                else
                {
                    RandomMoveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;// + Vector3.forward * 3f;
                }
                RandomMoveTime = Random.Range(3, 8);
                CurrentRandomMoveTime = 0f;
            }
            else
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(RandomMoveDirection), Time.deltaTime * LookAtSpeed);

                this.transform.Translate(0, 0, Time.deltaTime * this.moveSpeed, Space.Self);

                CurrentRandomMoveTime += Time.deltaTime;
            }
        }
    }
    protected override void OnVirusDead()
    {
        GameObject.Instantiate(this.ExplosionPrefab, this.transform.position, Quaternion.identity);
        base.OnVirusDead();
        GameObject.Destroy(this.gameObject);
    }
}
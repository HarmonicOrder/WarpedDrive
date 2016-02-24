using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Popup : MonoBehaviour {
    public static List<Popup> Pool = new List<Popup>();
    public static void Create(Vector3 position, Transform parent, Popups state, bool isSubroutine, bool persist = false)
    {
        Popup tobeCreated;
        if (Pool.Count > 0)
        {
            tobeCreated = Pool[0];
            Pool.RemoveAt(0);
            tobeCreated.gameObject.SetActive(true);
            tobeCreated.transform.position = position;
        }
        else
        {
            Transform g = (Transform)GameObject.Instantiate(CombatStaticPrefabReference.Instance.Popup, position, Quaternion.identity);
            tobeCreated = g.GetComponent<Popup>();
        }

        if (parent != null)
            tobeCreated.transform.SetParent(parent);
        tobeCreated.SetSprite(state, isSubroutine, persist);
    }

    public Color GoodColor, BadColor, FreezeColor;
    public Sprite Block, Reboot, Freeze, Lag;
    public enum Popups { Block, Reboot, Freeze, Lag}
    private SpriteRenderer visual;

    void Awake()
    {
        visual = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Popups toPopup, bool isSubroutine, bool persist)
    {
        Color color = BadColor;
        if (isSubroutine)
            color = GoodColor;

        switch (toPopup)
        {
            case Popups.Block:
                this.visual.sprite = Block;
                break;
            case Popups.Reboot:
                this.visual.sprite = Reboot;
                break;
            case Popups.Freeze:
                this.visual.sprite = Freeze;
                color = FreezeColor;
                break;
            case Popups.Lag:
                this.visual.sprite = Lag;
                if (isSubroutine)
                    color = BadColor;
                else
                    color = GoodColor;
                break;
        }
        visual.color = color;

        if (persist)
        {

        }
        else
        {
            StartCoroutine(SelfDestruct());
        }
    }

    public void Stop()
    {
        Die();
    }

    public IEnumerator SelfDestruct()
    {
        yield return new WaitForSecondsInterruptTime(3f);
        Die();
    }

    private void Die()
    {
        this.gameObject.SetActive(false);
        Pool.Add(this);
    }
}

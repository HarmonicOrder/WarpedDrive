using UnityEngine;
using System.Collections;

public class HueShift : MonoBehaviour {

    public int materialIndex = 0;
    public string colorName = "_Color";
    public float degreeRate = .1f;

    private MeshRenderer myRender;
    void Start()
    {
        myRender = GetComponent<MeshRenderer>();
    }

    private HarmonicUtils.HSBColor hslColor;
    // Update is called once per frame
    void LateUpdate()
    {
        if (myRender != null)
        {
            hslColor = HarmonicUtils.HSBColor.FromColor(myRender.materials[materialIndex].GetColor(colorName));
            hslColor.h += (degreeRate / 360);
            if (hslColor.h > 1)
                hslColor.h = 0;
            myRender.materials[materialIndex].SetColor(colorName, hslColor.ToColor());
        }
    }
}

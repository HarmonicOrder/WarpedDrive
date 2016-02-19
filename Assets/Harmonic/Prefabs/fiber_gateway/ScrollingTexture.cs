using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class ScrollingTexture : MonoBehaviour {

	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 1, 0 );
	public string textureName = "_MainTex";
    public bool AlsoAnimateProperty = false;
    public string PropertyName = "";
    public float PropertyStartValue = 0f;
    public float PropertyDelta = 1f;
    public float PropertyMax = 128f;
    public bool PingPongProperty = true;

    private float currentPropVal;
    private float propertyDirection = 1;
	private Vector2 uvOffset = Vector2.zero;
	private MeshRenderer myRender;
	void Start(){
		myRender = GetComponent<MeshRenderer>();
        currentPropVal = PropertyStartValue;
    }

	// Update is called once per frame
	void LateUpdate () {
		uvOffset += ( uvAnimationRate * InterruptTime.deltaTime );
		if (myRender != null)
		{
			myRender.materials[ materialIndex ].SetTextureOffset( textureName, uvOffset);

            if (AlsoAnimateProperty)
            {
                if (currentPropVal > PropertyMax)
                {
                    if (PingPongProperty)
                    {
                        propertyDirection = -1 * propertyDirection;
                        currentPropVal += PropertyDelta * propertyDirection;
                    }
                    else
                        currentPropVal = PropertyStartValue;
                }
                else if (PingPongProperty && propertyDirection < 0 && currentPropVal <= PropertyStartValue)
                {
                    currentPropVal = PropertyStartValue;
                    propertyDirection = -propertyDirection;
                }
                else
                    currentPropVal += PropertyDelta * propertyDirection;

                myRender.materials[materialIndex].SetFloat(PropertyName, currentPropVal);
            }
        }
	}
}

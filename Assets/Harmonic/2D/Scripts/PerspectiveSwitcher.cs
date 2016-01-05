using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(MatrixBlender))]
public class PerspectiveSwitcher : MonoBehaviour
{
    private Matrix4x4 ortho,
                        perspective;
    public float fov = 60f,
                        near = .3f,
                        far = 1000f,
                        orthographicSize = 50f;
    private float aspect;
    private MatrixBlender blender;

    void Start()
    {
        aspect = (float)Screen.width / (float)Screen.height;
        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);

        if (HarmonicSerialization.Instance.IsNewGame)
        {
            GetComponent<Camera>().projectionMatrix = perspective;
            StartCoroutine(Wait());
        }
        else
            GetComponent<Camera>().projectionMatrix = ortho;

        blender = (MatrixBlender)GetComponent(typeof(MatrixBlender));
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        blender.BlendToMatrix(ortho, 1f);
    }

    //void Update()
    //{
    //    if (ChangePerspective)
    //    {
    //        if (ChangeToOrtho)
    //            blender.BlendToMatrix(ortho, 1f);
    //        else if (ChangeToPerspective)
    //            blender.BlendToMatrix(perspective, 1f);
    //    }
    //}
}
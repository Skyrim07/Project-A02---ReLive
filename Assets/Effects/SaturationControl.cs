using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class SaturationControl : MonoBehaviour
{
    [SerializeField] Shader satShader;
    [SerializeField] Texture noise;

    Material satMaterial;
    private void Start()
    {
        satMaterial = new Material(satShader);
        satMaterial.SetTexture("_NoiseTex", noise);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination,satMaterial);
    }
}

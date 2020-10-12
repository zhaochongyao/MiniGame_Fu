using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public sealed class CameraTask : MonoBehaviour
{
    public Shader Blend2Shader;
    public Texture texture0;
    public Texture texture1;
    public Material material;
    
    private void Start()
    {
       
    }
    public void move()
    {

    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
     /*   var blend2Material = new Material(Blend2Shader);
        blend2Material.SetTexture("_Texture0", texture0);
        blend2Material.SetTexture("_Texture1", texture1);
        Graphics.Blit(source, destination, blend2Material);*/
    }
}

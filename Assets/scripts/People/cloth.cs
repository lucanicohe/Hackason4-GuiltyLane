using UnityEngine;

public class cloth : MonoBehaviour
{
    public Texture[] textures;

    private static int instanceCounter = 0;

    void Start()
    {
        if (textures == null || textures.Length == 0)
            return;

        int index = instanceCounter % textures.Length;
        instanceCounter++;

        Renderer renderer = GetComponent<Renderer>();

        // Use instance material so each plane gets its own texture
        Material mat = renderer.material;
        mat.SetTexture("_MainTex", textures[index]);
    }
}
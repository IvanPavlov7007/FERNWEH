using UnityEngine;
using UnityEditor;

namespace Sailboat
{
    public class SpriteTo3DTexture : MonoBehaviour
    {
        [Header("Source Sprite (2D Texture)")]
        public Sprite sprite;

        [Header("Generated 3D Texture")]
        public Texture3D volumeTexture;

        [Header("Depth Settings")]
        public int depth = 16; // Z-depth of the 3D texture

        [ContextMenu("Generate 3D Texture")]
        void Generate3DTexture()
        {
            Texture2D sourceTexture = sprite.texture;

            if (sourceTexture == null)
            {
                Debug.LogError("Please assign a source texture!");
                return;
            }

            int width = sourceTexture.width;
            int height = sourceTexture.height;

            volumeTexture = new Texture3D(width, height, depth, TextureFormat.RGBAHalf, false);
            Color[] colors = new Color[width * height * depth];

            Color[] slicePixels = sourceTexture.GetPixels();

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index2D = x + y * width;
                        int index3D = x + y * width + z * width * height;

                        Color pixel = slicePixels[index2D];

                        // If you're using 0-255 painted colors, remap from [0,1] to [-1,1]
                        Vector3 force = new Vector3(
                            pixel.r * 2f - 1f,
                            pixel.g * 2f - 1f,
                            pixel.b * 2f - 1f
                        );

                        colors[index3D] = new Color(force.x, force.y, force.z, 1f);
                    }
                }
            }

            volumeTexture.SetPixels(colors);
            volumeTexture.Apply();

#if UNITY_EDITOR
            string assetPath = "Assets/GeneratedVolumeTexture.asset";
            AssetDatabase.CreateAsset(volumeTexture, assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log("Saved 3D texture at " + assetPath);
#endif
        }
    }
}
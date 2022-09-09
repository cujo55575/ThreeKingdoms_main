using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PostProcessor : AssetPostprocessor
{
    // Change All Import Images To Sprite Folder as Sprite 2D
    public void OnPreprocessTexture()
    {
        if (ImageEditor.ImageType == null || ImageEditor.ImageType== ImageType.None.ToString())
        {
            if (assetPath.Contains("Sprite"))
            {
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                textureImporter.textureType = TextureImporterType.Sprite;
            }
        }else if(ImageEditor.ImageType == ImageType.Sprite_2D.ToString())
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
        }

    }
}

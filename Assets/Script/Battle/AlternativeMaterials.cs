using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeMaterials : MonoBehaviour
{
	
    public SkinnedMeshRenderer[] rends;

    public List<Skin> skins;
    [System.Serializable]
    public class Skin
    {
        public string SkinName;
        public List<Material> mats;
    }
    private void Start()
    {
        if (rends.Length == 0)
        {
            rends = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
    }
    public void ChangeSkin(string ModelName)
    {
        for (int i = 0; i < skins.Count; i++)
        {
            if (skins[i].SkinName == ModelName)
            {
                for (int x = 0; x < rends.Length; x++)
                {
                    rends[x].material = skins[i].mats[x];
                }
            }
        }
    }
	public void ChangeShaderWithType(E_ShaderType shaderType)
	{
         if(rends.Length==0)
        {
                this.Start();
        }
		switch (shaderType)
		{
			case E_ShaderType.Normal:
				for (int x = 0; x < rends.Length; x++)
				{
					rends[x].material.shader = Shader.Find("Mobile/Bumped Diffuse");
					rends[x].material.color = Color.white;
				}
			break;
			case E_ShaderType.SemiTransparent:
				for (int x = 0; x < rends.Length; x++)
				{
					rends[x].material.shader = Shader.Find("Custom/Transparent");
					rends[x].material.color = new Color(1,1,1,0.3f);
				}
			break;
			case E_ShaderType.Invisible:
				for (int x = 0; x < rends.Length; x++)
				{
					rends[x].material.shader = Shader.Find("Custom/Transparent");
					rends[x].material.color = new Color(1,1,1,0.0f);
				}
			break;
            case E_ShaderType.ModelGreyScale:
                for (int x = 0; x < rends.Length; x++)
                {
                    rends[x].material.shader = Shader.Find("Mobile/Bumped Diffuse GrayScale");
                }
            break;
        }
	}
    public void ChangeShader()
    {
        for (int x = 0; x < rends.Length; x++)
        {
            rends[x].material.shader = Shader.Find("Unlit/Color");
            rends[x].material.color = Color.red;
        }
    }
    public void TriggerRed()
    {
        rends = GetComponentsInChildren<SkinnedMeshRenderer>();
        for(int i=0;i<rends.Length;i++)
        {
            rends[i].material.shader = Shader.Find("Custom/FresnelShader");
            rends[i].material.SetFloat("Shinines",0.6f);
        }
    }
    public void TriggerInvisible()
    {
        rends = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.shader = Shader.Find("Custom/Transparent");
            rends[i].material.color = new Color(1,1,1,0.3f);
        }
    }
}

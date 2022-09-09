using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using XEResources;

public class GameTool
{
	[InitializeOnLoad]
	class AutoSetupConditional
	{
		static AutoSetupConditional()
		{
			PlayerSettings.bundleVersion = GLOBALCONST.SYSTEM_VERSION;
			PlayerSettings.Android.bundleVersionCode = GLOBALCONST.SYSTEM_VERSIONCODE;
			PlayerSettings.applicationIdentifier = GLOBALCONST.SYSTEM_BUNDLEID;
			PlayerSettings.productName = GLOBALCONST.SYSTEM_PRODUCTNAME;

			if (!File.Exists("Assets/Script/Common/Define.cs"))
				return;

			var sr = File.OpenText("Assets/Script/Common/Define.cs");
			List<string> defines = new List<string>();
			try
			{
				var fileLines = sr.ReadToEnd().Split("\n"[0]).ToList();
				foreach (string k in fileLines)
				{
					var define = k.Trim();
					int ind = define.IndexOf("//");
					if (ind != -1)
						define = define.Remove(ind).Trim();
					if (define == string.Empty)
						continue;
					ind = define.IndexOf("#define");
					if (ind == 0)
						define = define.Remove(ind,7).Trim();
					defines.Add(define);
					//Debug.Log(define);
				}
			}
			finally
			{
				sr.Close();
			}

			var types = new[] { BuildTargetGroup.Standalone,BuildTargetGroup.WebGL,BuildTargetGroup.iOS,BuildTargetGroup.Android };
			var toInclude = defines.ToArray();
			var hasEntry = new bool[toInclude.Length];
			var conditionals = string.Empty;
			for (int i = 0; i < toInclude.Length; i++)
			{
				conditionals += (string.IsNullOrEmpty(conditionals) ? string.Empty : ";") + toInclude[i];
			}

			foreach (var type in types)
			{
				PlayerSettings.SetScriptingDefineSymbolsForGroup(type,conditionals);
			}
		}
	}

	[MenuItem("Tools/Build/10 Build All Table")]
	static void ExportTables()
	{
		TableTool.SaveAllTable();
	}
}

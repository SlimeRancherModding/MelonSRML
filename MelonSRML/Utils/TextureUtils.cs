using System.Collections.Generic;
using System.IO;

namespace MelonSRML.Utils
{

	public static class TextureUtils
	{
		
		public static Texture2D CreateRamp(Color a, Color b)
		{
			Texture2D ramp = new Texture2D(128, 32);

			for (int x = 0; x < 128; x++)
			{
				Color curr = Color.Lerp(a, b, x / 127f);
				for (int y = 0; y < 32; y++)
					ramp.SetPixel(x, y, curr);
			}

			ramp.name = $"generatedTexture-{ramp.GetInstanceID()}";
			ramp.AddToAntiGC();
			ramp.Apply();
			ramp.hideFlags |= HideFlags.HideAndDontSave;
			return ramp;
		}

	
		public static Texture2D CreateRamp(Color a, Color b, params Color[] others)
		{
			Texture2D ramp = new Texture2D(128, 32);

			List<Color> colors = new List<Color>() { a, b };
			colors.AddRange(others);

			int stage = Mathf.RoundToInt(128f / (colors.Count - 1));

			for (int x = 0; x < 128; x++)
			{
				Color curr = Color.Lerp(colors[0], colors[1], (x % stage) / (stage - 1));

				if ((x % stage) == stage - 1)
					colors.RemoveAt(0);

				for (int y = 0; y < 32; y++)
					ramp.SetPixel(x, y, curr);
			}

			ramp.name = $"generatedTexture-{ramp.GetInstanceID()}";
			ramp.Apply();
			ramp.AddToAntiGC();
			ramp.hideFlags |= HideFlags.HideAndDontSave;
			return ramp;
		}

	
		public static Texture2D CreateRamp(string hexA, string hexB)
		{

			ColorUtility.TryParseHtmlString("#" + hexA.ToUpper(), out Color a);
			a.BoxIl2CppObject().AddToAntiGC();
			ColorUtility.TryParseHtmlString("#" + hexB.ToUpper(), out Color b);
			b.BoxIl2CppObject().AddToAntiGC();
			
			return CreateRamp(a, b);
		}

		
		public static Texture2D CreateRamp(string hexA, string hexB, params string[] hexs)
		{
			ColorUtility.TryParseHtmlString("#" + hexA.ToUpper(), out Color a);
			a.BoxIl2CppObject().AddToAntiGC();
			ColorUtility.TryParseHtmlString("#" + hexB.ToUpper(), out Color b);
			b.BoxIl2CppObject().AddToAntiGC();
			List<Color> colors = new List<Color>();
			foreach (string hex in hexs)
			{
				ColorUtility.TryParseHtmlString("#" + hex.ToUpper(), out Color c);
				c.BoxIl2CppObject().AddToAntiGC();
				colors.Add(c);
			}

			return CreateRamp(a, b, colors.ToArray());
		}

		public static Sprite ConvertSprite(Texture2D texture2D)
		{
			return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
		}

		public static Texture2D LoadTexture2D(string path)
		{
			var texture2D = new Texture2D(1, 1);
			texture2D.hideFlags |= HideFlags.HideAndDontSave;
			texture2D.AddToAntiGC();
			Il2CppImageConversionManager.LoadImage(texture2D, File.ReadAllBytes(path));
			return texture2D;
		}

		public static Texture2D LoadTexture2D(Stream stream)
		{
			var texture2D = new Texture2D(1, 1);
			texture2D.hideFlags |= HideFlags.HideAndDontSave;
			texture2D.AddToAntiGC();
			byte[] data = new byte[stream.Length];
			var read = stream.Read(data, 0, data.Length);
			Il2CppImageConversionManager.LoadImage(texture2D, data);
			return texture2D;
			//

		}
	}
}
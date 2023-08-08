using System.Collections.Generic;

namespace MelonSRML.Utils
{
    
    public static class ColorUtils
    {

        public static Color FromHex(string hex)
        {
            ColorUtility.TryParseHtmlString("#" + hex.ToUpper(), out Color color);
            color.BoxIl2CppObject().AddToAntiGC();
            return color;
        }
		
        public static Color[] FromHexArray(params string[] hexas)
        {
            List<Color> colors = new List<Color>();

            foreach (string hex in hexas)
            {
                
                colors.Add(FromHex(hex));
            }

            return colors.ToArray();
        }

        public static string ToHexRGB(Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }
        
        public static string ToHexRGBA(Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }
        public static UnityEngine.Color ToUnityColor(this System.Drawing.Color drawingColor)
        {
            return new UnityEngine.Color(drawingColor.R / 255f, drawingColor.G / 255f, drawingColor.B / 255f, drawingColor.A / 255f);
        }

        public static System.Drawing.Color ToDrawingColor(this UnityEngine.Color unityColor)
        {
            return System.Drawing.Color.FromArgb(
                (int) (unityColor.r * 255f),
                (int) (unityColor.g * 255f),
                (int) (unityColor.b * 255f),
                (int) (unityColor.a * 255f)
            );
        }

        public static string ToHexString(this System.Drawing.Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";
    }
}
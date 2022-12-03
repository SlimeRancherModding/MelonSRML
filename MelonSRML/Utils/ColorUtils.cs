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
    }
}
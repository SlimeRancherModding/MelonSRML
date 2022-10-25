using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

namespace MelonSRML.Utils
{
    public static class TextureUtility
    {
        /// <summary>
        /// Loads A Texture2D
        /// </summary>
        public static Texture2D LoadTexture(string SoutionName, string name, string fileExtension)
        {
            //note to self: need to change SoutionName
            string _name = String.Concat(SoutionName + "." + name + fileExtension);
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_name);
            var streamBuffer = new byte[manifestResourceStream.Length];
            manifestResourceStream.Read(streamBuffer, 0, streamBuffer.Length);
            Texture2D image = new Texture2D(1, 1);
            Il2CppImageConversionManager.LoadImage(image, (Il2CppStructArray<byte>)streamBuffer);
            image.name = name;
            return image;
        }

        /// <summary>
        /// Creates A Texture2D Of A Solid Color
        /// </summary>
        public static Texture2D CreateTextureFromColor(Color color, Vector2 size)
        {
            Texture2D texture = new Texture2D(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));
            var texturePixelArray = texture.GetPixels();
            for (var i = 0; i < texturePixelArray.Length; ++i)
            {
                texturePixelArray[i] = color;
            }
            texture.SetPixels(texturePixelArray);
            texture.name = string.Concat("ColorTexture", texture.GetInstanceID());
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Turns A Texture2D Into A Sprite
        /// </summary>
        public static Sprite CreateSprite(Texture2D texture)
        {
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            sprite.name = string.Concat("Sprite", texture.name);
            return sprite;
        }

        /// <summary>
        /// Saves A Texture2D To A Folder
        /// </summary>
        public static void TextureToPng(Texture2D texture, string folderToSaveTo)
        {
            byte[] bytes = Il2CppImageConversionManager.EncodeToPNG(texture);
            if (!Directory.Exists(folderToSaveTo)) Directory.CreateDirectory(folderToSaveTo);
            File.WriteAllBytes(folderToSaveTo + texture.name + ".png", bytes);
        }
    }
}

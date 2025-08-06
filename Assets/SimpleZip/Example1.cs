using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleZip
{
    public class Example1 : MonoBehaviour
    {
        public Text Text;

        /// <summary>
        /// Usage example
        /// </summary>
        public void Start()
        {
            var sample = "El perro de San Roque no tiene rabo porque Ramón Rodríguez se lo ha robado.";
            var compressed = Zip1.CompressToString(sample);
            var decompressed = Zip1.Decompress(compressed);

            Text.text = $"Plain text: {sample}\n\nCompressed: {compressed}\n\nDecompressed: {decompressed}";

            Directory.CreateDirectory("Assets/Temp");
            Zip1.CompressDirectory("Assets/SimpleZip", "Assets/Temp/SimpleZip.zip");
            Zip1.CompressFile("Assets/SimpleZip/Readme.txt", "Assets/Temp/Readme.zip");
            Zip1.DecompressArchive("Assets/Temp/Readme.zip", "Assets/Temp/");
        }
    }
}
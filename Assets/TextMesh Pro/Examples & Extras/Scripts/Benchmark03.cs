using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace TextMesh_Pro.Scripts
{

    public class Benchmark03 : MonoBehaviour
    {
        public enum BenchmarkType { TmpSdfMobile = 0, TmpSdfMobileSsd = 1, TmpSdf = 2, TmpBitmapMobile = 3, TextmeshBitmap = 4 }

        public int numberOfSamples = 100;
        public BenchmarkType benchmark;

        public Font sourceFontFile;


        void Awake()
        {

        }


        void Start()
        {
            TMP_FontAsset fontAsset = null;

            // Create Dynamic Font Asset for the given font file.
            switch (benchmark)
            {
                case BenchmarkType.TmpSdfMobile:
                    fontAsset = TMP_FontAsset.CreateFontAsset(sourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
                    break;
                case BenchmarkType.TmpSdfMobileSsd:
                    fontAsset = TMP_FontAsset.CreateFontAsset(sourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
                    fontAsset.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field SSD");
                    break;
                case BenchmarkType.TmpSdf:
                    fontAsset = TMP_FontAsset.CreateFontAsset(sourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
                    fontAsset.material.shader = Shader.Find("TextMeshPro/Distance Field");
                    break;
                case BenchmarkType.TmpBitmapMobile:
                    fontAsset = TMP_FontAsset.CreateFontAsset(sourceFontFile, 90, 9, GlyphRenderMode.SMOOTH, 256, 256, AtlasPopulationMode.Dynamic);
                    break;
            }

            for (int i = 0; i < numberOfSamples; i++)
            {
                switch (benchmark)
                {
                    case BenchmarkType.TmpSdfMobile:
                    case BenchmarkType.TmpSdfMobileSsd:
                    case BenchmarkType.TmpSdf:
                    case BenchmarkType.TmpBitmapMobile:
                        {
                            GameObject go = new GameObject();
                            go.transform.position = new Vector3(0, 1.2f, 0);

                            TextMeshPro textComponent = go.AddComponent<TextMeshPro>();
                            textComponent.font = fontAsset;
                            textComponent.fontSize = 128;
                            textComponent.text = "@";
                            textComponent.alignment = TextAlignmentOptions.Center;
                            textComponent.color = new Color32(255, 255, 0, 255);

                            if (benchmark == BenchmarkType.TmpBitmapMobile)
                                textComponent.fontSize = 132;

                        }
                        break;
                    case BenchmarkType.TextmeshBitmap:
                        {
                            GameObject go = new GameObject();
                            go.transform.position = new Vector3(0, 1.2f, 0);

                            TextMesh textMesh = go.AddComponent<TextMesh>();
                            textMesh.GetComponent<Renderer>().sharedMaterial = sourceFontFile.material;
                            textMesh.font = sourceFontFile;
                            textMesh.anchor = TextAnchor.MiddleCenter;
                            textMesh.fontSize = 130;

                            textMesh.color = new Color32(255, 255, 0, 255);
                            textMesh.text = "@";
                        }
                        break;
                }
            }
        }

    }
}

using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Scripts
{
    
    public class TmpUiFrameRateCounter : MonoBehaviour
    {
        public float updateInterval = 5.0f;
        private float mLastInterval = 0;
        private int mFrames = 0;

        public enum FpsCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };

        public FpsCounterAnchorPositions anchorPosition = FpsCounterAnchorPositions.TopRight;

        private string htmlColorTag;
        private const string FpsLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

        private TextMeshProUGUI mTextMeshPro;
        private RectTransform mFrameCounterTransform;

        private FpsCounterAnchorPositions lastAnchorPosition;

        void Awake()
        {
            if (!enabled)
                return;

            Application.targetFrameRate = 1000;

            GameObject frameCounter = new GameObject("Frame Counter");
            mFrameCounterTransform = frameCounter.AddComponent<RectTransform>();

            mFrameCounterTransform.SetParent(this.transform, false);

            mTextMeshPro = frameCounter.AddComponent<TextMeshProUGUI>();
            mTextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            mTextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");

            mTextMeshPro.enableWordWrapping = false;
            mTextMeshPro.fontSize = 36;

            mTextMeshPro.isOverlay = true;

            Set_FrameCounter_Position(anchorPosition);
            lastAnchorPosition = anchorPosition;
        }


        void Start()
        {
            mLastInterval = Time.realtimeSinceStartup;
            mFrames = 0;
        }


        void Update()
        {
            if (anchorPosition != lastAnchorPosition)
                Set_FrameCounter_Position(anchorPosition);

            lastAnchorPosition = anchorPosition;

            mFrames += 1;
            float timeNow = Time.realtimeSinceStartup;

            if (timeNow > mLastInterval + updateInterval)
            {
                // display two fractional digits (f2 format)
                float fps = mFrames / (timeNow - mLastInterval);
                float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

                if (fps < 30)
                    htmlColorTag = "<color=yellow>";
                else if (fps < 10)
                    htmlColorTag = "<color=red>";
                else
                    htmlColorTag = "<color=green>";

                mTextMeshPro.SetText(htmlColorTag + FpsLabel, fps, ms);

                mFrames = 0;
                mLastInterval = timeNow;
            }
        }


        void Set_FrameCounter_Position(FpsCounterAnchorPositions anchorPosition)
        {
            switch (anchorPosition)
            {
                case FpsCounterAnchorPositions.TopLeft:
                    mTextMeshPro.alignment = TextAlignmentOptions.TopLeft;
                    mFrameCounterTransform.pivot = new Vector2(0, 1);
                    mFrameCounterTransform.anchorMin = new Vector2(0.01f, 0.99f);
                    mFrameCounterTransform.anchorMax = new Vector2(0.01f, 0.99f);
                    mFrameCounterTransform.anchoredPosition = new Vector2(0, 1);
                    break;
                case FpsCounterAnchorPositions.BottomLeft:
                    mTextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
                    mFrameCounterTransform.pivot = new Vector2(0, 0);
                    mFrameCounterTransform.anchorMin = new Vector2(0.01f, 0.01f);
                    mFrameCounterTransform.anchorMax = new Vector2(0.01f, 0.01f);
                    mFrameCounterTransform.anchoredPosition = new Vector2(0, 0);
                    break;
                case FpsCounterAnchorPositions.TopRight:
                    mTextMeshPro.alignment = TextAlignmentOptions.TopRight;
                    mFrameCounterTransform.pivot = new Vector2(1, 1);
                    mFrameCounterTransform.anchorMin = new Vector2(0.99f, 0.99f);
                    mFrameCounterTransform.anchorMax = new Vector2(0.99f, 0.99f);
                    mFrameCounterTransform.anchoredPosition = new Vector2(1, 1);
                    break;
                case FpsCounterAnchorPositions.BottomRight:
                    mTextMeshPro.alignment = TextAlignmentOptions.BottomRight;
                    mFrameCounterTransform.pivot = new Vector2(1, 0);
                    mFrameCounterTransform.anchorMin = new Vector2(0.99f, 0.01f);
                    mFrameCounterTransform.anchorMax = new Vector2(0.99f, 0.01f);
                    mFrameCounterTransform.anchoredPosition = new Vector2(1, 0);
                    break;
            }
        }
    }
}
﻿using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace TextMesh_Pro.Scripts
{

    public class TmpTextInfoDebugTool : MonoBehaviour
    {
        // Since this script is used for debugging, we exclude it from builds.
        // TODO: Rework this script to make it into an editor utility.
        #if UNITY_EDITOR
        public bool showCharacters;
        public bool showWords;
        public bool showLinks;
        public bool showLines;
        public bool showMeshBounds;
        public bool showTextBounds;
        [Space(10)]
        [TextArea(2, 2)]
        public string objectStats;

        [SerializeField]
        private TMP_Text mTextComponent;

        private Transform mTransform;
        private TMP_TextInfo mTextInfo;

        private float mScaleMultiplier;
        private float mHandleSize;


        void OnDrawGizmos()
        {
            if (mTextComponent == null)
            {
                mTextComponent = GetComponent<TMP_Text>();

                if (mTextComponent == null)
                    return;
            }

            mTransform = mTextComponent.transform;

            // Get a reference to the text object's textInfo
            mTextInfo = mTextComponent.textInfo;

            // Update Text Statistics
            objectStats = "Characters: " + mTextInfo.characterCount + "   Words: " + mTextInfo.wordCount + "   Spaces: " + mTextInfo.spaceCount + "   Sprites: " + mTextInfo.spriteCount + "   Links: " + mTextInfo.linkCount
                          + "\nLines: " + mTextInfo.lineCount + "   Pages: " + mTextInfo.pageCount;

            // Get the handle size for drawing the various
            mScaleMultiplier = mTextComponent.GetType() == typeof(TextMeshPro) ? 1 : 0.1f;
            mHandleSize = HandleUtility.GetHandleSize(mTransform.position) * mScaleMultiplier;

            // Draw line metrics
            #region Draw Lines
            if (showLines)
                DrawLineBounds();
            #endregion

            // Draw word metrics
            #region Draw Words
            if (showWords)
                DrawWordBounds();
            #endregion

            // Draw character metrics
            #region Draw Characters
            if (showCharacters)
                DrawCharactersBounds();
            #endregion

            // Draw Quads around each of the words
            #region Draw Links
            if (showLinks)
                DrawLinkBounds();
            #endregion

            // Draw Quad around the bounds of the text
            #region Draw Bounds
            if (showMeshBounds)
                DrawBounds();
            #endregion

            // Draw Quad around the rendered region of the text.
            #region Draw Text Bounds
            if (showTextBounds)
                DrawTextBounds();
            #endregion
        }


        /// <summary>
        /// Method to draw a rectangle around each character.
        /// </summary>
        /// <param name="text"></param>
        void DrawCharactersBounds()
        {
            int characterCount = mTextInfo.characterCount;

            for (int i = 0; i < characterCount; i++)
            {
                // Draw visible as well as invisible characters
                TMP_CharacterInfo characterInfo = mTextInfo.characterInfo[i];

                bool isCharacterVisible = i >= mTextComponent.maxVisibleCharacters ||
                                          characterInfo.lineNumber >= mTextComponent.maxVisibleLines ||
                                          (mTextComponent.overflowMode == TextOverflowModes.Page && characterInfo.pageNumber + 1 != mTextComponent.pageToDisplay) ? false : true;

                if (!isCharacterVisible) continue;

                float dottedLineSize = 6;

                // Get Bottom Left and Top Right position of the current character
                Vector3 bottomLeft = mTransform.TransformPoint(characterInfo.bottomLeft);
                Vector3 topLeft = mTransform.TransformPoint(new Vector3(characterInfo.topLeft.x, characterInfo.topLeft.y, 0));
                Vector3 topRight = mTransform.TransformPoint(characterInfo.topRight);
                Vector3 bottomRight = mTransform.TransformPoint(new Vector3(characterInfo.bottomRight.x, characterInfo.bottomRight.y, 0));

                // Draw character bounds
                if (characterInfo.isVisible)
                {
                    Color color = Color.green;
                    DrawDottedRectangle(bottomLeft, topRight, color);
                }
                else
                {
                    Color color = Color.grey;

                    float whiteSpaceAdvance = Math.Abs(characterInfo.origin - characterInfo.xAdvance) > 0.01f ? characterInfo.xAdvance : characterInfo.origin + (characterInfo.ascender - characterInfo.descender) * 0.03f;
                    DrawDottedRectangle(mTransform.TransformPoint(new Vector3(characterInfo.origin, characterInfo.descender, 0)), mTransform.TransformPoint(new Vector3(whiteSpaceAdvance, characterInfo.ascender, 0)), color, 4);
                }

                float origin = characterInfo.origin;
                float advance = characterInfo.xAdvance;
                float ascentline = characterInfo.ascender;
                float baseline = characterInfo.baseLine;
                float descentline = characterInfo.descender;

                //Draw Ascent line
                Vector3 ascentlineStart = mTransform.TransformPoint(new Vector3(origin, ascentline, 0));
                Vector3 ascentlineEnd = mTransform.TransformPoint(new Vector3(advance, ascentline, 0));

                Handles.color = Color.cyan;
                Handles.DrawDottedLine(ascentlineStart, ascentlineEnd, dottedLineSize);

                // Draw Cap Height & Mean line
                float capline = characterInfo.fontAsset == null ? 0 : baseline + characterInfo.fontAsset.faceInfo.capLine * characterInfo.scale;
                Vector3 capHeightStart = new Vector3(topLeft.x, mTransform.TransformPoint(new Vector3(0, capline, 0)).y, 0);
                Vector3 capHeightEnd = new Vector3(topRight.x, mTransform.TransformPoint(new Vector3(0, capline, 0)).y, 0);

                float meanline = characterInfo.fontAsset == null ? 0 : baseline + characterInfo.fontAsset.faceInfo.meanLine * characterInfo.scale;
                Vector3 meanlineStart = new Vector3(topLeft.x, mTransform.TransformPoint(new Vector3(0, meanline, 0)).y, 0);
                Vector3 meanlineEnd = new Vector3(topRight.x, mTransform.TransformPoint(new Vector3(0, meanline, 0)).y, 0);

                if (characterInfo.isVisible)
                {
                    // Cap line
                    Handles.color = Color.cyan;
                    Handles.DrawDottedLine(capHeightStart, capHeightEnd, dottedLineSize);

                    // Mean line
                    Handles.color = Color.cyan;
                    Handles.DrawDottedLine(meanlineStart, meanlineEnd, dottedLineSize);
                }

                //Draw Base line
                Vector3 baselineStart = mTransform.TransformPoint(new Vector3(origin, baseline, 0));
                Vector3 baselineEnd = mTransform.TransformPoint(new Vector3(advance, baseline, 0));

                Handles.color = Color.cyan;
                Handles.DrawDottedLine(baselineStart, baselineEnd, dottedLineSize);

                //Draw Descent line
                Vector3 descentlineStart = mTransform.TransformPoint(new Vector3(origin, descentline, 0));
                Vector3 descentlineEnd = mTransform.TransformPoint(new Vector3(advance, descentline, 0));

                Handles.color = Color.cyan;
                Handles.DrawDottedLine(descentlineStart, descentlineEnd, dottedLineSize);

                // Draw Origin
                Vector3 originPosition = mTransform.TransformPoint(new Vector3(origin, baseline, 0));
                DrawCrosshair(originPosition, 0.05f / mScaleMultiplier, Color.cyan);

                // Draw Horizontal Advance
                Vector3 advancePosition = mTransform.TransformPoint(new Vector3(advance, baseline, 0));
                DrawSquare(advancePosition, 0.025f / mScaleMultiplier, Color.yellow);
                DrawCrosshair(advancePosition, 0.0125f / mScaleMultiplier, Color.yellow);

                // Draw text labels for metrics
               if (mHandleSize < 0.5f)
               {
                   GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Label"));
                   style.normal.textColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                   style.fontSize = 12;
                   style.fixedWidth = 200;
                   style.fixedHeight = 20;

                   Vector3 labelPosition;
                   float center = (origin + advance) / 2;

                   //float baselineMetrics = 0;
                   //float ascentlineMetrics = ascentline - baseline;
                   //float caplineMetrics = capline - baseline;
                   //float meanlineMetrics = meanline - baseline;
                   //float descentlineMetrics = descentline - baseline;

                   // Ascent Line
                   labelPosition = mTransform.TransformPoint(new Vector3(center, ascentline, 0));
                   style.alignment = TextAnchor.UpperCenter;
                   Handles.Label(labelPosition, "Ascent Line", style);
                   //Handles.Label(labelPosition, "Ascent Line (" + ascentlineMetrics.ToString("f3") + ")" , style);

                   // Base Line
                   labelPosition = mTransform.TransformPoint(new Vector3(center, baseline, 0));
                   Handles.Label(labelPosition, "Base Line", style);
                   //Handles.Label(labelPosition, "Base Line (" + baselineMetrics.ToString("f3") + ")" , style);

                   // Descent line
                   labelPosition = mTransform.TransformPoint(new Vector3(center, descentline, 0));
                   Handles.Label(labelPosition, "Descent Line", style);
                   //Handles.Label(labelPosition, "Descent Line (" + descentlineMetrics.ToString("f3") + ")" , style);

                   if (characterInfo.isVisible)
                   {
                       // Cap Line
                       labelPosition = mTransform.TransformPoint(new Vector3(center, capline, 0));
                       style.alignment = TextAnchor.UpperCenter;
                       Handles.Label(labelPosition, "Cap Line", style);
                       //Handles.Label(labelPosition, "Cap Line (" + caplineMetrics.ToString("f3") + ")" , style);

                       // Mean Line
                       labelPosition = mTransform.TransformPoint(new Vector3(center, meanline, 0));
                       style.alignment = TextAnchor.UpperCenter;
                       Handles.Label(labelPosition, "Mean Line", style);
                       //Handles.Label(labelPosition, "Mean Line (" + ascentlineMetrics.ToString("f3") + ")" , style);

                       // Origin
                       labelPosition = mTransform.TransformPoint(new Vector3(origin, baseline, 0));
                       style.alignment = TextAnchor.UpperRight;
                       Handles.Label(labelPosition, "Origin ", style);

                       // Advance
                       labelPosition = mTransform.TransformPoint(new Vector3(advance, baseline, 0));
                       style.alignment = TextAnchor.UpperLeft;
                       Handles.Label(labelPosition, "  Advance", style);
                   }
               }
            }
        }


        /// <summary>
        /// Method to draw rectangles around each word of the text.
        /// </summary>
        /// <param name="text"></param>
        void DrawWordBounds()
        {
            for (int i = 0; i < mTextInfo.wordCount; i++)
            {
                TMP_WordInfo wInfo = mTextInfo.wordInfo[i];

                bool isBeginRegion = false;

                Vector3 bottomLeft = Vector3.zero;
                Vector3 topLeft = Vector3.zero;
                Vector3 bottomRight = Vector3.zero;
                Vector3 topRight = Vector3.zero;

                float maxAscender = -Mathf.Infinity;
                float minDescender = Mathf.Infinity;

                Color wordColor = Color.green;

                // Iterate through each character of the word
                for (int j = 0; j < wInfo.characterCount; j++)
                {
                    int characterIndex = wInfo.firstCharacterIndex + j;
                    TMP_CharacterInfo currentCharInfo = mTextInfo.characterInfo[characterIndex];
                    int currentLine = currentCharInfo.lineNumber;

                    bool isCharacterVisible = characterIndex > mTextComponent.maxVisibleCharacters ||
                                              currentCharInfo.lineNumber > mTextComponent.maxVisibleLines ||
                                             (mTextComponent.overflowMode == TextOverflowModes.Page && currentCharInfo.pageNumber + 1 != mTextComponent.pageToDisplay) ? false : true;

                    // Track Max Ascender and Min Descender
                    maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
                    minDescender = Mathf.Min(minDescender, currentCharInfo.descender);

                    if (isBeginRegion == false && isCharacterVisible)
                    {
                        isBeginRegion = true;

                        bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);
                        topLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.ascender, 0);

                        //Debug.Log("Start Word Region at [" + currentCharInfo.character + "]");

                        // If Word is one character
                        if (wInfo.characterCount == 1)
                        {
                            isBeginRegion = false;

                            topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                            bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                            bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                            topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                            // Draw Region
                            DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, wordColor);

                            //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                        }
                    }

                    // Last Character of Word
                    if (isBeginRegion && j == wInfo.characterCount - 1)
                    {
                        isBeginRegion = false;

                        topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, wordColor);

                        //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                    }
                    // If Word is split on more than one line.
                    else if (isBeginRegion && currentLine != mTextInfo.characterInfo[characterIndex + 1].lineNumber)
                    {
                        isBeginRegion = false;

                        topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, wordColor);
                        //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                        maxAscender = -Mathf.Infinity;
                        minDescender = Mathf.Infinity;

                    }
                }

                //Debug.Log(wInfo.GetWord(m_TextMeshPro.textInfo.characterInfo));
            }


        }


        /// <summary>
        /// Draw rectangle around each of the links contained in the text.
        /// </summary>
        /// <param name="text"></param>
        void DrawLinkBounds()
        {
            TMP_TextInfo textInfo = mTextComponent.textInfo;

            for (int i = 0; i < textInfo.linkCount; i++)
            {
                TMP_LinkInfo linkInfo = textInfo.linkInfo[i];

                bool isBeginRegion = false;

                Vector3 bottomLeft = Vector3.zero;
                Vector3 topLeft = Vector3.zero;
                Vector3 bottomRight = Vector3.zero;
                Vector3 topRight = Vector3.zero;

                float maxAscender = -Mathf.Infinity;
                float minDescender = Mathf.Infinity;

                Color32 linkColor = Color.cyan;

                // Iterate through each character of the link text
                for (int j = 0; j < linkInfo.linkTextLength; j++)
                {
                    int characterIndex = linkInfo.linkTextfirstCharacterIndex + j;
                    TMP_CharacterInfo currentCharInfo = textInfo.characterInfo[characterIndex];
                    int currentLine = currentCharInfo.lineNumber;

                    bool isCharacterVisible = characterIndex > mTextComponent.maxVisibleCharacters ||
                                              currentCharInfo.lineNumber > mTextComponent.maxVisibleLines ||
                                             (mTextComponent.overflowMode == TextOverflowModes.Page && currentCharInfo.pageNumber + 1 != mTextComponent.pageToDisplay) ? false : true;

                    // Track Max Ascender and Min Descender
                    maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
                    minDescender = Mathf.Min(minDescender, currentCharInfo.descender);

                    if (isBeginRegion == false && isCharacterVisible)
                    {
                        isBeginRegion = true;

                        bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);
                        topLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.ascender, 0);

                        //Debug.Log("Start Word Region at [" + currentCharInfo.character + "]");

                        // If Link is one character
                        if (linkInfo.linkTextLength == 1)
                        {
                            isBeginRegion = false;

                            topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                            bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                            bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                            topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                            // Draw Region
                            DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, linkColor);

                            //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                        }
                    }

                    // Last Character of Link
                    if (isBeginRegion && j == linkInfo.linkTextLength - 1)
                    {
                        isBeginRegion = false;

                        topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, linkColor);

                        //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                    }
                    // If Link is split on more than one line.
                    else if (isBeginRegion && currentLine != textInfo.characterInfo[characterIndex + 1].lineNumber)
                    {
                        isBeginRegion = false;

                        topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, linkColor);

                        maxAscender = -Mathf.Infinity;
                        minDescender = Mathf.Infinity;
                        //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                    }
                }

                //Debug.Log(wInfo.GetWord(m_TextMeshPro.textInfo.characterInfo));
            }
        }


        /// <summary>
        /// Draw Rectangles around each lines of the text.
        /// </summary>
        /// <param name="text"></param>
        void DrawLineBounds()
        {
            int lineCount = mTextInfo.lineCount;

            for (int i = 0; i < lineCount; i++)
            {
                TMP_LineInfo lineInfo = mTextInfo.lineInfo[i];
                TMP_CharacterInfo firstCharacterInfo = mTextInfo.characterInfo[lineInfo.firstCharacterIndex];
                TMP_CharacterInfo lastCharacterInfo = mTextInfo.characterInfo[lineInfo.lastCharacterIndex];

                bool isLineVisible = (lineInfo.characterCount == 1 && (firstCharacterInfo.character == 10 || firstCharacterInfo.character == 11 || firstCharacterInfo.character == 0x2028 || firstCharacterInfo.character == 0x2029)) ||
                                      i > mTextComponent.maxVisibleLines ||
                                     (mTextComponent.overflowMode == TextOverflowModes.Page && firstCharacterInfo.pageNumber + 1 != mTextComponent.pageToDisplay) ? false : true;

                if (!isLineVisible) continue;

                float lineBottomLeft = firstCharacterInfo.bottomLeft.x;
                float lineTopRight = lastCharacterInfo.topRight.x;

                float ascentline = lineInfo.ascender;
                float baseline = lineInfo.baseline;
                float descentline = lineInfo.descender;

                float dottedLineSize = 12;

                // Draw line extents
                DrawDottedRectangle(mTransform.TransformPoint(lineInfo.lineExtents.min), mTransform.TransformPoint(lineInfo.lineExtents.max), Color.green, 4);

                // Draw Ascent line
                Vector3 ascentlineStart = mTransform.TransformPoint(new Vector3(lineBottomLeft, ascentline, 0));
                Vector3 ascentlineEnd = mTransform.TransformPoint(new Vector3(lineTopRight, ascentline, 0));

                Handles.color = Color.yellow;
                Handles.DrawDottedLine(ascentlineStart, ascentlineEnd, dottedLineSize);

                // Draw Base line
                Vector3 baseLineStart = mTransform.TransformPoint(new Vector3(lineBottomLeft, baseline, 0));
                Vector3 baseLineEnd = mTransform.TransformPoint(new Vector3(lineTopRight, baseline, 0));

                Handles.color = Color.yellow;
                Handles.DrawDottedLine(baseLineStart, baseLineEnd, dottedLineSize);

                // Draw Descent line
                Vector3 descentLineStart = mTransform.TransformPoint(new Vector3(lineBottomLeft, descentline, 0));
                Vector3 descentLineEnd = mTransform.TransformPoint(new Vector3(lineTopRight, descentline, 0));

                Handles.color = Color.yellow;
                Handles.DrawDottedLine(descentLineStart, descentLineEnd, dottedLineSize);

                // Draw text labels for metrics
                if (mHandleSize < 1.0f)
                {
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
                    style.fontSize = 12;
                    style.fixedWidth = 200;
                    style.fixedHeight = 20;
                    Vector3 labelPosition;

                    // Ascent Line
                    labelPosition = mTransform.TransformPoint(new Vector3(lineBottomLeft, ascentline, 0));
                    style.padding = new RectOffset(0, 10, 0, 5);
                    style.alignment = TextAnchor.MiddleRight;
                    Handles.Label(labelPosition, "Ascent Line", style);

                    // Base Line
                    labelPosition = mTransform.TransformPoint(new Vector3(lineBottomLeft, baseline, 0));
                    Handles.Label(labelPosition, "Base Line", style);

                    // Descent line
                    labelPosition = mTransform.TransformPoint(new Vector3(lineBottomLeft, descentline, 0));
                    Handles.Label(labelPosition, "Descent Line", style);
                }
            }
        }


        /// <summary>
        /// Draw Rectangle around the bounds of the text object.
        /// </summary>
        void DrawBounds()
        {
            Bounds meshBounds = mTextComponent.bounds;

            // Get Bottom Left and Top Right position of each word
            Vector3 bottomLeft = mTextComponent.transform.position + meshBounds.min;
            Vector3 topRight = mTextComponent.transform.position + meshBounds.max;

            DrawRectangle(bottomLeft, topRight, new Color(1, 0.5f, 0));
        }


        void DrawTextBounds()
        {
            Bounds textBounds = mTextComponent.textBounds;

            Vector3 bottomLeft = mTextComponent.transform.position + (textBounds.center - textBounds.extents);
            Vector3 topRight = mTextComponent.transform.position + (textBounds.center + textBounds.extents);

            DrawRectangle(bottomLeft, topRight, new Color(0f, 0.5f, 0.5f));
        }


        // Draw Rectangles
        void DrawRectangle(Vector3 bl, Vector3 tr, Color color)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(new Vector3(bl.x, bl.y, 0), new Vector3(bl.x, tr.y, 0));
            Gizmos.DrawLine(new Vector3(bl.x, tr.y, 0), new Vector3(tr.x, tr.y, 0));
            Gizmos.DrawLine(new Vector3(tr.x, tr.y, 0), new Vector3(tr.x, bl.y, 0));
            Gizmos.DrawLine(new Vector3(tr.x, bl.y, 0), new Vector3(bl.x, bl.y, 0));
        }

        void DrawDottedRectangle(Vector3 bottomLeft, Vector3 topRight, Color color, float size = 5.0f)
        {
            Handles.color = color;
            Handles.DrawDottedLine(bottomLeft, new Vector3(bottomLeft.x, topRight.y, bottomLeft.z), size);
            Handles.DrawDottedLine(new Vector3(bottomLeft.x, topRight.y, bottomLeft.z), topRight, size);
            Handles.DrawDottedLine(topRight, new Vector3(topRight.x, bottomLeft.y, bottomLeft.z), size);
            Handles.DrawDottedLine(new Vector3(topRight.x, bottomLeft.y, bottomLeft.z), bottomLeft, size);
        }

        void DrawSolidRectangle(Vector3 bottomLeft, Vector3 topRight, Color color, float size = 5.0f)
        {
            Handles.color = color;
            Rect rect = new Rect(bottomLeft, topRight - bottomLeft);
            Handles.DrawSolidRectangleWithOutline(rect, color, Color.black);
        }

        void DrawSquare(Vector3 position, float size, Color color)
        {
            Handles.color = color;
            Vector3 bottomLeft = new Vector3(position.x - size, position.y - size, position.z);
            Vector3 topLeft = new Vector3(position.x - size, position.y + size, position.z);
            Vector3 topRight = new Vector3(position.x + size, position.y + size, position.z);
            Vector3 bottomRight = new Vector3(position.x + size, position.y - size, position.z);

            Handles.DrawLine(bottomLeft, topLeft);
            Handles.DrawLine(topLeft, topRight);
            Handles.DrawLine(topRight, bottomRight);
            Handles.DrawLine(bottomRight, bottomLeft);
        }

        void DrawCrosshair(Vector3 position, float size, Color color)
        {
            Handles.color = color;

            Handles.DrawLine(new Vector3(position.x - size, position.y, position.z), new Vector3(position.x + size, position.y, position.z));
            Handles.DrawLine(new Vector3(position.x, position.y - size, position.z), new Vector3(position.x, position.y + size, position.z));
        }


        // Draw Rectangles
        void DrawRectangle(Vector3 bl, Vector3 tl, Vector3 tr, Vector3 br, Color color)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(bl, tl);
            Gizmos.DrawLine(tl, tr);
            Gizmos.DrawLine(tr, br);
            Gizmos.DrawLine(br, bl);
        }


        // Draw Rectangles
        void DrawDottedRectangle(Vector3 bl, Vector3 tl, Vector3 tr, Vector3 br, Color color)
        {
            var cam = Camera.current;
            float dotSpacing = (cam.WorldToScreenPoint(br).x - cam.WorldToScreenPoint(bl).x) / 75f;
            UnityEditor.Handles.color = color;

            UnityEditor.Handles.DrawDottedLine(bl, tl, dotSpacing);
            UnityEditor.Handles.DrawDottedLine(tl, tr, dotSpacing);
            UnityEditor.Handles.DrawDottedLine(tr, br, dotSpacing);
            UnityEditor.Handles.DrawDottedLine(br, bl, dotSpacing);
        }
        #endif
    }
}


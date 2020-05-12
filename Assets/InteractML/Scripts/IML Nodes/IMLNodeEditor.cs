﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor.Internal;
#if UNITY_EDITOR
using UnityEditor;
using XNodeEditor;
#endif

namespace InteractML
{

    /// <summary>
    /// Super class to deal with node drawing
    /// </summary>
    public class IMLNodeEditor : NodeEditor
    {

        #region Variables

        /// <summary>
        /// Texture2D for node color
        /// </summary>
        /// <returns></returns>
        protected Texture2D NodeColor { get; set; }

        /// <summary>
        /// Texture2D  for line color
        /// </summary>
        /// <returns></returns>
        protected Texture2D SeparatorLineColor { get; set; }

        /// <summary>
        /// Texture2D  for line color
        /// </summary>
        /// <returns></returns>
        protected Texture2D SectionTopColor { get; set; }

        /// <summary>
        /// Float value for line weight
        /// </summary>
        /// <returns></returns>
        protected float WeightOfSeparatorLine { get; set; } = 1f;

        /// <summary>
        /// Float value for line weight
        /// </summary>
        /// <returns></returns>
        protected float WeightOfSectionLine { get; set; } = 2f;

        /// <summary>
        /// Float value for node width
        /// </summary>
        /// <returns></returns>
        protected float NodeWidth { get; set; } = 250f;

        /// <summary>
        /// Rect value for node header
        /// </summary>
        /// <returns></returns>
        protected Rect HeaderRect;

        /// <summary>
        /// Rect value for node header
        /// </summary>
        /// <returns></returns>
        protected Rect LineBelowHeader;

        public Vector2 positionPort;

        string description;

        #endregion



        #region Methods


        public void InitHeaderRects()
        {
            // Set header background Rect values
            HeaderRect.x = 5;
            HeaderRect.y = 0;
            HeaderRect.width = NodeWidth - 10;
            HeaderRect.height = 60;

            // Set Line below header background Rect values
            LineBelowHeader.x = 5;
            LineBelowHeader.y = HeaderRect.height - WeightOfSectionLine;
            LineBelowHeader.width = NodeWidth - 10;
            LineBelowHeader.height = WeightOfSectionLine;
        }

        /// <summary>
        /// Return solid color texture from hex string
        /// </summary>
        public static Texture2D GetColorTextureFromHexString(string colorHex)
        {
            Color color = new Color();
            ColorUtility.TryParseHtmlString(colorHex, out color);
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        /// <summary> Make a simple port field- edited from XNodeEditorGUILayout to edit GUIStyle of port label</summary>
        public static void PortField(GUIContent label, XNode.NodePort port, GUIStyle style, params GUILayoutOption[] options)
        {
            if (port == null) return;
            if (options == null) options = new GUILayoutOption[] { GUILayout.MinWidth(30) };
            Vector2 position = Vector3.zero;
            GUIContent content = label != null ? label : new GUIContent(ObjectNames.NicifyVariableName(port.fieldName));

            // If property is an input, display a regular property field and put a port handle on the left side
            if (port.direction == XNode.NodePort.IO.Input)
            {
                // Display a label
                EditorGUILayout.LabelField(content, style, options);

                Rect rect = GUILayoutUtility.GetLastRect();
                position = rect.position - new Vector2(15, 0);
            }
            // If property is an output, display a text label and put a port handle on the right side
            else if (port.direction == XNode.NodePort.IO.Output)
            {
                // Display a label
                EditorGUILayout.LabelField(content, new GUIStyle(style) { alignment = TextAnchor.UpperRight }, options);

                Rect rect = GUILayoutUtility.GetLastRect();
                position = rect.position + new Vector2(rect.width, 0);
            }
            NodeEditorGUILayout.PortField(position, port);
        }

        public static void IMLNoodleDraw(Vector2 Out, Vector2 In)
        {
            Gradient grad = new Gradient();
            Color a = hexToColor("#ffffff");
            Color b = hexToColor("#000000");
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(a, 0f), new GradientColorKey(b, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
                );
            NoodlePath noodlePath = NodeEditorPreferences.GetSettings().noodlePath;
            float noodleThickness = 10f;
            NoodleStroke noodleStroke = NodeEditorPreferences.GetSettings().noodleStroke;
            
            List<Vector2> gridPoints = new List<Vector2>();
            gridPoints.Add(Out);
            gridPoints.Add(In);
            //NodeEditorGUI.DrawNoodle(grad, noodlePath, noodleStroke, noodleThickness, gridPoints);
        }

        public static Color hexToColor(string hex)
        {
            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        public static Vector2 GetPortPosition(XNode.NodePort port)
        {
            Vector2 position = Vector3.zero;
            if (port == null) return position;

            // If property is an input, display a regular property field and put a port handle on the left side
            if (port.direction == XNode.NodePort.IO.Input)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                position = rect.position - new Vector2(15, - 65);
            }
            // If property is an output, display a text label and put a port handle on the right side
            else if (port.direction == XNode.NodePort.IO.Output)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                position = rect.position + new Vector2(rect.width, 0);
            }

            return position;
        }

        public void DataInToggle(Boolean dataIn, Rect m_InnerBodyRect, Rect m_BodyRect)
        {
            m_InnerBodyRect.x = m_BodyRect.x + 20;
            m_InnerBodyRect.y = m_BodyRect.y + 20;
            m_InnerBodyRect.width = m_BodyRect.width - 20;
            m_InnerBodyRect.height = m_BodyRect.height - 20;

            GUILayout.BeginArea(m_InnerBodyRect);

            if (dataIn)
            {
                DrawPositionValueTogglesAndLabels(Resources.Load<GUISkin>("GUIStyles/InteractMLGUISkin").GetStyle("Green Toggle"));
            }
            else
            {
                DrawPositionValueTogglesAndLabels(Resources.Load<GUISkin>("GUIStyles/InteractMLGUISkin").GetStyle("Red Toggle"));
            }

            GUILayout.EndArea();
        }

        protected virtual void DrawPositionValueTogglesAndLabels(GUIStyle style)
        {
            Debug.Log("need to implement in node editor file");
        }
        protected void HelpButton(string name)
        {
            DescriptionFinder(name);
            GUILayout.Button(new GUIContent("Help", description), Resources.Load<GUISkin>("GUIStyles/InteractMLGUISkin").GetStyle("Help Button"));
            GUI.Label(new Rect(10, 40, 100, 40), GUI.tooltip);
        }

        public void DescriptionFinder(string name)
        {
            if (name == "InteractML.CRIMLConfigurationEditor")
            {
                description = "Teach the Machine node is where you record examples of the movement that you want to be recognised, where you record the training data. It is through this node you set the number of outputs, their format and record examples of movement. This node is connected to one or more extractors through its input port. It is attached to IML Configuration through its output port. ";
            } else if(name == "InteractML.SeriesTrainingExamplesNodeEditor")
            {
                description = "Teach the Machine node is where you record examples of the movement that you want to be recognised, where you record the training data. It is through this node you set the number of outputs, their format and record examples of movement. This node is connected to one or more extractors through its input port. It is attached to IML Configuration through its output port. ";
            }
        }
        #endregion

    }

}

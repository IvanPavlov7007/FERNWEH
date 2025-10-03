using System.Collections;
using UnityEngine;
using Pixelplacement;

namespace Sailboat.Obsolete
{
    public class HierarchyLog : Singleton<HierarchyLog>
    {
        public Color activeColor, inactiveColor, completedColor;

        public static string caption(string content)
        {
            return "    " + content;
        }

        public static string active(string content)
        {
            return applyRichTextColor(content, Instance.activeColor);
        }

        public static string inactive(string content)
        {
            return applyRichTextColor(content, Instance.inactiveColor);
        }

        public static string complete(string content)
        {
            return applyRichTextColor(content, Instance.completedColor);
        }

        static string applyRichTextColor(string content, Color color)
        {
            return "<color="+color.GetHashCode().ToString()+">" + content + "</color>";
        }
    }
}
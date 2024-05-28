using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client.Models
{
    internal class Colors
    {
        public static Brush CharColors(string cha)
        {
            cha = cha.ToUpper();

            var colorMap = new Dictionary<string, Brush>
            {
            {"A", (Brush)(new BrushConverter().ConvertFrom("#FF0000"))},
            {"B", (Brush)(new BrushConverter().ConvertFrom("#00FF00"))},
            {"C", (Brush)(new BrushConverter().ConvertFrom("#0000FF"))},
            {"D", (Brush)(new BrushConverter().ConvertFrom("#FFA500"))},
            {"E", (Brush)(new BrushConverter().ConvertFrom("#FFC0CB"))},
            {"F", (Brush)(new BrushConverter().ConvertFrom("#800080"))},
            {"G", (Brush)(new BrushConverter().ConvertFrom("#FFFF00"))},
            {"H", (Brush)(new BrushConverter().ConvertFrom("#808080"))},
            {"I", (Brush)(new BrushConverter().ConvertFrom("#008000"))},
            {"J", (Brush)(new BrushConverter().ConvertFrom("#800000"))},
            {"K", (Brush)(new BrushConverter().ConvertFrom("#FF4500"))},
            {"L", (Brush)(new BrushConverter().ConvertFrom("#9400D3"))},
            {"M", (Brush)(new BrushConverter().ConvertFrom("#87CEEB"))},
            {"N", (Brush)(new BrushConverter().ConvertFrom("#8B4513"))},
            {"O", (Brush)(new BrushConverter().ConvertFrom("#F08080"))},
            {"P", (Brush)(new BrushConverter().ConvertFrom("#4682B4"))},
            {"Q", (Brush)(new BrushConverter().ConvertFrom("#D2B48C"))},
            {"R", (Brush)(new BrushConverter().ConvertFrom("#DC143C"))},
            {"S", (Brush)(new BrushConverter().ConvertFrom("#556B2F"))},
            {"T", (Brush)(new BrushConverter().ConvertFrom("#B22222"))},
            {"U", (Brush)(new BrushConverter().ConvertFrom("#FFD700"))},
            {"V", (Brush)(new BrushConverter().ConvertFrom("#20B2AA"))},
            {"W", (Brush)(new BrushConverter().ConvertFrom("#FF69B4"))},
            {"X", (Brush)(new BrushConverter().ConvertFrom("#4B0082"))},
            {"Y", (Brush)(new BrushConverter().ConvertFrom("#FA8072"))},
            {"Z", (Brush)(new BrushConverter().ConvertFrom("#00FFFF"))},
            };

            Brush defaultColor = Brushes.Black;

            if (colorMap.ContainsKey(cha))
            {
                return colorMap[cha];
            }
            else
            {
                return defaultColor;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZedGraph;

namespace KLib.KGraphics
{
    public class ZedGraphUtils
    {
        public static GraphPane InitZedGraph(ZedGraphControl zgc, string xlabel="", string ylabel="")
        {
            GraphPane zgPane;

            zgPane = zgc.GraphPane;
            zgPane.XAxis.Title.FontSpec.Size = 12;
            zgPane.IsFontsScaled = false;

            zgPane.Title.FontSpec.Size = 10;
            zgPane.Title.Text = "";

            zgPane.Chart.Border.IsVisible = false;

            zgPane.XAxis.Title.FontSpec.IsBold = false;
            zgPane.XAxis.Title.Text = xlabel;
            zgPane.XAxis.MinorTic.IsAllTics = false;
            zgPane.XAxis.MajorTic.IsInside = false;
            zgPane.XAxis.MajorTic.IsOutside = true;
            zgPane.XAxis.MajorTic.IsOpposite = false;
            zgPane.XAxis.Scale.FontSpec.Size = 10;

            zgPane.YAxis.MajorTic.IsOpposite = false;
            zgPane.YAxis.MinorTic.IsAllTics = false;
            zgPane.YAxis.MajorTic.IsInside = false;
            zgPane.YAxis.MajorTic.IsOutside = true;
            zgPane.YAxis.MajorTic.IsOpposite = false;
            zgPane.YAxis.Scale.FontSpec.Size = 10;
            zgPane.YAxis.Scale.MajorStepAuto = true;
            zgPane.YAxis.Scale.MaxAuto = true;

            zgPane.YAxis.Title.FontSpec.IsBold = false;
            zgPane.YAxis.Title.Text = ylabel;

            return zgPane;
        }

    }
}

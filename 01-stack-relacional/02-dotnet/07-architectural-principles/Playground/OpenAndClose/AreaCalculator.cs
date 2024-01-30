using OpenAndClose.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAndClose;

internal class AreaCalculator
{
    public double TotalArea(Circle[] circles)
    {
        double area = 0;
        foreach (var circle in circles)
        {
            area += circle.Radius * circle.Radius * Math.PI;
        }
        return area;
    }
}
﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

namespace ModernUI.Toolkit.Data.Charting.Charts.Series
{
    /// <summary>
    /// Control that displays values as a 100% stacked column chart visualization.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public class Stacked100ColumnSeries : StackedColumnSeries
    {
        /// <summary>
        /// Initializes a new instance of the Stacked100ColumnSeries class.
        /// </summary>
        public Stacked100ColumnSeries()
        {
            IsStacked100 = true;
        }
    }
}

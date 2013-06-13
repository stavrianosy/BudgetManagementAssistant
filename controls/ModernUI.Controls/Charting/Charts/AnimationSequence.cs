﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

namespace ModernUI.Toolkit.Data.Charting.Charts
{
    /// <summary>
    /// Specifies the supported animation sequences.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public enum AnimationSequence
    {
        /// <summary>
        /// Animates all of the data points simultaneously.
        /// </summary>
        Simultaneous = 0,

        /// <summary>
        /// Animates the data points from first to last.
        /// </summary>
        FirstToLast = 1,

        /// <summary>
        /// Animates the data points from last to first.
        /// </summary>
        LastToFirst = 2
    }
}
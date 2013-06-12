﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

namespace ModernUI.Toolkit.Data.Charting.Charts.Axis
{
    /// <summary>
    /// An object that listens for changes in an axis.
    /// </summary>
    public interface IAxisListener
    {
        /// <summary>
        /// This method is called when the axis is invalidated.
        /// </summary>
        /// <param name="axis">The axis that has been invalidated.</param>
        void AxisInvalidated(IAxis axis);
    }
}

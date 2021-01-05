// ReSharper disable once CheckNamespace

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine.Annotations;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public enum DataEntryGridCellStates
    {
        Enabled = 0,
        ReadOnly = 1,
        Disabled = 2
    }

    public class DataEntryGridCellStyle
    {
        public int DisplayStyleId { get; set; }

        public DataEntryGridCellStates State { get; set; }

        public string ColumnHeader { get; set; }

        protected virtual void OnStateChanged()
        { 
        }
    }
}

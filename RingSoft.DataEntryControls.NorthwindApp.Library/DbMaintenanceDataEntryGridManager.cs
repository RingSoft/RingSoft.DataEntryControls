﻿using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbMaintenance;
using System;
using System.Collections.Generic;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public abstract class DbMaintenanceDataEntryGridManager<TEntity> : DataEntryGridManager
        where TEntity : new()
    {
        public DbMaintenanceViewModelBase ViewModel { get; }

        public DbMaintenanceDataEntryGridManager(DbMaintenanceViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }

        public virtual void LoadGrid(IEnumerable<TEntity> entityList)
        {
            PreLoadGridFromEntity();

            foreach (var entity in entityList)
            {
                var parentRowId = GetParentRowIdFromEntity(entity);
                if (string.IsNullOrEmpty(parentRowId))
                    AddRowFromEntity(entity);
            }

            PostLoadGridFromEntity();
        }

        public virtual void AddRowFromEntity(TEntity entity)
        {
            var newRow = ConstructNewRowFromEntity(entity);
            AddRow(newRow);
            newRow.LoadFromEntity(entity);
            Grid.UpdateRow(newRow);
        }

        protected abstract DbMaintenanceDataEntryGridRow<TEntity> ConstructNewRowFromEntity(TEntity entity);

        protected virtual string GetParentRowIdFromEntity(TEntity entity)
        {
            return string.Empty;
        }

        public override void RaiseDirtyFlag()
        {
            ViewModel.RecordDirty = true;
            base.RaiseDirtyFlag();
        }

        public virtual bool ValidateGrid()
        {
            if (!Grid.CommitEdit())
                return false;

            foreach (var dataEntryGridRow in Rows)
            {
                if (dataEntryGridRow is DbMaintenanceDataEntryGridRow<TEntity> row && !row.IsNew)
                    if (!row.ValidateRow())
                        return false;
            }
            return true;
        }

        public List<TEntity> GetEntityList()
        {
            var result = new List<TEntity>();
            var rowIndex = 0;
            foreach (var dataEntryGridRow in Rows)
            {
                if (dataEntryGridRow is DbMaintenanceDataEntryGridRow<TEntity> row && !row.IsNew)
                {
                    var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                    row.SaveToEntity(entity, rowIndex);
                    result.Add(entity);
                    rowIndex++;
                }
            }

            return result;
        }
    }
}

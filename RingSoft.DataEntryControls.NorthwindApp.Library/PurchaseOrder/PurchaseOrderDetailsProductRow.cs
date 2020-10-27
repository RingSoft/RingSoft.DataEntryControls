using RingSoft.DataEntryControls.NorthwindApp.Library.Model;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.PurchaseOrder
{
    public class PurchaseOrderDetailsProductRow : PurchaseOrderDetailsRow
    {
        public override PurchaseOrderDetailsLineTypes LineType => PurchaseOrderDetailsLineTypes.Product;

        public PurchaseOrderDetailsProductRow(PurchaseOrderDetailsGridManager manager) : base(manager)
        {
        }

        public override void LoadFromEntity(PurchaseDetails entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(PurchaseDetails entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}

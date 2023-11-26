using System.Linq;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class SystemDataRepositoryEfCore : SystemDataRepository
    {

        public override IDbContext GetDataContext()
        {
            return new NorthwindDbContext();
        }

        public override IDbContext GetDataContext(DbDataProcessor dataProcessor)
        {
            return new NorthwindDbContext();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingSoft.DataEntryControls.Engine
{
    public interface ITwoTierProcessingProcedure
    {
        public void SetProgress(int topMax = 0
            , int topValue = 0
            , string topText = ""
            , int bottomMax = 0
            , int bottomValue = 0
            , string bottomText = "");
    }
}

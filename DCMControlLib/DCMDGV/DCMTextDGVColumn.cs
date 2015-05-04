using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DCMControlLib
{
    public class DCMTextDGVColumn:DataGridViewColumn
    {
        public DCMTextDGVColumn()
            : base(new DCMDGVTextCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell. 
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DCMDGVTextCell)))
                {
                    throw new InvalidCastException("Must be a DCMDGVTextCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}

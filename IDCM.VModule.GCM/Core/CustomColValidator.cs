using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Core
{
    class CustomColValidator
    {
        internal CustomColValidator(CustomColDef ccd)
        {
            isRequire = ccd.IsRequire;
            validDesc +="Value of "+ ccd.Alias + " can not be NULL.\n";
            if (ccd.IsUnique)
            {
                sets = new HashSet<string>();
                validDesc += "Value of " + ccd.Alias + " should be unique value.\n";
            }
            if (ccd.Restrict != null)
            {
                restrict = new DCMEx.DCMEx(ccd.Restrict);
                validDesc += "Value of " + ccd.Alias + " should be match the expression:"+ccd.Restrict+".\n";
            }
        }
        public bool checkValid(string cellValue)
        {
            bool isValid = true;
            if (isRequire && (cellValue == null || cellValue.Trim().Length < 1))
            {
                isValid = false;
            }
            if (isValid && cellValue!=null && sets != null)
            {
                if (sets.Contains(cellValue))
                    isValid = false;
                else
                    sets.Add(cellValue);
            }
            if (isValid && restrict != null)
            {
                isValid = restrict.isMatch(cellValue);
            }
            return isValid;
        }
        internal string getValidDescription()
        {
            return validDesc;
        }
        private string validDesc = "";
        private DCMEx.DCMEx restrict;
        /// <summary>
        /// 必选性键值约束
        /// </summary>
        private bool isRequire;
        private HashSet<string> sets = null;
    }
}

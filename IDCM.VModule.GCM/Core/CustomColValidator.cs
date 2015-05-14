using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Core
{
    class CustomColValidator
    {
        #region Constructor&Destructor
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
        #endregion

        #region Methods
        public bool checkValid(string cellValue)
        {
            bool isValid = true;
            
            if (cellValue != null && cellValue.Length > 0)
            {
                if (isValid && sets != null)
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
            }
            else if (isRequire)
            {
                isValid = false;
            }
            return isValid;
        }
        internal string getValidDescription()
        {
            return validDesc;
        }
        #endregion

        #region Members
        private string validDesc = "";
        private DCMEx.DCMEx restrict;
        /// <summary>
        /// 必选性键值约束
        /// </summary>
        private bool isRequire;
        private HashSet<string> sets = null;
        #endregion
    }
}

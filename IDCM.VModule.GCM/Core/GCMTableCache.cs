using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using IDCM.Base.ComPO;

namespace IDCM.Core
{
    internal class GCMTableCache
    {
        public GCMTableCache(ComPO.SignVModel svmodel, DCMControlLib.DCMDataGridView dcmDataGridView_gcm, DCMControlLib.Tree.DCMTreeView dcmTreeView_gcm)
        {
            this.svmodel = svmodel;
            this.dcmDataGridView_gcm = dcmDataGridView_gcm;
            this.dcmTreeView_gcm = dcmTreeView_gcm;
        }
        internal bool signed()
        {
            return auth != null && auth.LoginFlag;
        }

        /// <summary>
        /// 整体数据更新共享锁对象
        /// </summary>
        public readonly object GSyncRoot = new object();
        /// <summary>
        /// 主键名称标记
        /// </summary>
        private string keyName = "";
        /// <summary>
        /// 主键映射表缓存表设定
        /// </summary>
        private Dictionary<string, int> keyIndexs;
        /// <summary>
        /// 缓存表（暂为备用）
        /// </summary>
        private DataTable dataTable;
        //用户登录信息
        private AuthInfo auth;
        private ComPO.SignVModel svmodel;
        /// <summary>
        /// 数据表视图
        /// </summary>
        private DCMControlLib.DCMDataGridView dcmDataGridView_gcm;
        private DCMControlLib.Tree.DCMTreeView dcmTreeView_gcm;
    }
}

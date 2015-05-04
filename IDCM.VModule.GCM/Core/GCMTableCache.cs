using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace IDCM.Core
{
    internal class GCMTableCache
    {

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
        /// <summary>
        /// 数据表视图
        /// </summary>
        private DataGridView dgv;
    }
}

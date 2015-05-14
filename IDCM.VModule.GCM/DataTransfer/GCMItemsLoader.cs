using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Core;
using IDCM.ComPO;
using IDCM.MsgDriver;
using IDCM.DataTransfer;

namespace IDCM.DataTransfer
{
    /// <summary>
    /// 发送网络请求，获取strain信息，构建GCM DataSet
    /// </summary>
    class GCMItemsLoader
    {
        #region Methods

        /// <summary>
        /// 分多次发送网络请求，获取strain信息，将strain数据添加到DataGridView中
        /// 说明：
        /// 1：分为多次网络请求，每次请求获取1页数据，获取到最后一页时请求结束
        /// 2：每次获取数据后调用showDataItems将strain数据添加到DataGridView
        /// </summary>
        /// <returns></returns>
        public static bool loadOverViewData(GCMTableCache gtcache, AuthInfo authInfo)
        {
            int curPage = 1;
            StrainListPage slp = StrainListQueryExecutor.strainListQuery(curPage,"","",authInfo);
            if (slp != null && slp.list != null)
            {
                lock (gtcache.GSyncRoot)
                {
                    foreach (Dictionary<string, string> valMap in slp.list)
                    {
                        gtcache.addOverViewRow(valMap);
                    }
                }
                DCMPublisher.noteJobFeedback(AsyncMsgNotice.GCMItemDetailRender);
                while (hasNextPage(slp, curPage))
                {
                    curPage++;
                    slp = StrainListQueryExecutor.strainListQuery(curPage,"","",authInfo);
                    if (slp != null && slp.list != null)
                    {
                        lock (gtcache.GSyncRoot)
                        {
                            foreach (Dictionary<string, string> valMap in slp.list)
                            {
                                gtcache.addOverViewRow(valMap);
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断分页请求是否存在下一页内容
        /// </summary>
        /// <param name="slp"></param>
        /// <param name="reqPage"></param>
        /// <returns></returns>
        private static bool hasNextPage(StrainListPage slp, int reqPage)
        {
            if (slp != null && slp.totalpage > slp.pageNumber && slp.totalpage > reqPage)
            {
                return true;
            }
            return false;
        }

        internal static bool loadDetailViewData(GCMTableCache gtcache, string sid, AuthInfo authInfo)
        {
            StrainView sv = StrainViewQueryExecutor.strainViewQuery(sid,authInfo);
            gtcache.resetTree(sv,sid);
            return true;
        }
        #endregion
    }
}

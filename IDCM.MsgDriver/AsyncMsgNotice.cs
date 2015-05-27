using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.MsgDriver
{
    /// <summary>
    /// 异步消息类型及附属参数的封装类
    /// </summary>
    public class AsyncMsgNotice
    {
        public static readonly AsyncMsgNotice GCMUserSigned = new AsyncMsgNotice(MsgNoticeType.GCMUserSigned, "GCM User Signed");
        public static readonly AsyncMsgNotice LocalDataImported = new AsyncMsgNotice(MsgNoticeType.LocalDataImported, "Local Data Imported");
        public static readonly AsyncMsgNotice LocalDataExported = new AsyncMsgNotice(MsgNoticeType.LocalDataExported, "Local Data Exported");
        public static readonly AsyncMsgNotice GCMDataLoaded = new AsyncMsgNotice(MsgNoticeType.GCMDataLoaded, "GCM Data Loaded");
        public static readonly AsyncMsgNotice GCMItemDetailRender = new AsyncMsgNotice(MsgNoticeType.GCMItemDetailRender, "GCM Item Detail Render");
        public static readonly AsyncMsgNotice LocalDataPublished = new AsyncMsgNotice(MsgNoticeType.GCMItemDetailRender, "Local Data Published");
        public static readonly AsyncMsgNotice LocalDataChecked = new AsyncMsgNotice(MsgNoticeType.GCMItemDetailRender, "Local Data Checked");
        
        
        
        /// <summary>
        /// For iterator 
        /// </summary>
        public static IEnumerable<AsyncMsgNotice> Values
        {
            get
            {
                yield return GCMUserSigned;
                yield return LocalDataImported;
                yield return LocalDataExported;
                yield return GCMDataLoaded;
                yield return GCMItemDetailRender;
                yield return LocalDataPublished;
                yield return LocalDataChecked;
            }
        }
        private readonly string msgTag;
        private readonly MsgNoticeType msgType;
        private readonly object[] parameters;

        public AsyncMsgNotice(AsyncMsgNotice amsg, params object[] parameters)
        {
            this.msgTag = amsg.msgTag;
            this.msgType = amsg.msgType;
            this.parameters = parameters;
        }

        protected AsyncMsgNotice(MsgNoticeType msgType, string msgTag, object[] parameters = null)
        {
            this.msgTag = msgTag;
            this.msgType = msgType;
            this.parameters = parameters;
        }

        public string MsgTag { get { return msgTag; } }

        public MsgNoticeType MsgType { get { return msgType; } }

        public object[] Parameters { get { return parameters; } }

        public override string ToString()
        {
            return msgType + ":" + msgTag;
        }
    }
    /// <summary>
    /// 预定义的消息类型
    /// </summary>
    public enum MsgNoticeType
    {
        GCMUserSigned = 0,
        LocalDataImported = 1,
        LocalDataExported=2,
        GCMDataLoaded=3,
        GCMItemDetailRender=4,
        LocalDataPublished=5,
        LocalDataChecked=6,
    }
}

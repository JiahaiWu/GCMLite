using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.Base.Utils;
/********************************
 * IDCM GCM Pro
 * Specific Data Admin For Global Catalogue of Microorganisms (GCM) Data Intergration
 * Based from Individual Data Center of Microbial resources (IDCM)
 * 
 * With function requirements:
 * 
 * Desktop software package for Global Catalogue of Microorganisms (GCM)
 * 
 * Server-side storage and publishing
 * 
 * Data sharing
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM
{
    static class Program
    {
        /// <summary>
        /// IDCM应用程序的主入口点。
        /// 说明：
        /// 1.应用程序支持参数请求，请求格式为 IDCMPro.exe [-Option1 arg1] [-Option2 arg2] ...，目前支持项有
        ///   1） -lws ${worksSpacePath}
        ///   1） -reset ${Configkey}
        /// 2.组件依赖主要包括
        ///   1）.Net framework 4.0
        ///   2）Nlog
        ///   3）NPOI
        ///   4) SQLite
        ///   5) Dapper
        ///   6) Newtonsoft.Json
        /// 注意：
        /// 无效参数请求将被忽略处理，参数选项开关大小写敏感。
        /// 
        /// @author JiahaiWu
        /// </summary>
        /// <param name="args">应用命令调用请求附加参数</param>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Dictionary<string, string> preSetttings = commandArgScreening(args);
                if (preSetttings.Count > 0)
                {
                    prepareAppContext(preSetttings);
                    return;
                }
                IDCMAppContext appContext = new IDCMAppContext(preSetttings);
                Application.Run(appContext);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("It's Crash! \n FATAL ERROR:" + ex.Message + "\n" + ex.ToString());
#else
                MessageBox.Show("It's Crash! \n FATAL ERROR:" + ex.Message);
#endif
                log.Info("FATAL!", ex);
            }
        }

        /// <summary>
        /// 控制台请求参数初筛
        /// 说明：
        /// 1. 请求格式为 IDCMPro.exe [-Option1 arg1] [-Option2 arg2] ...，其中option之前的-符号不能省略。
        /// </summary>
        /// <param name="args">应用程序启动时命令行参数</param>
        /// <returns>返回过滤后的“有效”命令-参数表达式集合</returns>
        private static Dictionary<string, string> commandArgScreening(string[] args)
        {
            Dictionary<string, string> actions = new Dictionary<string, string>();
            if (args!=null && args.Length > 0)
            {
                for (int i = 0; i < args.Length - 1; i++)
                {
                    if (args[i].StartsWith("-"))
                    {
                        string cmd = args[i].Substring(1);
                        if (cmd.Length > 0 && prepareAbleCmds.Contains(cmd.ToLower()))
                        {
                            string param = args[i + 1].Trim(new char[] { '"', '\'' });
                            actions[cmd.ToLower()] = param;
                        }
                    }
                }
            }
            return actions;
        }
        /// <summary>
        /// 命令行参数预处理
        /// 说明：
        /// 1. 如果用户指定了reset 参数，则试图重置App.config配置文件中的相关参数设置值
        /// </summary>
        /// <param name="preActions"></param>
        private static void prepareAppContext(Dictionary<string, string> preSetttings)
        {
            foreach (KeyValuePair<string, string> kvpair in preSetttings)
            {
                if (kvpair.Key.Equals(CMD_RESET,StringComparison.CurrentCultureIgnoreCase))
                {
                    ConfigurationHelper.SetAppConfig(kvpair.Value, "");
                }
            }
        }

        private const string CMD_RESET = "reset";
        private static string[] prepareAbleCmds = new string[] { CMD_RESET };

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}

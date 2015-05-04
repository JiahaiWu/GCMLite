﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IDCM.Base
{
    public class SysConstants
    {
        #region Keys of AppSettings
        public const string cacheDir = "/GCMPro/cache/";
        public const string tableDefNote = "tableDef.ini";
        public const string export_note = "export.ini";
        public const string exit_note = "exit.ini";
        public const string default_pack_version = "0.1";
        public const string default_dump_Note = "dumper.ini";
        public const string default_dump_Suffix = ".mdi";
        public const string default_output_name = "GCMOutput.mdi";
        /// <summary>
        /// GCM上传XML格式模板文档
        /// </summary>
        public const string GCMUploadTemplate = "GCMUploadTemplate";
        public const string LastWorkSpace = "LWS";
        public const string LUID = "LUID";
        public const string LPWD = "LPWD";
        /// <summary>
        /// Default Table Setting File
        /// </summary>
        public const string CTableDef = "CTableDef";
        /// <summary>
        /// 帮助文档资源定位
        /// </summary>
        public const string HelpBase = "HelpBase";
        /// <summary>
        /// GCM用户登录请求资源地址
        /// </summary>
        public const string SignInUri = "SignInUri";
        /// <summary>
        /// GCM用户签出请求资源地址
        /// </summary>
        public const string SignOffUri = "SignOffUri";
        /// <summary>
        /// GCM菌种列表信息查询请求资源地址
        /// </summary>
        public const string StrainListUri = "StrainListUri";
        /// <summary>
        /// GCM菌种保藏记录详细信息请求资源地址
        /// </summary>
        public const string StrainViewUri = "StrainViewUri";
        /// <summary>
        /// GCM菌种保藏记录批量导入请求资源地址
        /// </summary>
        public const string XMLImportUri = "XMLImportUri";


        /// <summary>
        /// 探索XML节点稳定性，游标，代表探索的当前位置，初始是0，代表从0开始
        /// </summary>
        public static string Cursor = "Cursor";
        /// <summary>
        /// 探索XML节点稳定性，深度，代表探索深度，如果在当前深度下，XML比较稳定则不会继续向下探索
        /// </summary>
        public static string DetectDepth = "DetectDepth";
        /// <summary>
        /// 探索XML节点稳定性，增长系数，如果在DetectDepth深度下，探索过程中出现strain节点下的attr节点增加的情况，会触发 DetectDepth * DetectDepth，使探索深度翻倍
        /// </summary>
        public static string GrowthFactor = "GrowthFactor";
        /// <summary>
        /// 菌种号字段标识
        /// </summary>
        public static string StrainNumber = "StrainNumber";

        
        #endregion

        /// <summary>
        /// 内部默认的字符编码
        /// </summary>
        public static Encoding defaultEncoding = new UTF8Encoding(true);
        public static string initEnvDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        public static string defaultCfgPath = Path.GetDirectoryName(exePath) + Path.DirectorySeparatorChar + Path.GetFileName(exePath).Replace(".vshost.exe", ".exe") + ".config";
    }
}
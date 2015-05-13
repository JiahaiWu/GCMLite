using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base;
using System.IO;
using System.Text.RegularExpressions;
using IDCM.Base.Utils;
using System.Windows.Forms;
using Newtonsoft.Json;
using IDCM.Core;

namespace IDCM.DataTransfer
{
    class LocalDataDumper
    {
        #region Constructor&Destructor
        public LocalDataDumper()
        {
            this.packVersion = SysConstants.default_pack_version;
            loadLastDumpPath();
        }
        #endregion

        #region Methods
        /**************************************************************
         * 基于DataGridView表单，生成内部数据对象
         * @auther JiahaiWu 2014-03-19
         **************************************************************/
        public LocalDataDumper build(CTableCache ctcache,bool withExported = true)
        {
            List<Dictionary<string,string>> dgvpo = new List<Dictionary<string,string>>();
            for (int i = 0; i < ctcache.getRowCount(); i++)
            {
                Dictionary<string,string> rec=ctcache.getRow(i);
                dgvpo.Add(rec);
            }
            this.proInfo = dgvpo;
            return this;
        }

        private string getCellValue(DataGridViewCell cell)
        {
            if (cell != null && cell.Value != null)
            {
                return cell.Value.ToString();
            }
            return "";
        }
        private string checkReferPath(string fpath, string referPath)
        {
            if (File.Exists(fpath))
                return Path.GetFullPath(fpath);
            fpath = (referPath.EndsWith("/") ? referPath : (referPath + "/")) + fpath;
            if (File.Exists(fpath))
                return Path.GetFullPath(fpath);
            return "";
        }

        /**
        * JSON序列化
        **/
        public string dump(string dumpName = null)
        {
            //JSON序列化
            String jsonStr = JsonConvert.SerializeObject(proInfo);
            if (dumpName != null)
            {
                FileUtil.writeToUTF8File(SysConstants.initEnvDir + SysConstants.cacheDir + dumpName, jsonStr);
                noteLastDumpPath(SysConstants.initEnvDir + SysConstants.cacheDir + dumpName);
                return lastDumpPath;
            }
            else
            {
                //校验标记生成
                string md5Code = HashUtil.md5HexCode(new UTF8Encoding(true).GetBytes(jsonStr));
                FileUtil.writeToUTF8File(SysConstants.initEnvDir + SysConstants.cacheDir + md5Code + SysConstants.default_dump_Suffix, jsonStr);
                noteLastDumpPath(SysConstants.initEnvDir + SysConstants.cacheDir + md5Code + SysConstants.default_dump_Suffix);
                return lastDumpPath;
            }
        }
        /**
        * JSON序列化
        **/
        public string export(string fpath)
        {
            //JSON序列化
            String jsonStr = JsonConvert.SerializeObject(proInfo);
            FileUtil.writeToUTF8File(fpath, jsonStr);
            noteLastDumpPath(fpath);
            return lastDumpPath;
        }
        protected void noteLastDumpPath(string path)
        {
            if (path == null)
                return;
            lastDumpPath = path;
            FileUtil.writeToUTF8File(SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.default_dump_Note, lastDumpPath);
        }
        protected void loadLastDumpPath(bool autoRemoveOld=true)
        {
            FileInfo fileinfo = new FileInfo(SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.default_dump_Note);
            if (fileinfo.Exists)
            {
                String path = FileUtil.readAsUTF8Text(SysConstants.initEnvDir + SysConstants.cacheDir + SysConstants.default_dump_Note);
                lastDumpPath = path;
                if(autoRemoveOld)
                    clearOlderDumpPath(fileinfo.LastWriteTime);
            }
        }
        protected void clearOlderDumpPath(DateTime lastModified)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(SysConstants.initEnvDir + SysConstants.cacheDir);
            if (dirInfo.Exists)
            {
                foreach(FileInfo fileinfo in dirInfo.GetFiles())
                {
                    if (fileinfo.Name.EndsWith(SysConstants.default_dump_Suffix) && DateTime.Compare(fileinfo.LastWriteTime, lastModified) < 0)
                        fileinfo.Delete();
                }
            }
        }
        #endregion

        #region Members
        protected List<Dictionary<string,string>> proInfo;
        private string packVersion;
        private string lastDumpPath = null;
        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Core;
using System.IO;
using IDCM.Base;

namespace IDCM.DataTransfer
{
    class TextExporter
    {
        /// <summary>
        /// 根据历史查询条件导出目标文本数据集
        /// </summary>
        /// <returns></returns>
        public bool exportText(CTableCache ctableCache, string filepath,string spliter = " ")
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int tcount = ctableCache.getRowCount();
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    Dictionary<string, int> maps = ctableCache.getIAttrMapping();
                    //填写表头
                    int i = 0;
                    string key = null;
                    for (i = 0; i < maps.Count - 1; i++)
                    {
                        key = maps.ElementAt(i).Key;
                        strbuilder.Append(key).Append(spliter);
                    }
                    key = maps.ElementAt(i).Key;
                    strbuilder.Append(key);
                    //填写内容////////////////////
                    int ridx = 0;
                    while (ridx < tcount)
                    {
                        Dictionary<int, string> drow = ctableCache.getIRow(ridx);
                        if (drow != null)
                        {
                            string dataLine = convertToText(maps, drow, spliter);
                            strbuilder.Append("\n").Append(dataLine);
                            /////////////
                            if (++ridx % 100 == 0)
                            {
                                Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                                BinaryWriter bw = new BinaryWriter(fs);
                                fs.Write(info, 0, info.Length);
                                strbuilder.Length = 0;
                            }
                        }
                    }
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in TextExporter(...)", ex);
            }
            return true;
        }
        /// <summary>
        /// 以Excel导出数据，数据源DataGridViewSelectedRowCollection
        /// </summary>
        /// <param name="textPath">导出路径</param>
        /// <param name="recordIDs">一条记录的ID</param>
        /// <param name="spliter">字符串分隔符，如果导出的是文本分隔符是"\t"，如果导出的是csv分隔符是","</param>
        /// <returns></returns>
        public bool exportText(CTableCache ctableCache, string filepath, int[] selectedRows, string spliter)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    Dictionary<string, int> maps = ctableCache.getIAttrMapping();
                    //填写表头
                    foreach (string key in maps.Keys)
                    {
                        strbuilder.Append(key).Append(spliter);
                    }
                    //填写内容////////////////////
                    for (int ridx = selectedRows.Length - 1; ridx >= 0; ridx--)
                    {
                        int lindex = selectedRows[ridx];
                        Dictionary<int, string> drow = ctableCache.getIRow(lindex);
                        if (drow != null)
                        {
                            string dataLine = convertToText(maps, drow, spliter);
                            strbuilder.Append("\n").Append(dataLine);
                            /////////////
                            if (++count % 100 == 0)
                            {
                                Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                                BinaryWriter bw = new BinaryWriter(fs);
                                fs.Write(info, 0, info.Length);
                                strbuilder.Length = 0;
                            }
                        }
                    }
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in TextExporter(...)", ex);
            }
            return true;
        }
        /// <summary>
        /// 根据字段将一行记录转换成Text
        /// </summary>
        /// <param name="customAttrDBMapping"></param>
        /// <param name="row"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        private static string convertToText(Dictionary<string, int> customAttrDBMapping, Dictionary<int,string> row, string spliter)
        {
            StringBuilder strbuilder = new StringBuilder();
            foreach (KeyValuePair<string, int> kvpair in customAttrDBMapping)
            {
                strbuilder.Append(row[kvpair.Value]).Append(spliter);
            }
            return strbuilder.ToString();
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
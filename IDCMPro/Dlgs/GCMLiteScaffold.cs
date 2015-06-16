using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using IDCM.Base;

namespace IDCM.Dlgs
{
    public partial class GCMLiteScaffold : Form
    {
        public GCMLiteScaffold()
        {
            InitializeComponent();
        }

        private void button_i18n_EN_Click(object sender, EventArgs e)
        {
            button_i18n_EN.Enabled = false;
            button_i18N_CN.Enabled = false;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en");
            i18nRecalling(ci,checkBox_noMapping.Checked);
            button_i18n_EN.Enabled = true;
            button_i18N_CN.Enabled = true;
        }

        private void button_i18N_CN_Click(object sender, EventArgs e)
        {
            button_i18n_EN.Enabled = false;
            button_i18N_CN.Enabled = false;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("zh-CN");
            i18nRecalling(ci, checkBox_noMapping.Checked);
            button_i18n_EN.Enabled = true;
            button_i18N_CN.Enabled = true;
        }
        private void i18nRecalling(System.Globalization.CultureInfo ci,bool onlyNoMapping=false)
        {
            Dictionary<string,string> textDict=new Dictionary<string,string>();
            List<FileInfo> csFiles = detectAllCSFiles(new DirectoryInfo(projectBasePath));
            foreach(FileInfo fi in csFiles)
            {
                using(FileStream fs =new FileStream(fi.FullName,FileMode.Open))
                {
                    using(StreamReader sr=new StreamReader(fs))
                    {
                        string line=sr.ReadLine();
                        while(line!=null)
                        {
                            MatchCollection mc=Regex.Matches(line,GlobalTextResTextPattern);
                            foreach(Match match in mc)
                            {
                                string text=match.Groups[1].Value;
                                if(text!=null && text.Length>0)
                                {
                                    if(!textDict.ContainsKey(text))
                                    {
                                        textDict[text]=GlobalTextRes.FindText(text,ci);
                                    }
                                }
                            }
                            if(sr.EndOfStream)
                                break;
                            else
                                line=sr.ReadLine();
                        }
                    }
                }
            }
            StringBuilder sb=new StringBuilder();
            foreach(KeyValuePair<string,string> kv in textDict)
            {
                if (kv.Value != null)
                {
                    if(!onlyNoMapping)
                        sb.Append(kv.Key).Append("\t").Append(kv.Value).Append("\t").AppendLine();
                }
                else
                    sb.Append(kv.Key).Append("\t\t").AppendLine();
            }
            this.richTextBox1.Text=sb.ToString();
        }
        private List<FileInfo> detectAllCSFiles(DirectoryInfo baseDir)
        {
            List<FileInfo> fis = new List<FileInfo>();
            foreach(FileInfo fi in baseDir.GetFiles())
            {
                if(fi.Name.EndsWith(".cs") && fi.Name.IndexOf('.').Equals(fi.Name.LastIndexOf('.')))
                {
                    fis.Add(fi);
                }
            }
            foreach(DirectoryInfo di in baseDir.GetDirectories())
            {
                fis.AddRange(detectAllCSFiles(di));
            }
            return fis;
        }
        private string projectBasePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))));
        private string GlobalTextResTextPattern="GlobalTextRes\\.Text\\(\\\"([^\\\"\\)]+)\\\"\\)";
    }
}

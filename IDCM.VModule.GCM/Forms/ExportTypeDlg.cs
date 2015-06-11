using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IDCM.Base.ComPO;
using IDCM.Base.Utils;

namespace IDCM.Forms
{
    public partial class ExportTypeDlg : Form
    {
        #region Constructor&Destructor

        public ExportTypeDlg(string fpath=null)
        {
            InitializeComponent();
            if (fpath != null && fpath.Length>0)
                lastFilePath = fpath;
            this.radioButton_excel.Checked = lastOptionValue.Equals(ExportType.Excel)?radioButton_excel.Checked=true:
                lastOptionValue.Equals(ExportType.JSONList)?radioButton_json.Checked=true:
                lastOptionValue.Equals(ExportType.XML)?radioButton_xml.Checked=true:
                lastOptionValue.Equals(ExportType.TSV)?radioButton_tsv.Checked=true:
                lastOptionValue.Equals(ExportType.CSV)?radioButton_csv.Checked=true:radioButton_excel.Checked=true;
            this.textBox_path.Text = LastFilePath;

            this.radioButton_excel.CheckedChanged+=radioButton_excel_CheckedChanged;
            this.radioButton_json.CheckedChanged += radioButton_json_CheckedChanged;
            this.radioButton_tsv.CheckedChanged+=radioButton_tsv_CheckedChanged;
            this.radioButton_xml.CheckedChanged+=radioButton_xml_CheckedChanged;
            this.radioButton_csv.CheckedChanged += radioButton_csv_CheckedChanged;
            
            this.button_cancel.Text = IDCM.Base.GlobalTextRes.Text("Cancel");
            this.button_confirm.Text = IDCM.Base.GlobalTextRes.Text("Confirm");
            this.label1.Text = IDCM.Base.GlobalTextRes.Text("SavePath")+":";
            this.Text = IDCM.Base.GlobalTextRes.Text("Select File Type For Export");
        }
        #endregion

        #region Events&Handlings

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            this.Dispose();
        }

        private void radioButton_tsv_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_tsv.Checked)
            {
                this.textBox_path.Text=System.Text.RegularExpressions.Regex.Replace(this.textBox_path.Text,@"(\.[A-Za-z]{1,4})$",getDefaultSuffix());
            }
        }

        private void radioButton_xml_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_xml.Checked)
            {
                this.textBox_path.Text=System.Text.RegularExpressions.Regex.Replace(this.textBox_path.Text,@"(\.[A-Za-z]{1,4})$",getDefaultSuffix());
            }
        }

        void radioButton_csv_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_csv.Checked)
            {
                this.textBox_path.Text=System.Text.RegularExpressions.Regex.Replace(this.textBox_path.Text,@"(\.[A-Za-z]{1,4})$",getDefaultSuffix());
            }
        }

        private void radioButton_excel_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_excel.Checked)
            {
                this.textBox_path.Text=System.Text.RegularExpressions.Regex.Replace(this.textBox_path.Text,@"(\.[A-Za-z]{1,4})$",getDefaultSuffix());
            }
        }

        void radioButton_json_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_json.Checked)
            {
                this.textBox_path.Text=System.Text.RegularExpressions.Regex.Replace(this.textBox_path.Text,@"(\.[A-Za-z]{1,4})$",getDefaultSuffix());
            }
        }
        private void button_confirm_Click(object sender, EventArgs e)
        {
            string suffix = getDefaultSuffix();
            FileInfo fi = new FileInfo(textBox_path.Text.Trim());
            if (fi.Exists || (fi.Directory != null && fi.Directory.Exists))
            {
                string fpath = fi.FullName;
                if (Directory.Exists(fpath))
                {
                    MessageBox.Show(IDCM.Base.GlobalTextRes.Text("The save file path should be available, can not be a directory."));
                    //fpath = Path.GetDirectoryName(fpath) + "\\" + CUIDGenerator.getUID(CUIDGenerator.Radix_32) + suffix;
                    return;
                }
                if (Path.GetExtension(fpath) == "")
                {
                    fpath += suffix;
                }
                if (FileUtil.isFileWriteAble(fpath))
                {
                    textBox_path.Text = fpath;
                    lastFilePath = fpath;
                    lastOptionValue = radioButton_excel.Checked ? ExportType.Excel :
                        radioButton_json.Checked ? ExportType.JSONList :
                        radioButton_tsv.Checked ? ExportType.TSV :
                        radioButton_xml.Checked ? ExportType.XML :
                        radioButton_csv.Checked ? ExportType.CSV : ExportType.Excel;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show(IDCM.Base.GlobalTextRes.Text("The save file path should be writeable."));
                }
            }
            else
            {
                MessageBox.Show(IDCM.Base.GlobalTextRes.Text("The save file path should be available."));
            }
        }
        /// <summary>
        /// 双击显示文件浏览对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_path_DoubleClick(object sender, EventArgs e)
        {
            SaveFileDialog fbd = new SaveFileDialog();
            fbd.FileName = CUIDGenerator.getUID(CUIDGenerator.Radix_32) + getDefaultSuffix();
            if(lastFilePath.Length>1)
                fbd.InitialDirectory = Path.GetDirectoryName(lastFilePath);
            if (radioButton_excel.Checked)
                fbd.Filter = "Excel File(*.xls,*.xlsx)|*.xls;*.xlsx;";
            if (radioButton_json.Checked)
                fbd.Filter = "JSON File(*.jso)|*.jso;";
            if (radioButton_csv.Checked)
                fbd.Filter = "CSV File(*.csv)|*.csv;";
            if (radioButton_tsv.Checked)
                fbd.Filter = "Text File(*.tsv)|*.tsv;";
            if (radioButton_xml.Checked)
                fbd.Filter = "XML File(*.xml)|*.xml;";
            fbd.SupportMultiDottedExtensions = false;
            fbd.OverwritePrompt = true;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox_path.Text = fbd.FileName;
            }
        }

        #endregion

        #region Methods

        protected string getDefaultSuffix()
        {
            string suffix = "";
            if (radioButton_json.Checked)
            {
                lastOptionValue = ExportType.JSONList;
                suffix = ".jso";
            }
            else if (radioButton_excel.Checked)
            {
                lastOptionValue = ExportType.Excel;
                suffix = ".xlsx";
            }
            else if (radioButton_csv.Checked)
            {
                lastOptionValue = ExportType.CSV;
                suffix = ".csv";
            }
            else if (radioButton_tsv.Checked)
            {
                lastOptionValue = ExportType.TSV;
                suffix = ".tsv";
            }
            else if (radioButton_xml.Checked)
            {
                lastOptionValue = ExportType.XML;
                suffix = ".xml";
            }
            return suffix;
        }
        #endregion

        #region Members

        private static ExportType lastOptionValue = ExportType.Excel;
        private static string lastFilePath = "C:\\idcm_export.xlsx";
        #endregion

        #region Property

        public static string LastFilePath
        {
            get { return ExportTypeDlg.lastFilePath; }
        }

        public static ExportType LastOptionValue
        {
            get { return ExportTypeDlg.lastOptionValue; }
        }
        #endregion
    }
}

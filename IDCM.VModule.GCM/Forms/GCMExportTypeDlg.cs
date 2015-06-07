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
    public partial class GCMExportTypeDlg : Form
    {
        #region Constructor&Destructor
        
        public GCMExportTypeDlg(string fpath = null)
        {
            InitializeComponent();
            if (fpath != null)
                lastFilePath = fpath;
            this.radioButton_excel.Checked = lastOptionValue.Equals(ExportType.Excel) ? radioButton_excel.Checked = true :
                lastOptionValue.Equals(ExportType.JSONList) ? radioButton_json.Checked = true :
                lastOptionValue.Equals(ExportType.XML) ? radioButton_xml.Checked = true :
                lastOptionValue.Equals(ExportType.TSV) ? radioButton_tsv.Checked = true :
                lastOptionValue.Equals(ExportType.CSV) ? radioButton_csv.Checked = true : radioButton_excel.Checked = true;
            this.textBox_path.Text = LastFilePath;

            this.button_cancel.Text = IDCM.Base.GlobalTextRes.Text("Cancel");
            this.button_confirm.Text = IDCM.Base.GlobalTextRes.Text("Confirm");
            this.export_strain_tree_checkBox.Text = IDCM.Base.GlobalTextRes.Text("export strain tree");
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
        private void button_confirm_Click(object sender, EventArgs e)
        {
            string suffix = getDefaultSuffix();
            updatOptionalStatus();
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

        public void setCheckBoxVisible(bool visible = false)
        {
            groupbox_optional.Visible = visible;
            export_strain_tree_checkBox.Visible = visible;
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
            if (radioButton_excel.Checked)
            {
                lastOptionValue = ExportType.Excel;
                suffix = ".xlsx";
            }
            if (radioButton_csv.Checked)
            {
                lastOptionValue = ExportType.CSV;
                suffix = ".csv";
            }
            if (radioButton_tsv.Checked)
            {
                lastOptionValue = ExportType.TSV;
                suffix = ".tsv";
            }
            if (radioButton_xml.Checked)
            {
                lastOptionValue = ExportType.XML;
                suffix = ".xml";
            }
            return suffix;
        }

        private void updatOptionalStatus()
        {
            exportStainTree = export_strain_tree_checkBox.Checked;
        }
        #endregion

        #region Methods

        private static ExportType lastOptionValue = ExportType.Excel;
        private static string lastFilePath = "C:\\idcm_export.xlsx";
        private static bool exportStainTree = false;
        #endregion

        #region Property
        public static bool ExportStainTree
        {
            get { return exportStainTree; }
        }

        public static string LastFilePath
        {
            get { return lastFilePath; }
        }

        public static ExportType LastOptionValue
        {
            get { return lastOptionValue; }
        }
        #endregion
    }
}

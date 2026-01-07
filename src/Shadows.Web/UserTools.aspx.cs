using Shadows.Data;
using Shadows.Data.Excel;
using Shadows.Web;
using System;
using System.Collections.Generic;
using System.IO;

public partial class UserTools : CustomWebPage
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        pnlLogCopy.Visible = false;
        pnlProcessCopy.Visible = false;
    }
    #endregion

    #region ExcelCopy
    public const string UPLOAD_COPY_FILE_NAME = "excelCopy.xlsm";
    public const string UPLOAD_COPY_FILE_NAME_2 = "excelCopy2.xlsm";
    public const string COPY_PREFIX = "COPY_";

    public List<NamedValue> ExcelCopyData
    {
        get
        {
            object obj = ViewState["_ExcelCopyData"];
            if (obj == null)
                return new List<NamedValue>();
            else
                return (List<NamedValue>)obj;
        }
        set
        {
            this.ViewState["_ExcelCopyData"] = value;
        }
    }

    protected void btnUploadCopy_Click(object sender, EventArgs e)
    {
        if (fuUploadCopy.HasFile)
        {
            FilesManager fm = new FilesManager();
            string filePath = fm.GetTempPath(UPLOAD_COPY_FILE_NAME);
            
            fuUploadCopy.SaveAs(filePath);

            NamedValuesManager nvm = new NamedValuesManager();
            ExcelCopyData = nvm.ReadNamedValues(COPY_PREFIX, filePath);
            int cnt = ExcelCopyData.Count;

            lblLogCopy.Text = String.Format("Počet načtených oblastí: {0}", cnt);
            pnlLogCopy.Visible = (cnt > 0);
            pnlProcessCopy.Visible = (cnt > 0);
        }
    }

    protected void btnProcessCopy_Click(object sender, EventArgs e)
    {
        if (fuProcessCopy.HasFile)
        {
            FilesManager fm = new FilesManager();
            string filePath = fm.GetTempPath(UPLOAD_COPY_FILE_NAME_2);

            fuProcessCopy.SaveAs(filePath);

            NamedValuesManager nvm = new NamedValuesManager();
            MemoryStream ms = nvm.CopyNamedValues(COPY_PREFIX, filePath, ExcelCopyData);
            ReturnExcelFile(ms, fuProcessCopy.FileName);
        }
    }
    #endregion
}
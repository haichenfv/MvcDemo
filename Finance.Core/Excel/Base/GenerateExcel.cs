using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace Core.Excel
{
    public class GenerateExcel
    {
        #region 私有字段
        protected HSSFWorkbook workbook = null;
        #endregion

        #region 属性
        /// <summary>
        /// Excel的Sheet集合
        /// </summary>
        public List<BaseGenerateSheet> SheetList { get; set; }
        #endregion

        #region 构造方法
        public GenerateExcel()
        {
            InitializeWorkbook();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 初始化相关对像
        /// </summary>
        private void InitializeWorkbook()
        {
            workbook = new HSSFWorkbook();
            SheetList = new List<BaseGenerateSheet>();
            #region 右击文件 属性信息

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "http://www.kjy.cn";
            workbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Author = "深圳市跨境易电子商务有限公司"; //填加xls文件作者信息
            si.ApplicationName = "深圳市跨境易电子商务有限公司"; //填加xls文件创建程序信息
            si.LastAuthor = "深圳市跨境易电子商务有限公司"; //填加xls文件最后保存者信息
            si.Comments = "深圳市跨境易电子商务有限公司"; //填加xls文件作者信息
            si.Title = "深圳市跨境易电子商务有限公司"; //填加xls文件标题信息
            si.Subject = "深圳市跨境易电子商务有限公司"; //填加文件主题信息
            si.CreateDateTime = DateTime.Now;
            workbook.SummaryInformation = si;

            #endregion
        }
        /// <summary>
        /// 生成Excel并返回内存流
        /// </summary>
        /// <returns></returns>
        private MemoryStream ExportExcel()
        {
            foreach (BaseGenerateSheet sheet in SheetList)
            {
                ISheet sh = null;
                if (string.IsNullOrEmpty(sheet.SheetName))
                    sh = workbook.CreateSheet();
                else
                    sh = workbook.CreateSheet(sheet.SheetName);
                sheet.Workbook = this.workbook;
                sheet.GenSheet(sh);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 导出到Excel文件
        /// </summary>
        /// <param name="strFileName">保存位置</param>
        public void ExportExcel(string strFileName)
        {
            using (MemoryStream ms = ExportExcel())
            {
                if (!Directory.Exists(Path.GetDirectoryName(strFileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(strFileName));
                }
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }
        #endregion
    }
}

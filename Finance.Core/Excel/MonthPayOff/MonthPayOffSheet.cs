using Core.Receivable;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Excel
{
    /// <summary>
    /// 总表
    /// </summary>
    public class MonthPayOffSheet : GenerateSheet<Core.Receivable.MonthPayOff>
    {
        public MonthPayOffSheet(List<Core.Receivable.MonthPayOff> dataSource, string sheetName)
            : base(dataSource, sheetName)
        {

        }

        protected override List<ColumnsMapping> InitializeColumnHeadData()
        {
            List<ColumnsMapping> result = new List<ColumnsMapping>();
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "月份",
                ColumnsData = GetPropertyName(p => p.PayOffMonth),
                ColumnsIndex = 0,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "提单数量",
                ColumnsData = GetPropertyName(p => p.LoadBillCounts),
                ColumnsIndex = 1,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "包裹总数",
                ColumnsData = GetPropertyName(p => p.OrderCounts),
                ColumnsIndex = 2,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "预计总成本",
                ColumnsData = GetPropertyName(p => p.LoadBillCounts),
                ColumnsIndex = 3,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "真实总成本",
                ColumnsData = GetPropertyName(p => p.LoadBillCounts),
                ColumnsIndex = 4,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "预计总收入",
                ColumnsData = GetPropertyName(p => p.LoadBillCounts),
                ColumnsIndex = 5,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "真实总收入",
                ColumnsData = GetPropertyName(p => p.LoadBillCounts),
                ColumnsIndex = 6,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "总毛利",
                ColumnsData = GetPropertyName(p => p.LoadBillCounts),
                ColumnsIndex = 7,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "毛利率",
                ColumnsData = GetPropertyName(p => p.LoadBillCounts),
                ColumnsIndex = 8,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "备注",
                ColumnsData = GetPropertyName(p => p.LoadBillCounts),
                ColumnsIndex = 9,
                IsTotal = false,
                Width = 15
            });
            return result;
        }

        protected override void SetColumnHead(NPOI.SS.UserModel.ISheet sheet, ref int rowIndex)
        {
            if (this.ColumnHeadList.Count > 0)
            {
                // 所有列头居中
                this.HeadStyle.Alignment = HorizontalAlignment.Center;
                for (int i = 0; i < 2; i++)
                {
                    IRow row = sheet.CreateRow(rowIndex);
                    foreach (ColumnsMapping cm in this.ColumnHeadList)
                    {
                        ICell cell = null;
                        if (i == 0)
                        {
                            if (cm.ColumnsIndex < 3 || cm.ColumnsIndex == 9)
                            {
                                // 合并行
                                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 1, cm.ColumnsIndex, cm.ColumnsIndex));
                                cell = row.CreateCell(cm.ColumnsIndex);
                                // 设置列宽
                                SetColumnsWidth(sheet, cm.ColumnsIndex, cm.Width);
                                // 设置列头样式
                                cell.CellStyle = this.HeadStyle;
                                cell.SetCellValue(cm.ColumnsText);
                            }
                            else if (cm.ColumnsIndex == 3 || cm.ColumnsIndex == 5 || cm.ColumnsIndex == 7)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cm.ColumnsIndex, cm.ColumnsIndex + 1));
                                cell = row.CreateCell(cm.ColumnsIndex);
                                SetColumnsWidth(sheet, cm.ColumnsIndex, cm.Width);
                                cell.CellStyle = this.HeadStyle;
                                if (cm.ColumnsIndex == 3)
                                    cell.SetCellValue("成本");
                                else if (cm.ColumnsIndex == 5)
                                    cell.SetCellValue("收入");
                                else if (cm.ColumnsIndex == 7)
                                    cell.SetCellValue("毛利");
                            }
                        }
                        else
                        {
                            if (cm.ColumnsIndex >= 3 && cm.ColumnsIndex <= 8)
                            {
                                cell = row.CreateCell(cm.ColumnsIndex);
                                cell.CellStyle = this.HeadStyle;
                                SetColumnsWidth(sheet, cm.ColumnsIndex, cm.Width);
                                cell.SetCellValue(cm.ColumnsText);
                            }
                            else if (cm.ColumnsIndex < 3 || cm.ColumnsIndex == 9)
                            {
                                cell = row.CreateCell(cm.ColumnsIndex);
                                cell.CellStyle = this.HeadStyle;
                            }
                        }
                    }
                    rowIndex++;
                }
            }
        }

        protected override void SetTotal(NPOI.SS.UserModel.ISheet sheet, ref int rowIndex, int startRowIndex)
        {
            base.SetTotal(sheet, ref rowIndex, startRowIndex);
        }
    }
}

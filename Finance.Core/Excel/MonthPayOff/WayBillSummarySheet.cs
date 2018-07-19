using Core.Reconciliation;
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
    /// 运单汇总Sheet
    /// </summary>
    public class WayBillSummarySheet : GenerateSheet<WayBillReconciliation>
    {
        public WayBillSummarySheet(List<WayBillReconciliation> dataSource, string sheetName)
            : base(dataSource, sheetName)
        {
        }

        protected override List<ColumnsMapping> InitializeColumnHeadData()
        {
            List<ColumnsMapping> result = new List<ColumnsMapping>();
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "客户名称",
                ColumnsData = GetPropertyName(p => p.CusName),
                ColumnsIndex = 0,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "运单号",
                ColumnsData = GetPropertyName(p => p.ExpressNo),
                ColumnsIndex = 1,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "提单号",
                ColumnsData = GetPropertyName(p => p.LoadBillNum),
                ColumnsIndex = 2,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "重量（kg）",
                ColumnsData = GetPropertyName(p => p.Weight),
                ColumnsIndex = 3,
                IsTotal = true,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "邮政邮资",
                ColumnsData = GetPropertyName(p => p.WayBillFee),
                ColumnsIndex = 4,
                IsTotal = true,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "邮政邮件处理费",
                ColumnsData = GetPropertyName(p => p.ProcessingFee),
                ColumnsIndex = 5,
                IsTotal = true,
                Width = 18
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "其他费用",
                ColumnsData = GetPropertyName(p => p.CostOtherFee),
                ColumnsIndex = 6,
                IsTotal = true,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "客户运费",
                ColumnsData = GetPropertyName(p => p.ExpressFee),
                ColumnsIndex = 7,
                IsTotal = true,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "客户操作费",
                ColumnsData = GetPropertyName(p => p.OperateFee),
                ColumnsIndex = 8,
                IsTotal = true,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "客户其他费用",
                ColumnsData = GetPropertyName(p => p.InComeOtherFee),
                ColumnsIndex = 9,
                IsTotal = true,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "运费毛利",
                ColumnsData = GetPropertyName(p => p.WayBillProfit),
                ColumnsIndex = 10,
                IsTotal = true,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "总毛利",
                ColumnsData = string.Empty,
                ColumnsIndex = 11,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "毛利率",
                ColumnsData = string.Empty,
                ColumnsIndex = 12,
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
                            if (cm.ColumnsIndex < 4)
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
                            else if (cm.ColumnsIndex == 4 || cm.ColumnsIndex == 7 || cm.ColumnsIndex == 10)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cm.ColumnsIndex, cm.ColumnsIndex + 2));
                                cell = row.CreateCell(cm.ColumnsIndex);
                                SetColumnsWidth(sheet, cm.ColumnsIndex, cm.Width);
                                cell.CellStyle = this.HeadStyle;
                                if (cm.ColumnsIndex == 4)
                                    cell.SetCellValue("成本");
                                else if (cm.ColumnsIndex == 7)
                                    cell.SetCellValue("收入");
                                else if (cm.ColumnsIndex == 10)
                                    cell.SetCellValue("毛利");
                                for (int j = 4; j <= 12; j++)
                                {
                                    if (j == 4 || j == 7 || j == 10)
                                        continue;
                                    cell = row.CreateCell(j);
                                    cell.CellStyle = this.HeadStyle;
                                }
                            }
                        }
                        else
                        {
                            if (cm.ColumnsIndex >= 4 && cm.ColumnsIndex <= 12)
                            {
                                cell = row.CreateCell(cm.ColumnsIndex);
                                cell.CellStyle = this.HeadStyle;
                                SetColumnsWidth(sheet, cm.ColumnsIndex, cm.Width);
                                cell.SetCellValue(cm.ColumnsText);
                            }
                            else if (cm.ColumnsIndex < 4)
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

        protected override void SetCellValue(ICell cell, int rowIndex, string drValue, ColumnsMapping columns)
        {
            if (columns.ColumnsIndex == 11 || columns.ColumnsIndex == 12)
            {
                int currRowIndex = rowIndex + 1;

                string colE = string.Format("{0}{1}", CellReference.ConvertNumToColString(4), currRowIndex); // 邮政邮资        WayBillFee
                string colF = string.Format("{0}{1}", CellReference.ConvertNumToColString(5), currRowIndex); // 邮政邮件处理费  ProcessingFee
                string colG = string.Format("{0}{1}", CellReference.ConvertNumToColString(6), currRowIndex); // 其他费用        CostOtherFee

                string colH = string.Format("{0}{1}", CellReference.ConvertNumToColString(7), currRowIndex); // 客户运费        ExpressFee
                string colI = string.Format("{0}{1}", CellReference.ConvertNumToColString(8), currRowIndex); // 客户操作费      OperateFee
                string colJ = string.Format("{0}{1}", CellReference.ConvertNumToColString(9), currRowIndex); // 客户其他费用    InComeOtherFee

                string HToJ = string.Format("({0} + {1} + {2})", colH, colI, colJ);
                string EToG = string.Format("({0} + {1} + {2})", colE, colF, colG);
                cell.CellStyle = this.ContentsStyle;
                if (columns.ColumnsIndex == 11)
                {
                    //(H7 + I7 + J7) - (E7 + F7 + G7)
                    cell.SetCellFormula(string.Format("{0} - {1}", HToJ, EToG)); // 设置公式
                }
                else if (columns.ColumnsIndex == 12)
                {
                    // if ((H7 + I7 + J7) = 0, 0, ((H7 + I7 + J7) - (E7 + F7 + G7) / (H7 + I7 + J7))
                    cell.SetCellFormula(string.Format("if ({0} = 0, 0, ({1} - {2} / {3})", HToJ, HToJ, EToG, HToJ)); // 设置公式
                }
            }
            else
            {
                base.SetCellValue(cell, rowIndex, drValue, columns);
            }
        }

        protected override void SetTotalCellValue(ICell cell, int rowIndex, int startRowIndex, ColumnsMapping columns)
        {
            if (columns.ColumnsIndex == 0)
            {
                cell.CellStyle = this.TotalStyle;
                cell.SetCellValue("总计：");
            }
            else
            {
                base.SetTotalCellValue(cell, rowIndex, startRowIndex, columns);
            }
        }
    }
}

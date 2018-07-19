using Core.Reconciliation;
using NPOI.HSSF.UserModel;
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
    /// 提单汇总
    /// </summary>
    public class LoadBillSheet : GenerateSheet<LoadBillReconciliation>
    {
        public LoadBillSheet(List<LoadBillReconciliation> dataSource, string sheetName)
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
                ColumnsText = "提单重量（kg）",
                ColumnsData = GetPropertyName(p => p.FeeWeight),
                ColumnsIndex = 1,
                IsTotal = false,
                Width = 17
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "包裹数量",
                ColumnsData = GetPropertyName(p => p.ExpressCount),
                ColumnsIndex = 2,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "邮政地勤费",
                ColumnsData = GetPropertyName(p => p.GroundHandlingFee),
                ColumnsIndex = 3,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "邮政仓租",
                ColumnsData = GetPropertyName(p => p.CostStoreFee),
                ColumnsIndex = 4,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "邮政运费",
                ColumnsData = GetPropertyName(p => p.CostExpressFee),
                ColumnsIndex = 5,
                IsTotal = false,
                Width = 18
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "邮政邮件处理费",
                ColumnsData = GetPropertyName(p => p.CostOperateFee),
                ColumnsIndex = 6,
                IsTotal = false,
                Width = 18
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "其他费用",
                ColumnsData = GetPropertyName(p => p.CostOtherFee),
                ColumnsIndex = 7,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "客户提货费",
                ColumnsData = GetPropertyName(p => p.InComeLoadFee),
                ColumnsIndex = 8,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "客户仓租",
                ColumnsData = GetPropertyName(p => p.InComeStoreFee),
                ColumnsIndex = 9,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "客户运费",
                ColumnsData = GetPropertyName(p => p.InComeExpressFee),
                ColumnsIndex = 10,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "客户操作费",
                ColumnsData = GetPropertyName(p => p.InComeOperateFee),
                ColumnsIndex = 11,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "其他费用",
                ColumnsData = GetPropertyName(p => p.InComeOtherFee),
                ColumnsIndex = 12,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "总毛利",
                ColumnsData = "",
                ColumnsIndex = 13,
                IsTotal = false,
                Width = 15
            });
            result.Add(new ColumnsMapping()
            {
                ColumnsText = "毛利率",
                ColumnsData = "",
                ColumnsIndex = 14,
                IsTotal = false,
                Width = 15
            });

            return result;
        }

        protected override void SetColumnHead(NPOI.SS.UserModel.ISheet sheet, ref int rowIndex)
        {
            if (this.ColumnHeadList.Count > 0)
            {
                // 冻结
                sheet.CreateFreezePane(1, 4);
                // 数据从第3行开始显示
                rowIndex = rowIndex + 2;
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
                            if (cm.ColumnsIndex < 3 || cm.ColumnsIndex == 13 || cm.ColumnsIndex == 14)
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
                            else if (cm.ColumnsIndex == 3 || cm.ColumnsIndex == 8)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, cm.ColumnsIndex, cm.ColumnsIndex + 4));
                                cell = row.CreateCell(cm.ColumnsIndex);
                                SetColumnsWidth(sheet, cm.ColumnsIndex, cm.Width);
                                cell.CellStyle = this.HeadStyle;
                                if (cm.ColumnsIndex == 3)
                                    cell.SetCellValue("成本");
                                else if (cm.ColumnsIndex == 8)
                                    cell.SetCellValue("收入");
                                for (int j = 3; j <= 12; j++)
                                {
                                    if (j == 3 || j == 8)
                                        continue;
                                    cell = row.CreateCell(j);
                                    cell.CellStyle = this.HeadStyle;
                                }
                            }
                        }
                        else
                        {
                            if (cm.ColumnsIndex >= 3 && cm.ColumnsIndex <= 12)
                            {
                                cell = row.CreateCell(cm.ColumnsIndex);
                                cell.CellStyle = this.HeadStyle;
                                SetColumnsWidth(sheet, cm.ColumnsIndex, cm.Width);
                                cell.SetCellValue(cm.ColumnsText);
                            }
                            else if (cm.ColumnsIndex < 3 || cm.ColumnsIndex == 13 || cm.ColumnsIndex == 14)
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
            if (columns.ColumnsIndex == 13 || columns.ColumnsIndex == 14)
            {
                int currRowIndex = rowIndex + 1;
                string colD = string.Format("{0}{1}", CellReference.ConvertNumToColString(3), currRowIndex); // 邮政地勤费       GroundHandlingFee
                string colE = string.Format("{0}{1}", CellReference.ConvertNumToColString(4), currRowIndex); // 邮政仓租         CostStoreFee
                string colF = string.Format("{0}{1}", CellReference.ConvertNumToColString(5), currRowIndex); // 邮政运费         CostExpressFee
                string colG = string.Format("{0}{1}", CellReference.ConvertNumToColString(6), currRowIndex); // 邮政邮件处理费   CostOperateFee
                string colH = string.Format("{0}{1}", CellReference.ConvertNumToColString(7), currRowIndex); // 其他费用         CostOtherFee

                string colJ = string.Format("{0}{1}", CellReference.ConvertNumToColString(8), currRowIndex); // 客户提货费       InComeLoadFee
                string colK = string.Format("{0}{1}", CellReference.ConvertNumToColString(9), currRowIndex); // 客户仓租         InComeStoreFee
                string colL = string.Format("{0}{1}", CellReference.ConvertNumToColString(10), currRowIndex); // 客户运费        InComeExpressFee
                string colM = string.Format("{0}{1}", CellReference.ConvertNumToColString(11), currRowIndex); // 客户操作费      InComeOperateFee
                string colN = string.Format("{0}{1}", CellReference.ConvertNumToColString(12), currRowIndex); // 其他费用        InComeOtherFee

                string DToH = string.Format("({0} + {1} + {2} + {3} + {4})", colD, colE, colF, colG, colH);
                string JToN = string.Format("({0} + {1} + {2} + {3} + {4})", colJ, colK, colL, colM, colN);

                cell.CellStyle = this.ContentsStyle;
                cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                if (columns.ColumnsIndex == 13)
                {
                    // (J3 + K3 + L3 + M3 + N3) - (D3 + E3 + F3 + G3 + H3)
                    string formula = string.Format("{0} - {1}", JToN, DToH);
                    cell.SetCellFormula(formula); // 设置公式
                }
                else if (columns.ColumnsIndex == 14)
                {
                    // if ((J3 + K3 + L3 + M3 + N3) = 0, 0, (1 - (D3 + E3 + F3 + G3 + H3) / (J3 + K3 + L3 + M3 + N3)) * 100)
                    string formula = string.Format("if ({0} = 0, 0, (1 - {1} / {2}) * 100)", JToN, DToH, JToN);
                    cell.SetCellFormula(formula); // 设置公式
                }
            }
            else
            {
                base.SetCellValue(cell, rowIndex, drValue, columns);
            }
        }
    }
}

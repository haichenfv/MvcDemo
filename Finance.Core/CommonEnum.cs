using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core
{
    /// <summary>
    /// 结算状态
    /// </summary>
    public enum PayStatus
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("待扣款")]
        StayReconciliation = 0,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("已扣款")]
        StayPayment = 1,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("待生成结算单")]
        HasPayment = 2,
    }
    /// <summary>
    /// 提单状态
    /// </summary>
    public enum BillStatusEnum
    {
        /// <summary>
        /// 待报关
        /// </summary>
        [Description("待报关")]
        CustomsDeclaration = 0,
        /// <summary>
        ///资料初审中
        /// </summary>
        [Description("资料初审中")]
        CustomsImagesUpload = 1,
        /// <summary>
        /// 申报资料已上传
        /// </summary>
        [Description("申报资料已上传")]
        DataUploaded = 2,
        /// <summary>
        /// 资料复审中
        /// </summary>
        [Description("资料复审中")]
        StartAudit = 3,
        /// <summary>
        /// 开始申报
        /// </summary>
        [Description("开始申报")]
        StartDeclare = 4,
        /// <summary>
        /// 清关过机
        /// </summary>
        [Description("清关过机")]
        DeclarationPass = 5,

        /// <summary>
        /// 清关完成
        /// </summary>
        [Description("清关完成")]
        ClearanceFinsh = 7

    }
    /// <summary>
    /// 费用类型
    /// </summary>
    public enum WaybillFeeType
    {
        /// <summary>
        /// 基本运费
        /// </summary>
        [Description("基本运费")]
        ExpressFee = 1,
        /// <summary>
        /// 提货费
        /// </summary>
        [Description("提货费")]
        DeliveryFee = 2,
        /// <summary>
        ///	危险标识处理费 3
        /// </summary>
        [Description("危险标识处理费")]
        DangerMarkFee = 3,
        /// <summary>
        ///	换单费 4
        /// </summary>
        [Description("换单费")]
        ChangeOrderFee = 4,
        /// <summary>
        ///	陆运费(自提) 5
        /// </summary>
        [Description("陆运费(自提)")]
        LuShippingFee = 5,
        /// <summary>
        ///	陆运费(二次派送) 6
        /// </summary>
        [Description("陆运费(二次派送)")]
        LuShippingTwoFee = 6,
        /// <summary>
        ///	身份证验证费 7
        /// </summary>
        [Description("身份证验证费")]
        IdentityVerifyFee = 7,
        /// <summary>
        ///	标准快递清关费 8
        /// </summary>
        [Description("标准快递清关费")]
        StandardClearanceFee = 8,
        /// <summary>
        ///	经济快递清关费 9
        /// </summary>
        [Description("经济快递清关费")]
        EconomyClearanceFee = 9,
        /// <summary>
        ///	重新申报操作费 10
        /// </summary>
        [Description("重新申报操作费")]
        RepeatDeclarationFee = 10,
        /// <summary>
        ///	退件再派送费 11
        /// </summary>
        [Description("退件再派送费")]
        BackPiecesDeliveryFee = 11,
        /// <summary>
        ///	有信息无件费 12
        /// </summary>
        [Description("有信息无件费")]
        ThereInfoNoMemberFee = 12,
        /// <summary>
        ///	丢失件 13
        /// </summary>
        [Description("丢失件")]
        LoseFee = 13,
        /// <summary>
        ///	外包装破损件 14
        /// </summary>
        [Description("外包装破损件")]
        DamagedPackaging = 14
    }
    /// <summary>
    /// 快递类型
    /// </summary>
    public enum ExpressTypeEnum
    {
        /// <summary>
        ///	标准快递
        /// </summary>
        [Description("国内进口商业快件物品限时递")]
        Standard = 0,
        /// <summary>
        ///	经济快递
        /// </summary>
        [Description("国内进口商业快件物品经济时限")]
        Economy = 1,
    }
    /// <summary>
    /// 运单数据异常类型 组合类型
    /// </summary>
    public enum WayBillExceptionTypeEnum
    {
        /// <summary>
        /// 邮政运单不存在
        /// </summary>
        [Description("邮政运单不存在")]
        UnableToMatch = 1,
        /// <summary>
        /// 运单上月已导入
        /// </summary>
        [Description("运单上月已导入")]
        WaybillAlreadyClearing = 2,
        /// <summary>
        /// 本月运单号重复
        /// </summary>
        [Description("本月运单号重复")]
        WaybillNoRepeat = 13,
        /// <summary>
        /// 快递类型不一致
        /// </summary>
        [Description("快递类型不一致")]
        ExpressTypeNotSame = 4,

        /// <summary>
        /// 本月运单号重复且邮政运单不存在 13,1
        /// </summary>
        [Description("本月运单号重复,邮政运单不存在")]
        WaybillNoRepeatAndUnableToMatch = 14,

        /// <summary>
        ///运单上月已导入且 13,2
        /// </summary>
        [Description("本月运单号重复,运单上月已导入")]
        WNRAndWAC = 15,

        /// <summary>
        ///运单上月已导入且快递类型不一致 2,4
        /// </summary>
        [Description("运单上月已导入,快递类型不一致")]
        WACAndETNS = 6,

        /// <summary>
        ///本月运单号重复且快递类型不一致 13,4
        /// </summary>
        [Description("本月运单号重复,快递类型不一致")]
        WNPAndEPNS = 17,

        /// <summary>
        ///本月运单号重复且快递类型不一致且运单上月已导入 13,4,2
        /// </summary>
        [Description("本月运单号重复,快递类型不一致,运单上月已导入")]
        WACAndETNSAndWAC = 19
    }
    public enum LoadBillExceptionTypeEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        ///  本月提单号重复
        /// </summary>
        [Description("本月提单号重复")]
        LoadBillNoRepeat = 1,
        /// <summary>
        /// 邮政提单不存在
        /// </summary>
        [Description("邮政提单不存在")]
        UnableToMatch = 2,
        /// <summary>
        /// 提单上月已导入
        /// </summary>
        [Description("提单上月已导入")]
        ImportRepeat = 3,
        /// <summary>
        /// 提单上月已导入，本月提单号重复
        /// </summary>
        [Description("提单上月已导入，本月提单号重复")]
        IRepeatLRepeat = 4,
        /// <summary>
        /// 邮政提单不存在，提单号重复
        /// </summary>
        [Description("邮政提单不存在，提单号重复")]
        IRepeatUMatch = 5
    }
    /// <summary>
    /// 对账单状态
    /// </summary>
    public enum StatementEnum
    {
        [Description("未添加到月结表")]
        AddedMonthPayOff = 0,
        [Description("已添加到月结表")]
        NoAddedMonthPayOff = 1
    }
}

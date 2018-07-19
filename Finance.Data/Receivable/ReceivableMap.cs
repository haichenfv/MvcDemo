
using Core;
using Core.Receivable;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Receivable
{
    public class LoadBillInComeMap : ClassMapping<LoadBillInCome>
    {
        public LoadBillInComeMap()
        {
            Table("LoadBillInCome");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.LoadBillNum);
            ManyToOne(p => p.CustomerInfoBy, m => { m.Column("CustomerID"); });
            Property(p => p.BussinessType);
            Property(p => p.PayStatus, x => x.Type<NHibernate.Type.EnumType<PayStatus>>());
            Property(p => p.PayTime);
            Property(p => p.StoreFee);
            Property(p => p.LoadFee);
            Property(p => p.BusinessTime);
            Property(p => p.TotalCollectFees);
            Property(p => p.CreateTime);
            Property(p => p.CompanyID);
            Property(p => p.DeliveryID);
            Property(p => p.TotalOperateFee);
            Property(p => p.OtherFee);
            Property(p => p.FeeRemark);
            Property(p => p.BillStatus, x => x.Type<NHibernate.Type.EnumType<BillStatusEnum>>());
            Property(p => p.CompletionTime);
            Property(p => p.TotalReceivableFee);
            Property(p => p.BusinessID);
            Property(p => p.BillWeight);
            Property(p => p.OrderCounts);
            Property(p => p.MonthPayTime, m => m.Column("PayOffMonth"));
            Property(p => p.IsAddMonthPayOff);
            Property(p => p.StorageFeeReason);
            Property(p => p.LoadTime);
            Property(p => p.DeliveryTime);
        }
    }

    //public class TaxDetailMap : ClassMapping<TaxDetail>
    //{
    //    public TaxDetailMap()
    //    {
    //        Table("TaxDetail");
    //        Id(p => p.ID, m => { m.Generator(Generators.Identity); });
    //        Property(p => p.ExpressNo);
    //        Property(p => p.OrderNO);
    //        Property(p => p.LoadBillNum);
    //        Property(p => p.TaxFee);
    //        Property(p => p.AffirmTime);
    //        Property(p => p.TaxBillNO);
    //        Property(p => p.InputTime);
    //        ManyToOne(p => p.CustomerInfoBy, m => m.Column("CusID"));
    //        Property(p => p.PayStatus, m => { m.Type<NHibernate.Type.EnumType<Core.PayStatus>>(); });
    //    }
    //}

    public class WayBillInComeMap : ClassMapping<WayBillInCome>
    {
        public WayBillInComeMap()
        {
            Table("WayBillInCome");
            Id(p => p.ID, m => m.Generator(Generators.Identity));
            ManyToOne(p => p.LoadBillBy, m => m.Column("LoadBillID"));
            ManyToOne(p => p.CustomerInfoBy, m => m.Column("CustomerID"));
            Property(p => p.PayStatus, m => { m.Type<NHibernate.Type.EnumType<Core.PayStatus>>(); });
            Property(p => p.WayBillType, m => { m.Type<NHibernate.Type.EnumType<WayBillType>>(); });
            Property(p => p.ExpressType, m => { m.Type<NHibernate.Type.EnumType<ExpressType>>(); });
            Property(p => p.ExpressNo);
            Property(p => p.OrderNo);
            Property(p => p.Weight);
            Property(p => p.ReceiverName);
            Property(p => p.ReceiverProvince);
            Property(p => p.ReceiverAddress);
            Property(p => p.SenderProvince);
            Property(p => p.PayTime);
            Property(p => p.ExpressFee);
            Property(p => p.OperateFee);
            Property(p => p.ReplaceWayBillCount);
            Property(p => p.DispatchAgainFee);
            Property(p => p.TaxFee);
            Property(p => p.CompensationFee);
            Property(p => p.ReturnedCargoFee);
            Property(p => p.CompanyID);
            Property(p => p.DeliveryID);
            Property(p => p.CreateTime);
        }
    }

    public class MonthPayOffMap : ClassMapping<MonthPayOff>
    {
        public MonthPayOffMap()
        {
            Table("MonthPayOff");
            Id(p => p.ID, m => m.Generator(Generators.Identity));
            Property(p => p.PayOffMonth);
            Property(p => p.Remark);
            Property(p => p.Status, m => m.Type<NHibernate.Type.EnumType<MonthlyBalanceStatus>>());
            Bag(p => p.Items, m => { m.Key(k => k.Column("MonthPayOffID")); m.Cascade(Cascade.All); }, rel => rel.OneToMany());
        }
    }

    public class MonthPayOffDetailMap : ClassMapping<MonthPayOffDetail>
    {
        public MonthPayOffDetailMap()
        {
            Table("MonthPayOffDetail");
            Id(p => p.ID, m => m.Generator(Generators.Identity));
            Property(p => p.PreTotalCostFee);
            Property(p => p.TotalCostFee);
            Property(p => p.PreInComeFee);
            Property(p => p.InComeFee);
            Property(p => p.TotalMargin);
            Property(p => p.CreateTime);
            ManyToOne(p => p.LoadBillBy, m => { m.Column("LoadBillID"); m.Cascade(Cascade.All); });
            ManyToOne(p => p.MonthPayOffBy, m => { m.Column("MonthPayOffID"); m.Cascade(Cascade.All); });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Core.CostFlow;

namespace Data.CostFlow
{
    /// <summary>
    /// 运单成本映射
    /// </summary>
    public class WayBillCostMap : ClassMapping<WayBillCost>
    {
        public WayBillCostMap()
        {
            Table("WayBillCost");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.ExpressNo);
            Property(p => p.BatchNO);
            Property(p => p.PostingTime);
            Property(p => p.Weight);
            Property(p => p.ProcessingFee);
            Property(p => p.WayBillFee);
            Property(p => p.SendAddress);
            Property(p => p.Product);

            //新加2015-06-30
            Property(p => p.ReconcileDate);
            Property(p => p.PayStatus);
            Property(p => p.PayTime);
        }
    }
}

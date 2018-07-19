using Core.Customer;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Customer
{
    /// <summary>
    /// 客户信息映射
    /// </summary>
    public class CustomerInfoMap : ClassMapping<CustomerInfo>
    {
        public CustomerInfoMap()
        {
            Table("CustomerInfo");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.CusCode, m => m.Column("Cus_Code"));
            Property(p => p.CusName, m => m.Column("Cus_Name"));
            Property(p => p.BusssinessType, m => { m.Type<NHibernate.Type.EnumType<Busssiness>>(); });
            Property(p => p.Phone);
            Property(p => p.Tel);
            Property(p => p.Email);
            Property(p => p.Fax);
            Property(p => p.Country);
            Property(p => p.Address);
            Property(p => p.CompanyName);
            Property(p => p.Balance);
            Property(p => p.CreditAmount);
            Property(p => p.Status, x => x.Type<NHibernate.Type.EnumType<CustomerStatus>>());
            //Bag Column传的值是从表的外键
            Bag(p => p.ExpressCurInfoBy, m => { m.Key(k => k.Column("CusID")); m.Cascade(Cascade.All); }, rel => rel.OneToMany());
        }
    }

    /// <summary>
    /// 快件客户信息
    /// </summary>
    public class ExpressCurInfoMap : ClassMapping<ExpressCurInfo>
    {
        public ExpressCurInfoMap()
        {
            Table("ExpressCurInfo");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.DeliveryID);
            Property(p => p.DeliveryName);
            Property(p => p.AccountName);
            Property(p => p.CreateTime);
            Property(p => p.IsMatch);
            //Bag Column传的值是从表的外键
            ManyToOne(p => p.CustomerInfoBy, m => { m.Column("CusID"); });
        }
    }
}

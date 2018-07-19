using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.Reconciliation;
using Core.Reconciliation.Repositories;
using ProjectBase.Utils.Entitles;
using Core.CostFlow;
using NHibernate;

namespace Data.Reconciliation
{
    public class WayBillExceptionRepository : AbstractNHibernateDao<WayBillException, int>, IWayBillExceptionRepository
    {
        public override IPageOfList<WayBillException> GetByFilter(ParameterFilter filter)
        {
            string column = @"e.ID AS ID,
w.ID AS IsInputCost,
w.PostingTime AS PostingTime, 
e.ExpressNo AS ExpressNo,  
e.LoadBillNO AS LoadBillNum, 
w.Weight AS Weight,       
c.Cus_Name AS CusName,		 
(CASE WHEN w.ExpressNo IS NULL THEN a.PreExpressFee ELSE w.WayBillFee END) AS WayBillFee,  
(CASE WHEN w.ExpressNo IS NULL THEN a.PreOperateFee ELSE w.ProcessingFee END) AS ProcessingFee,
a.ExpressFee AS ExpressFee,  
a.OperateFee AS OperateFee, 
w.PayStatus AS CostStatus,    
a.PayStatus AS InComeStatus,  
a.ExpressFee - w.WayBillFee AS WayBillProfit,
e.ExceptionMsg,e.WayBillCostID ";
            string sql = @" FROM ExpressNoExceptionDetail e
INNER JOIN WayBillCost w ON e.WayBillCostID = w.ID AND e.Status=0
LEFT JOIN WayBillInCome a  ON e.ExpressNo = a.ExpressNo
LEFT JOIN CustomerInfo c ON a.CustomerID = c.ID 
where 1=1";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            string strSQL = "SELECT COUNT(e.ID) as Count " + sql;
            var countQuery = NHibernateSession.CreateSQLQuery(strSQL);
            var query = NHibernateSession.CreateSQLQuery(string.Format("SELECT {0} {1} {2}", column, sql, filter.GetOrderString()));

            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
                query.SetParameter(key, paras[key]);
            }

            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            var Count = countQuery.List<long>()[0];
            long totalCounts = Count;

            var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(WayBillException))).SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<WayBillException>().ToList();
            return new WRPageOfList<WayBillException>(list, pageIndex, pageSize, totalCounts, new StatModel());
        }

        /// <summary>
        /// 运单对账页面统计异常数据数量
        /// </summary>
        /// <returns></returns>
        public long GetExceptionCount(ParameterFilter strWhere)
        {
            string sql = @"SELECT COUNT(e.ID) as Count FROM ExpressNoExceptionDetail e
INNER JOIN WayBillCost w ON e.WayBillCostID = w.ID AND e.Status=0
LEFT JOIN WayBillInCome a  ON e.ExpressNo = a.ExpressNo
LEFT JOIN CustomerInfo c ON a.CustomerID = c.ID 
where 1=1" + strWhere.ToHql();
            var countQuery = NHibernateSession.CreateSQLQuery(sql);
            var paras = strWhere.GetParameters();
            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
            }
            long result = countQuery.UniqueResult<long>();
            return result;
        }

        public IList<WayBillException> GetByExpressNo(string expressNo)
        {
            var query = NHibernateSession.CreateQuery("FROM WayBillException WHERE ExpressNo =:ExpressNo");
            query.SetParameter("ExpressNo", expressNo);
            return query.List<WayBillException>();
        }


        /// <summary>
        /// 删除异常数据和成本数据
        /// </summary>
        /// <param name="wayBillCostID"></param>
        /// <returns></returns>
        public int DeleteException(int wayBillCostID, WayBillException Excmodel, WayBillCost Costmodel)
        {
            using (ITransaction trans = NHibernateSession.BeginTransaction())
            {
                try
                {
                    var a = 0;
                    //删除异常数据
                    var query = NHibernateSession.CreateSQLQuery("DELETE FROM ExpressNoExceptionDetail WHERE WayBillCostID = :WayBillCostID");
                    query.SetParameter("WayBillCostID", wayBillCostID);
                    query.ExecuteUpdate();

                    //删除成本数据
                    query = NHibernateSession.CreateSQLQuery("DELETE FROM WayBillCost WHERE ID = :ID");
                    query.SetParameter("ID", wayBillCostID);
                    query.ExecuteUpdate();

                    //添加删除异常记录             
                    query = NHibernateSession.CreateSQLQuery("insert into  DelExpressNoExceptionDetail (ExpressNo,ExceptionType,ExceptionMsg,CreateTime,LoadBillNO,Status,WayBillCostID,PostingTime,DelTime) values (:ExpressNo,:ExceptionType,:ExceptionMsg,:CreateTime,:LoadBillNO,:Status,:WayBillCostID,:PostingTime,:DelTime)");
                    query.SetParameter("ExpressNo", Excmodel.ExpressNo);                  //运单号
                    query.SetParameter("ExceptionType", Excmodel.ExceptionType);          //异常类型
                    query.SetParameter("ExceptionMsg", Excmodel.ExceptionMsg);            //异常说明
                    query.SetParameter("CreateTime", DateTime.Now);                       //创建时间        
                    query.SetParameter("LoadBillNO", Excmodel.LoadBillNum);               //提单号
                    query.SetParameter("Status", 0);                                      //状态
                    query.SetParameter("WayBillCostID", Excmodel.WayBillCostID);          //异常成本ID
                    query.SetParameter("PostingTime", Excmodel.PostingTime);              //收寄日期
                    query.SetParameter("DelTime", DateTime.Now);                          //删除时间
                    query.ExecuteUpdate();

                    //添加删除成本记录               
                    query = NHibernateSession.CreateSQLQuery("insert into  DelWayBillCostEx (ExpressNo,WayBillFee,Weight,CreateTime,SendAddress,ProcessingFee,Product,BatchNO,PostingTime,ReconcileDate,PayStatus,PayTime,DelTime) values (:ExpressNo,:WayBillFee,:Weight,:CreateTime,:SendAddress,:ProcessingFee,:Product,:BatchNO,:PostingTime,:ReconcileDate,:PayStatus,:PayTime,:DelTime)");
                    query.SetParameter("ExpressNo", Costmodel.ExpressNo);               //邮件号
                    query.SetParameter("WayBillFee", Costmodel.WayBillFee);             //邮资
                    query.SetParameter("Weight", Costmodel.Weight);                     //重量
                    query.SetParameter("CreateTime", DateTime.Now);                     //创建时间        
                    query.SetParameter("SendAddress", Costmodel.SendAddress);           //寄达地
                    query.SetParameter("ProcessingFee", Costmodel.ProcessingFee);       //邮件处理费
                    query.SetParameter("Product", Costmodel.Product);                   //产品

                    query.SetParameter("BatchNO", Costmodel.BatchNO);                   //批次
                    query.SetParameter("PostingTime", Costmodel.PostingTime);           //收寄日期

                    query.SetParameter("ReconcileDate", Costmodel.ReconcileDate);       //结算月份
                    query.SetParameter("PayStatus", Costmodel.PayStatus);               //结算状态
                    query.SetParameter("PayTime", Costmodel.PayTime);                   //结算时间
                    query.SetParameter("DelTime", DateTime.Now);                        //删除时间
                    query.ExecuteUpdate();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return 1;
        }

        /// <summary>
        /// 修改后 删除异常记录
        /// </summary>
        /// <param name="costinfo"></param>
        /// <returns></returns>
        public int UpdateException(WayBillCost costinfo, WayBillException model, int WayBillCostID)
        {
            using (ITransaction trans = NHibernateSession.BeginTransaction())
            {
                try
                {
                    int a = 0;
                    //删除异常表记录
                    var query = NHibernateSession.CreateSQLQuery("DELETE FROM ExpressNoExceptionDetail WHERE WayBillCostID = :WayBillCostID ");
                    query.SetParameter("WayBillCostID", WayBillCostID);
                    query.ExecuteUpdate();

                    //修改运单成本信息
                    query = NHibernateSession.CreateSQLQuery("Update WayBillCost set Product=:Product,WayBillFee=:WayBillFee ,ProcessingFee=:ProcessingFee  WHERE ID = :ID ");
                    query.SetParameter("ID", WayBillCostID);
                    query.SetParameter("Product", costinfo.Product);                           //快递类型
                    query.SetParameter("WayBillFee", costinfo.WayBillFee);                     //运费
                    query.SetParameter("ProcessingFee", costinfo.ProcessingFee);               //操作费
                    query.ExecuteUpdate();

                    //添加 已删除运单异常记录                
                    query = NHibernateSession.CreateSQLQuery("insert into  DelExpressNoExceptionDetail (ExpressNo,ExceptionType,ExceptionMsg,CreateTime,LoadBillNO,Status,WayBillCostID,PostingTime,DelTime) values (:ExpressNo,:ExceptionType,:ExceptionMsg,:CreateTime,:LoadBillNO,:Status,:WayBillCostID,:PostingTime,:DelTime)");
                    query.SetParameter("ExpressNo", model.ExpressNo);                  //运单号
                    query.SetParameter("ExceptionType", model.ExceptionType);          //异常类型
                    query.SetParameter("ExceptionMsg", model.ExceptionMsg);            //异常说明
                    query.SetParameter("CreateTime", DateTime.Now);                    //创建时间        
                    query.SetParameter("LoadBillNO", model.LoadBillNum);               //提单号
                    query.SetParameter("Status", 0);                                   //状态
                    query.SetParameter("WayBillCostID", model.WayBillCostID);          //异常成本ID
                    query.SetParameter("PostingTime", model.PostingTime);              //收寄日期
                    query.SetParameter("DelTime", DateTime.Now);                       //删除时间
                    query.ExecuteUpdate();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return 1;
        }


        public IList<WayBillException> GetExceByID(string exceID)
        {
            //--客户名称 --收寄日期  --运单号 --提单号 --重量    --邮政邮政  -- 邮政操作费 --邮政其他费用（暂时没有） --成本状态  --客户运费 --客户操作费 --客户其他费用（暂时没有） 收入状态 异常原因
            string column = @"a.ExceptionType ,
b.PostingTime,b.ExpressNo,b.BatchNO as LoadBillNum,b.Weight,b.WayBillFee,b.ProcessingFee,b.PayStatus as CostStatus,
c.ExpressFee,c.OperateFee,c.PayStatus as InComeStatus,
d.Cus_Name as CusName ";
            string sql = @" FROM ExpressNoExceptionDetail a
 left join WayBillCost b on a.WayBillCostID=b.ID
 left JOIN WayBillInCome c on b.ExpressNo=c.ExpressNo
 left JOIN CustomerInfo d on c.CustomerID=d.ID
  where a.ID=" + exceID + " ";
            var query = NHibernateSession.CreateSQLQuery(string.Format("select {0} {1}  ", column, sql));
            //StatModel model = new StatModel();
            //var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(WayBillReconciliation))).SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<WayBillReconciliation>().ToList();
            //query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(WayBillReconciliation))).List<WayBillReconciliation>().ToList();
            var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(WayBillException))).List<WayBillException>().ToList();
            return list;
        }

        /// <summary>
        /// 根据运单成本ID运单成本详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>    
        public IList<WayBillCost> GetCostByExpByID(string CostID)
        {
            string column = @"ID,ExpressNo,BatchNO,PostingTime,Weight,SendAddress,ProcessingFee,WayBillFee,Product";
            string sql = @" from WayBillCost where ID='" + CostID + "'";
            var query = NHibernateSession.CreateSQLQuery(string.Format("select {0} {1}  ", column, sql)).AddEntity(typeof(WayBillCost));
            //StatModel model = new StatModel();
            return query.List<WayBillCost>();
        }
    }
}

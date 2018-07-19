using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.UserModule.Repositories;
using System.ComponentModel.DataAnnotations;
using ProjectBase.Utils.Entitles;

namespace Core.UserModule
{
    public class UserAccount : DomainObject<UserAccount, int, IUserAccountRepository>
    {
        #region property
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号不能为空", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "账号最大长度为50个字符")]
        public virtual string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// 
        [Required(ErrorMessage = "密码不能为空", AllowEmptyStrings = false)]
        public virtual string PassWord { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string RealName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public virtual bool IsAvailable { get; set; }
        #endregion

        #region common method
        public static IPageOfList<UserAccount> GetByFilter(ParameterFilter filter)
        {
            return Dao.GetByFilter(filter);
        }

        public static UserAccount GetByAccount(string account)
        {
            return Dao.GetByAccount(account);
        }

        public static string GeneratePassword(string password, DateTime createTime)
        {
            return (password + createTime.Date.Ticks).MD5(16);
        }

        /// <summary>
        /// 用户登陆验证
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="passWord">密码</param>
        /// <param name="message">返回信息</param>
        /// <returns>验证结果</returns>
        public static bool LoginValid(string account, string passWord, out string message)
        {
            var user = GetByAccount(account);
            if (user == null)
            {
                message = string.Format("用户名:“{0}”不存在", account);
                return false;
            }
            else if (!user.IsVaildPassword(passWord) && user.IsAvailable)
            {
                message = "密码错误";
                return false;
            }
            else if (!user.IsAvailable)
            {
                message = "账号已停用";
                return false;
            }
            else
            {
                message = string.Empty;
                return true;
            }
        }
        #endregion

        #region entity method
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="message">返回信息</param>
        /// <returns></returns>
        public virtual bool Insert(out string message)
        {
            if (GetByAccount(Account) != null)
            {
                message = string.Format("账号:“{0}”已经被注册", Account);
                return false;
            }
            message = ValidationMessage;
            if (!string.IsNullOrEmpty(message))
            {
                return false;
            }
            CreateTime = DateTime.Now;
            PassWord = GeneratePassword(PassWord, CreateTime);
            IsAvailable = true;
            this.Saving += iphone6_PriceChanged;
            this.Save();
            message = string.Format("账号:“{0}”注册成功", Account);
            return true;
        }

        static void iphone6_PriceChanged(object sender, SavedEventArgs e)
        {
            UserAccount v123 = sender as UserAccount;
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="oldPassWord">旧密码</param>
        /// <param name="newPassWord">新密码</param>
        /// <param name="message">返回提示</param>
        /// <returns>操作结果</returns>
        public virtual bool UpdatePassword(string oldPassWord, string newPassWord, out string message)
        {
            if (!IsVaildPassword(oldPassWord))
            {
                message = "输入旧密码错误";
                return false;
            }
            else
            {
                CreateTime = DateTime.Now;
                PassWord = GeneratePassword(newPassWord, CreateTime);
                this.Update();
                message = "修改密码成功";
                return true;
            }
        }

        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="password">验证密码</param>
        /// <returns>验证结果</returns>
        private bool IsVaildPassword(string password)
        {
            return GeneratePassword(password, CreateTime) == PassWord;
        }
        #endregion
    }
}

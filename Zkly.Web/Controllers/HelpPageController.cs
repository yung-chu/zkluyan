using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zkly.Web.Controllers
{
    public class HelpPageController : HelpBaseController
    {
        // GET: HelpPage
        //注册
        public ActionResult Register()
        {
            IndexName = "账户管理";
            PageName = "账户注册";
            return View();
        }

        //登录
        public ActionResult Login()
        {
            IndexName = "账户管理";
            PageName = "账户登录";
            return View();
        }

        //忘记密码和/重置密码
        public ActionResult ResetPassword()
        {
            IndexName = "账户管理";
            PageName = "忘记密码";
            return View();
        }

        //验证邮箱
        public ActionResult VerifyMailbox()
        {
            IndexName = "账户管理";
            PageName = "验证邮箱";
            return View();
        }

        //验证手机
        public ActionResult VerifyPhone()
        {
            IndexName = "账户管理";
            PageName = "验证手机";
            return View();
        }

        //更改手机
        public ActionResult UpdPhone()
        {
            IndexName = "账户管理";
            PageName = "更改手机";
            return View();
        }

        //更改密码
        public ActionResult UpdPWD()
        {
            IndexName = "账户管理";
            PageName = "更改密码";
            return View();
        }

        //提交一审
        public ActionResult FirstInstance()
        {
            IndexName = "申请投资";
            PageName = "提交一审资料";
            return View();
        }

        //修改一审
        public ActionResult UpdFirstInstance()
        {
            IndexName = "申请投资";
            PageName = "修改一审资料";
            return View();
        }

        //提交二审
        public ActionResult TowFirstInstance()
        {
            IndexName = "申请投资";
            PageName = "提交二审资料";
            return View();
        }

        //修改二审
        public ActionResult UpdTowFirstInstance()
        {
            IndexName = "申请投资";
            PageName = "修改二审资料";
            return View();
        }

        //  查看投资申请的状态Application 
        public ActionResult LookInvestment()
        {
            IndexName = "申请投资";
            PageName = "查看投资申请状态";
            return View();
        }

        //  申请投资流程
        public ActionResult ApplicationInvestment()
        {
            IndexName = "申请投资";
            PageName = "申请投资流程";
            return View();
        }

        //  上传业务路演
        public ActionResult UploadRoadShow()
        {
            IndexName = "申请投资";
            PageName = "上传业务路演";
            return View();
        }
       
        //  借贷流程
        public ActionResult LendingProcess()
        {
            IndexName = "申请贷款";
            PageName = "申请贷款流程";
            return View();
        }

        //借贷申请
        public ActionResult Loan()
        {
            IndexName = "申请贷款";
            PageName = "提交申请贷款资料";
            return View();
        }

        //修改借贷
        public ActionResult UpdLoan()
        {
            IndexName = "申请贷款";
            PageName = "修改申请贷款资料";
            return View();
        }

        //  偏好设计
        public ActionResult PreferenceSetting()
        {
            IndexName = "投资人偏好设置";
            PageName = "设置投资人偏好";
            return View();
        }

        //  修改偏好
        public ActionResult UpdPreferenceSetting()
        {
            IndexName = "投资人偏好设置";
            PageName = "修改投资人偏好";
            return View();
        }

        //  业务路演 路演厅
        public ActionResult BusinessRoadShow()
        {
            IndexName = "路演厅";
            PageName = "查看业务路演";
            return View();
        }

        //  资本路演
        public ActionResult CapitalRoadShow()
        {
            IndexName = "路演厅";
            PageName = "查看资本路演";
            return View();
        }

        //  联系QQ客服
        public ActionResult QQ()
        {
            IndexName = "联系客服";
            PageName = "客服";
            return View();
        }

        //  帮助中心主页
        public ActionResult Index()
        {
            return View();
        }
         
        // 账户管理
        public ActionResult AccountManagement()
        {
            PageName = "账户管理";
            return View();
        }

        // 申请投资
        public ActionResult ApplicationForInvestment()
        {
            PageName = "申请投资";
            return View();
        }

        // 申请借贷
        public ActionResult ApplicationForLoan()
        {
            PageName = "申请借贷";
            return View();
        }

        // 投资人偏好设置 
        public ActionResult InvestorPreference()
        {
            PageName = "投资人偏好设置";
            return View();
        }

        // 路演厅 
        public ActionResult RoadShowRoom()
        {
            PageName = "路演厅";
            return View();
        }

        // 联系客服 
        public ActionResult ContactCustomerService()
        {
            PageName = "联系客服";
            return View();
        }
    }
}
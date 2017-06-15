using System.Collections.Generic;
using System.Web.Mvc;
using VShuttle.Model;
using VShuttle.Model.ViewModel;
using VShuttle.Models;
using VShuttle.Repository;

namespace VShuttle.Controllers
{
    public class HomeController : Controller
    {
        UserInfoRepository userInfoRepository = new UserInfoRepository();
        public ActionResult Index()
        {
            RouteUserinfo routeUserinfo = new RouteUserinfo();          
            
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Location", Value = "0" });
            li.Add(new SelectListItem { Text = "Bhaktapur", Value = "1" });
            li.Add(new SelectListItem { Text = "Koteshwor", Value = "2" });
            li.Add(new SelectListItem { Text = "Baneshwor", Value = "3" });

            var routes = new List<Routes>()
            {
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor,Bhaktapur" },
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor,Bhaktapur" },
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor,Bhaktapur" },
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor,Bhaktapur" },
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor,Bhaktapur" }
            };

            ViewData["location"] = li;

            routeUserinfo.Routes = routes;
            routeUserinfo.UserInfo = new UserInfo();
          
            return View(routeUserinfo);
        }

        [HttpPost]
        public ActionResult Index(UserInfo userInfo)
        {
            //userInfoRepository.Save(userInfo);
            bool status = userInfo.Id > 0 ? userInfoRepository.Update(userInfo) : userInfoRepository.Save(userInfo);
            return View(); 
        }

        public ActionResult FindAll(AjaxModel ajaxGrid)
        {
            List<UserInfo> userInfo = new List<UserInfo>() {
                new UserInfo{ Id=1, Name="Abc", Location="Loc",   SubLocation="subloc", Date="2017/06/12"},
                new UserInfo{ Id=2, Name="Abc1", Location="Loc1", SubLocation="subloc1", Date="2017/06/12"},
                new UserInfo{ Id=3, Name="Abc2", Location="Loc2", SubLocation="subloc2", Date="2017/06/12"},
                new UserInfo{ Id=4, Name="Abc3", Location="Loc3", SubLocation="subloc3", Date="2017/06/12"}
            };

            AjaxGridResult result = new AjaxGridResult();
            result.Data = userInfo;
            result.pageNumber = ajaxGrid.pageNumber;
            result.RowCount = ajaxGrid.rowNumber;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Form(int id)
        {

            List<UserInfo> userInfo = new List<UserInfo>() {
                new UserInfo{ Id=1, Name="Abc", Location="1", SubLocation="subloc", Date="2017/06/12"},
                new UserInfo{ Id=2, Name="Abc1", Location="2", SubLocation="subloc1", Date="2017/06/12"},
                new UserInfo{ Id=3, Name="Abc2", Location="3", SubLocation="subloc2", Date="2017/06/12"},
                new UserInfo{ Id=4, Name="Abc3", Location="4", SubLocation="subloc3", Date="2017/06/12"}
            };

            UserInfo userinfo = new UserInfo();
            userinfo = userInfo[id - 1];

            var li = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Select Location", Value = "0" },
                new SelectListItem { Text = "Bhaktapur", Value = "1" },
                new SelectListItem { Text = "Koteshwor", Value = "2" },
                new SelectListItem { Text = "Baneshwor", Value = "3" }
            };
           
            ViewData["location"] = li;
            return View(userinfo);
        }

        public ActionResult Delete(int id) {
            return RedirectToAction("Index");
        }

        public ActionResult FindAllTotal(AjaxModel ajaxGrid)
        {
            var totalUsers = new List<TotalUsers>()
            {
                new TotalUsers { Location="Baneshwor", TotalCount=5 },
                new TotalUsers { Location="Koteshwor", TotalCount=2 },
                new TotalUsers { Location="Bhaktapur", TotalCount=10 },
                new TotalUsers { Location="Kalanki", TotalCount=8 },
            };

            AjaxGridResult result = new AjaxGridResult();
            result.Data = totalUsers;
            result.pageNumber = ajaxGrid.pageNumber;
            result.RowCount = ajaxGrid.rowNumber;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
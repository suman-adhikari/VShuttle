using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VShuttle.Model;
using VShuttle.Model.ViewModel;
using VShuttle.Models;
using VShuttle.Repository;

namespace VShuttle.Controllers
{
    public class AdminController : Controller
    {
        RoutesRepository routeRepository = new RoutesRepository();
        // [AuthorizeLoginUser]
        public ActionResult Index()
        {
            //var routes = routeRepository.FindAll();

            var routes = new List<Routes>()
            {
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor,Bhaktapur" },
                new Routes { Id=1, RouteLocations="Office,Koteshwor1,Baneshwor,Bhaktapur" },
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor1,Bhaktapur" },
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor,Bhaktapur1" },
                new Routes { Id=1, RouteLocations="Office,Koteshwor,Baneshwor,Bhaktapur" }
            };

            LocationRoute locationRoute = new LocationRoute();
            locationRoute.Route = routes;
            return View(locationRoute);
        }

        public ActionResult FindAll(AjaxModel ajaxGrid)
        {
            List<Locations> locationlist = new List<Locations>() {
                new Locations { Id=1,Location="Baneshwor"},
                new Locations { Id=2,Location="Koteshwor"},
                new Locations { Id=3,Location="Bhaktapur"}
            };

            AjaxGridResult result = new AjaxGridResult();
            result.Data = locationlist;
            result.pageNumber = ajaxGrid.pageNumber;
            result.RowCount = ajaxGrid.rowNumber;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Form(int id) {
            return View();
        }

        public ActionResult Delete(int id)
        {
            return View("Index");
        }

        public ActionResult UpdateRoute(FormCollection formdata)
        {
            int id = 0;
            foreach (string route in formdata)
            {
                id++;
                routeRepository.UpdateRoutes(id, formdata[route]);
            }          
            return View();
        }
     
    }
}
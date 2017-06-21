using System.Collections.Generic;
using System.Web.Mvc;
using VShuttle.Model;
using VShuttle.Model.ViewModel;
using VShuttle.Models;
using VShuttle.Repository;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Drawing;
using System;
using System.Runtime.InteropServices;

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

        public ActionResult FindAll(int offset, int rowNumber, string sortExpression, string sortOrder, int pageNumber, string Name="")
        {
            List<UserInfo> userInfo = new List<UserInfo>() {
                new UserInfo{ Id=1, UserId=1, Name="Abc", Location="Loc",   SubLocation="subloc", Date="2017/06/12"},
                new UserInfo{ Id=2, UserId=2, Name="Abc1", Location="Loc1", SubLocation="subloc1", Date="2017/06/12"},
                new UserInfo{ Id=3, UserId=3, Name="Abc2", Location="Loc2", SubLocation="subloc2", Date="2017/06/12"},
                new UserInfo{ Id=4, UserId=4, Name="Abc3", Location="Loc3", SubLocation="subloc3", Date="2017/06/12"}
            };


            AjaxGridResult result = new AjaxGridResult();
            result.Data = userInfo;
            result.pageNumber = pageNumber;
            result.RowCount = rowNumber;

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

        public ActionResult ExportToExcel()
        {
            
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkBook;
            Excel.Worksheet excelWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            excelWorkBook = excelApp.Workbooks.Add(misValue);
            Excel.Range range;

            string location = DateTime.Now.ToLongTimeString().Replace(":", "").Replace(" ", "") + ".xlsx";
            excelWorkBook.SaveAs(@"E:\"+location+"");

            Excel.Style headerStyle = excelWorkBook.Styles.Add("NewStyle");
            headerStyle.Font.Size = 10;
            excelWorkBook.InactiveListBorderVisible = true;
            headerStyle.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerStyle.Font.Color = ColorTranslator.ToOle(Color.White);
            headerStyle.Interior.Color =ColorTranslator.ToOle(Color.Gray);
            headerStyle.Interior.Pattern = Excel.XlPattern.xlPatternSolid;

            excelWorkSheet = excelWorkBook.Sheets.Add();
            range = excelWorkSheet.get_Range("A1", "G1");
            range.Style = "NewStyle";
            range.Columns.AutoFit();

            range.EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.EntireColumn.VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;

            for (int i = 3; i <= 6; i++)
            {
                excelWorkSheet.Columns[i].ColumnWidth = 15;             
            }
           
            var ds = userInfoRepository.GetData();
            string previousValue = "";

            foreach (DataTable table in ds.Tables)
            {              

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                      excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }
                
                int count = 1;
                
                for (int j = 0; j < table.Rows.Count; j++)
                {                 
                    for (int k = 0; k < table.Columns.Count; k++)
                    {        
                        string newValue = table.Rows[j].ItemArray[k].ToString();                      
                         if (k+1==4)

                        {                         
                            if(newValue == previousValue){
                                count++;                             
                                Excel.Range r = excelWorkSheet.Range[excelWorkSheet.Cells[j + 2 - 1, k + 1], excelWorkSheet.Cells[j + 2, k + 1]];
                                Excel.Range total = excelWorkSheet.Range[excelWorkSheet.Cells[j + 2 - 1, k + 2], excelWorkSheet.Cells[j + 2, k + 2]];
                                r.Merge(Type.Missing);
                                total.Merge(Type.Missing);
                                total.Value = "replace";
                                if (j== table.Rows.Count-1)
                                   excelWorkSheet.Cells.Replace("replace", count);
                                
                            }
                            else
                            {                                                            
                                excelWorkSheet.Cells.Replace("replace", count);
                                excelWorkSheet.Cells.Replace("", 1);
                                //excelWorkSheet.Cells[NoResetCount, 7] =  count;                                                                                                                             
                                count = 1;
                            }
                            
                            previousValue = table.Rows[j].ItemArray[k].ToString();
                        }
                        if (j== table.Rows.Count-1)
                        {
                            excelWorkSheet.Cells.Replace("", count);
                           
                            Excel.Range finalTotalLeft = excelWorkSheet.Range[excelWorkSheet.Cells[j + 3, 1], excelWorkSheet.Cells[j + 3, 4]];
                            Excel.Range finalTotalRight = excelWorkSheet.Range[excelWorkSheet.Cells[j + 3, 5], excelWorkSheet.Cells[j + 3, 7]];
                            finalTotalLeft.Merge(Type.Missing);
                            finalTotalRight.Merge(Type.Missing);
                            finalTotalLeft.Value = "Total";
                            finalTotalRight.Value = j + 1;

                            finalTotalLeft.Style = "NewStyle";
                            finalTotalRight.Style = "NewStyle";
                        }
                        excelWorkSheet.Cells[j + 2, k + 1] = newValue;
                       
                    }                   
                }
            }

            excelWorkBook.Save();
            excelWorkBook.Close(true,misValue,misValue);
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);

            return null;
        }
    }
}
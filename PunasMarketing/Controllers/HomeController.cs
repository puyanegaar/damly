using PunasMarketing.Helpers.Filters;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;

using PunasMarketing.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;
using PunasMarketing.ViewModels.Chart;

namespace PunasMarketing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChequeRepository _chequeRepo;
        private readonly ChequeStatusRepository _chequeStatusRepo;
        private readonly FiscalRepository _fiscalPeriodRepo;
        private readonly PersonnelRepository _personnelRepo;
        private readonly FactorRepository _factorRepo;
        private readonly ProductRepository _productRepo;


        public HomeController(ChequeRepository chequeRepo, ChequeStatusRepository chequeStatusRepo, FiscalRepository fiscalPeriodRepo, PersonnelRepository personnelRepo, FactorRepository factorRepo, ProductRepository productRepo)
        {
            _chequeRepo = chequeRepo;
            _chequeStatusRepo = chequeStatusRepo;
            _fiscalPeriodRepo = fiscalPeriodRepo;
            _personnelRepo = personnelRepo;
            _factorRepo = factorRepo;
            _productRepo = productRepo;
        }


        [CoustomAuthrize]
        public ActionResult Index()
        {
            var dbPendingPayCheques = _chequeRepo.GetPendingCheques(false);
            var dbPendingReceiveCheques = _chequeRepo.GetPendingCheques(true);

            #region SalesChart
            short fiscalId = Fiscal.GetFiscalId();

            var allSales =
                _factorRepo.Where(a =>
                        a.FactorTypeId == (byte)FactorType.Foroosh && a.IsDeleted == false && a.IsPreFactor == false && a.PeriodId == fiscalId)
                    .ToList();

            var farvardin = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 01).Sum(a => a.FinalPrice);
            var ordibehesht = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 02).Sum(a => a.FinalPrice);
            var khordad = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 03).Sum(a => a.FinalPrice);
            var tir = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 04).Sum(a => a.FinalPrice);
            var mordad = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 05).Sum(a => a.FinalPrice);
            var shahrivar = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 06).Sum(a => a.FinalPrice);
            var mehr = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 07).Sum(a => a.FinalPrice);
            var aban = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 08).Sum(a => a.FinalPrice);
            var azar = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 09).Sum(a => a.FinalPrice);
            var dey = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 10).Sum(a => a.FinalPrice);
            var bahman = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 11).Sum(a => a.FinalPrice);
            var esfand = allSales.Where(a => a.CreatedDate.ToPersianDateTime().Month == 12).Sum(a => a.FinalPrice);






            var chartData = new List<ChartViewModel>();

            chartData.Add(new ChartViewModel() { Name = "فروردین", Value = farvardin });
            chartData.Add(new ChartViewModel() { Name = "اردیبهشت", Value = ordibehesht });
            chartData.Add(new ChartViewModel() { Name = "خرداد", Value = khordad });
            chartData.Add(new ChartViewModel() { Name = "تیر", Value = tir });
            chartData.Add(new ChartViewModel() { Name = "مرداد", Value = mordad });
            chartData.Add(new ChartViewModel() { Name = "شهریور", Value = shahrivar });
            chartData.Add(new ChartViewModel() { Name = "مهر", Value = mehr });
            chartData.Add(new ChartViewModel() { Name = "آبان", Value = aban });
            chartData.Add(new ChartViewModel() { Name = "آذر", Value = azar });
            chartData.Add(new ChartViewModel() { Name = "دی", Value = dey });
            chartData.Add(new ChartViewModel() { Name = "بهمن", Value = bahman });
            chartData.Add(new ChartViewModel() { Name = "اسفند", Value = esfand });

            #endregion

            #region BuyChart
          
            var allBuys =
                _factorRepo.Where(a =>
                    a.FactorTypeId == (byte)FactorType.Kharid && a.IsPreFactor == false && a.PeriodId == fiscalId)
                    .ToList();


            var bfarvardin = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 01).Sum(a => a.FinalPrice);
            var bordibehesht = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 02).Sum(a => a.FinalPrice);
            var bkhordad = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 03).Sum(a => a.FinalPrice);
            var btir = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 04).Sum(a => a.FinalPrice);
            var bmordad = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 05).Sum(a => a.FinalPrice);
            var bshahrivar = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 06).Sum(a => a.FinalPrice);
            var bmehr = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 07).Sum(a => a.FinalPrice);
            var baban = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 08).Sum(a => a.FinalPrice);
            var bazar = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 09).Sum(a => a.FinalPrice);
            var bdey = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 10).Sum(a => a.FinalPrice);
            var bbahman = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 11).Sum(a => a.FinalPrice);
            var besfand = allBuys.Where(a => a.CreatedDate.ToPersianDateTime().Month == 12).Sum(a => a.FinalPrice);






            var buyChartData = new List<BuyChartViewModel>();

            buyChartData.Add(new BuyChartViewModel() { Name = "فروردین", Value = bfarvardin });
            buyChartData.Add(new BuyChartViewModel() { Name = "اردیبهشت", Value = bordibehesht });
            buyChartData.Add(new BuyChartViewModel() { Name = "خرداد", Value = bkhordad });
            buyChartData.Add(new BuyChartViewModel() { Name = "تیر", Value = btir });
            buyChartData.Add(new BuyChartViewModel() { Name = "مرداد", Value = bmordad });
            buyChartData.Add(new BuyChartViewModel() { Name = "شهریور", Value = bshahrivar });
            buyChartData.Add(new BuyChartViewModel() { Name = "مهر", Value = bmehr });
            buyChartData.Add(new BuyChartViewModel() { Name = "آبان", Value = baban });
            buyChartData.Add(new BuyChartViewModel() { Name = "آذر", Value = bazar });
            buyChartData.Add(new BuyChartViewModel() { Name = "دی", Value = bdey });
            buyChartData.Add(new BuyChartViewModel() { Name = "بهمن", Value = bbahman });
            buyChartData.Add(new BuyChartViewModel() { Name = "اسفند", Value = besfand });

            #endregion



            #region Infos

            var selesPrice = allSales.Sum(a => a.TotalPrice);
            var buyFactors = _factorRepo.Where(a =>
                a.IsBuy && a.IsDeleted == false && a.IsPreFactor == false && a.PeriodId == fiscalId);
            decimal buyPrice = 0;
            if (buyFactors.Any())
            {
                buyPrice = buyFactors.Sum(a => a.TotalPrice);
            }

            var personnelCount = _personnelRepo.Where(a => a.FireDate == null).Count();
            var productCount = _productRepo.Where(a => a.IsSellable).Count();





            InfoViewModel infos = new InfoViewModel()
            {
                BuyPrice = buyPrice,
                PersonnelCount = personnelCount,
                ProductCount = productCount,
                SellPrice = selesPrice

            };

            #endregion



          

            HomeViewModel viewModel = new HomeViewModel
            {
                PendingPayCheques = GetChequesFromPendingCheques(dbPendingPayCheques),
                PendingReceiveCheques = GetChequesFromPendingCheques(dbPendingReceiveCheques),
                ChartViewModels = chartData,
                BuyChartViewModel = buyChartData,
                Info = infos

            };

            return View(viewModel);
        }

        private List<Cheque> GetChequesFromPendingCheques(ObjectResult<GetPendingCheques_Result> dbPeningCheques)
        {
            List<Cheque> cheques = new List<Cheque>();

            foreach (var item in dbPeningCheques)
            {
                if (item.ChequeId != null
                    && item.Amount != null
                    && item.DueDate != null
                    && item.StatusId != null)
                {
                    cheques.Add(new Cheque
                    {
                        Id = item.ChequeId.Value,
                        Amount = item.Amount.Value,
                        DueDate = item.DueDate.Value,
                        StatusId = item.StatusId.Value,
                        CounterPartyName = item.CounterParyName,
                        StatusName = _chequeStatusRepo.Find(item.StatusId.Value).Name
                    });
                }
            }

            return cheques;
        }


        #region NavBar
        [ChildActionOnly]
        public ActionResult NavBar()
        {
            NavbarViewModel model = new NavbarViewModel();
            model.Fiscal = _fiscalPeriodRepo.Select();
            return PartialView("~/Views/Shared/_NavBar.cshtml", model);
        }

        [ChildActionOnly]
        public ActionResult GetLogedUserInfo()
        {
            LoggedUserInfo model = new LoggedUserInfo();
            int id = int.Parse(User.Identity.Name);
            var user = _personnelRepo.Find(id);
            model.UserName = user.FirstName + " " + user.LastName;
            model.Image = user.ImageName;
            return PartialView("_LoggedUserInfo", model);
        }
        #endregion
    }
}
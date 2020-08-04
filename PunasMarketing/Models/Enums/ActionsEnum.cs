using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.Enums
{
    public enum ActionsEnum
    {
        #region Factors

        AddFactor = 1,
        UpdateFactor = 2,
        ShowFactors = 3,
        DeleteFactor = 4,

        #endregion

        #region Invoices

        AddInvoice = 5,
        UpdateInvoice = 6,
        DeleteInvoice = 7,
        ShowInvoices = 38,

        #endregion

        #region Bank Accounts

        ShowBankAccounts = 8,
        AddBankAccount = 9,
        UpdateBankAccount = 10,
        DeleteBankAccount = 11,

        #endregion

        #region Banks

        ShowBanks = 12,
        AddBank = 13,
        UpdateBank = 14,
        DeleteBank = 15,

        #endregion

        #region CashDesks

        ShowCashDesks = 16,
        AddCashDesk = 17,
        UpdateCashDesk = 18,
        DeleteCashDesk = 19,

        #endregion

        #region Cheques

        ShowCheques = 20,
        ChangeStatus = 21,
        DeleteCheque = 115,

        #endregion

        #region Customer Categories

        ShowCustomerCategories = 22,
        AddCustomerCategory = 23,
        UpdateCustomerCategory = 24,
        DeleteCustomerCategory = 25,

        #endregion

        #region Customers

        ShowCustomers = 26,
        AddCustomer = 27,
        UpdateCustomer = 28,
        DeleteCustomer = 29,
        CustomerManagement = 30,

        #endregion

        #region Fiscal Periods

        FiscalSettings = 31,
        CloseFiscalPeriod = 32,

        #endregion

        #region Inventory Checkings

        ShowInventoryCheckings = 33,
        AddInventoryChecking = 34,
        UpdateInventoryChecking = 35,
        AddTadilInvoice = 36,
        DeleteInventoryChecking = 37,

        #endregion

        #region Job Titles

        ShowJobTitles = 39,
        AddJobTitle = 40,
        UpdateJobTitle = 41,
        DeleteJobTitle = 42,

        #endregion

        #region Marketers

        ShowMarketers = 43,
        AddMarketer = 44,
        UpdateMarketer = 45,
        MarketerManagement = 46,
        DeleteMarketer = 47,

        #endregion

        #region Offers

        ShowOffers = 48,
        AddOffer = 49,
        UpdateOffer = 50,
        DeleteOffer = 51,

        #endregion

        #region Orders

        ShowOrders = 52,
        VerifyUnverifyOrder = 53,
        AddFactorForOrder = 54,
        DeleteOrder = 55,

        #endregion

        #region Other Tafsilies

        ShowTafsilies = 56,
        AddTafsili = 57,
        UpdateTafsili = 58,
        DeleteTafsili = 59,

        #endregion

        #region Personnels

        ShowPersonnels = 60,
        AddPersonnel = 61,
        UpdatePersonnel = 62,
        PersonnelManagement = 63,
        DeletePersonnel = 64,

        #endregion

        #region Price Types

        ShowPriceTypes = 65,
        AddPriceType = 66,
        UpdatePriceType = 67,
        DeletePriceType = 68,

        #endregion

        #region Product Categories

        ShowProductCategories = 69,
        AddProductCategory = 70,
        UpdateProductCategory = 71,
        DeleteProductCategory = 72,

        #endregion


        #region Products

        ShowProducts = 73,
        AddProduct = 74,
        UpdateProduct = 75,
        ProductManagement = 76,
        DeleteProduct = 77,

        #endregion

        #region Regions

        ShowRegions = 78,
        AddRegion = 79,
        UpdateRegion = 80,
        DeleteRegion = 81,

        #endregion

        #region Financial Reports

        ShowJournalReport = 82,
        ShowJournalTotalAccountsReport = 83,
        LedgerReport = 84,

        #endregion

        #region Sanads

        ShowAsnad = 85,
        ShowAsnadDasti = 86,
        AddSanadDasti = 87,
        UpdateSanadDasti = 88,
        DeleteSanadDasti = 89,

        #endregion

        #region Sarfasls

        ShowSarfasls = 90,
        AddSarfasl = 91,
        UpdateSarfasl = 92,
        DeleteSarfasl = 93,

        #endregion

        #region Sections

        ShowSections = 94,
        AddSection = 95,
        UpdateSection = 96,
        DeleteSection = 97,

        #endregion

        #region Suppliers

        ShowSuppliers = 98,
        AddSupplier = 99,
        UpdateSupplier = 100,
        DeleteSupplier = 101,
        SupplierManagement = 102,

        #endregion

        #region Transactions

        ShowTransactions = 103,
        AddTransaction = 104,
        UpdateTransaction = 105,
        DeleteTransaction = 106,

        #endregion

        #region Units

        ShowUnits = 107,
        AddUnit = 108,
        UpdateUnit = 109,
        DeleteUnit = 110,

        #endregion

        #region Warehouses

        ShowWarehouses = 111,
        AddWarehouse = 112,
        UpdateWarehouse = 113,
        DeleteWarehouse = 114,
        CustmorFinancial=115

        #endregion
    }
}
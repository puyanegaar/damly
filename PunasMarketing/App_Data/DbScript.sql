USE [master]
GO
/****** Object:  Database [PunasMarketingDB]    Script Date: 1/7/2019 10:14:29 AM ******/
CREATE DATABASE [PunasMarketingDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PunasMarketingDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\PunasMarketingDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PunasMarketingDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\PunasMarketingDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [PunasMarketingDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PunasMarketingDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PunasMarketingDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PunasMarketingDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PunasMarketingDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PunasMarketingDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PunasMarketingDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET RECOVERY FULL 
GO
ALTER DATABASE [PunasMarketingDB] SET  MULTI_USER 
GO
ALTER DATABASE [PunasMarketingDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PunasMarketingDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PunasMarketingDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PunasMarketingDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PunasMarketingDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PunasMarketingDB', N'ON'
GO
ALTER DATABASE [PunasMarketingDB] SET QUERY_STORE = OFF
GO
USE [PunasMarketingDB]
GO
/****** Object:  Table [dbo].[Actions]    Script Date: 1/7/2019 10:14:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actions](
	[Id] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankAccounts]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankAccounts](
	[Id] [int] NOT NULL,
	[BankId] [tinyint] NOT NULL,
	[Iban] [varchar](26) NULL,
	[AccountNum] [varchar](15) NOT NULL,
	[Owner] [nvarchar](200) NOT NULL,
	[Branch] [nvarchar](100) NOT NULL,
	[BranchCode] [varchar](10) NULL,
	[HasPos] [bit] NOT NULL,
	[IsDisable] [bit] NOT NULL,
	[CurrentBalance] [money] NOT NULL,
	[InitialBalance] [money] NOT NULL,
 CONSTRAINT [PK_BankAccounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankBalances]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankBalances](
	[Id] [int] NOT NULL,
	[BankAccountId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[TransactionId] [int] NOT NULL,
	[BeforeUpdateBalance] [money] NOT NULL,
	[AfterUpdateBalance] [money] NOT NULL,
 CONSTRAINT [PK_BankBalances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Banks]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banks](
	[Id] [tinyint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Banks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChequeBooks]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChequeBooks](
	[Id] [int] NOT NULL,
	[BankAccountId] [int] NOT NULL,
	[LeavesCount] [tinyint] NOT NULL,
	[RemainintLeavesCount] [tinyint] NOT NULL,
	[FirstLeaf] [varchar](20) NOT NULL,
	[LastLeaf] [varchar](20) NOT NULL,
 CONSTRAINT [PK_ChequeBooks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[StateId] [int] NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CostCategories]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CostCategories](
	[Id] [tinyint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CostCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Costs]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Costs](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CostCategoryId] [tinyint] NOT NULL,
 CONSTRAINT [PK_Costs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FactorItems]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FactorItems](
	[Id] [int] NOT NULL,
	[FactorId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[MainUnitCount] [float] NULL,
	[SubUnitCount] [float] NULL,
	[UnitsRate] [float] NULL,
 CONSTRAINT [PK_FactorItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Factors]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factors](
	[Id] [int] NOT NULL,
	[RefId] [int] NULL,
	[FactorCode] [nvarchar](50) NULL,
	[Supplier_CustomerId] [int] NOT NULL,
	[SubmitDate] [datetime] NOT NULL,
	[FactorDate] [datetime] NULL,
	[Type] [int] NOT NULL,
	[TotalPrice] [money] NOT NULL,
	[FirstDiscount] [int] NULL,
	[Source_Destination] [int] NULL,
	[FinalPrice] [money] NOT NULL,
	[Deductions] [int] NULL,
	[AddedValue] [int] NULL,
	[Additions] [int] NULL,
	[UserId] [int] NOT NULL,
	[SalesExpertId] [int] NULL,
	[SalesExpertCommission] [int] NULL,
 CONSTRAINT [PK_Factors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceItems]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceItems](
	[Id] [int] NOT NULL,
	[InvoiceId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[MainUnitCount] [float] NOT NULL,
	[SubUnitCount] [float] NULL,
	[UintsRate] [float] NULL,
 CONSTRAINT [PK_InvoiceItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[Id] [int] NOT NULL,
	[SourceId] [int] NULL,
	[DestinationId] [int] NULL,
	[Date] [datetime] NOT NULL,
	[Type] [int] NOT NULL,
	[IsConfirmed] [bit] NOT NULL,
	[FactorNum] [nvarchar](50) NULL,
	[Description] [nvarchar](100) NULL,
 CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobTitles]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobTitles](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SectionId] [int] NOT NULL,
 CONSTRAINT [PK_JobTitles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[Id] [int] NOT NULL,
	[PayTypeId] [smallint] NOT NULL,
	[UserId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[SubmitDate] [datetime] NOT NULL,
	[SettleDate] [datetime] NULL,
	[IssueTracking] [nvarchar](50) NULL,
	[Amount] [money] NOT NULL,
	[IsDeposit] [bit] NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayTypes]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayTypes](
	[Id] [tinyint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_PayTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personnels]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personnels](
	[Id] [int] NOT NULL,
	[PersonnelCode] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FatherName] [nvarchar](50) NOT NULL,
	[CodeMelli] [varchar](10) NOT NULL,
	[IsMale] [bit] NOT NULL,
	[IsMarried] [bit] NOT NULL,
	[Mobile] [varchar](15) NOT NULL,
	[Tell] [varchar](15) NULL,
	[BirthPlaceId] [int] NOT NULL,
	[BirthDate] [date] NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Image] [varchar](50) NULL,
	[HireDate] [date] NOT NULL,
	[FireDate] [date] NULL,
	[AcademicDegree] [smallint] NOT NULL,
	[JobTitleId] [int] NOT NULL,
	[IsGettingSalary] [bit] NOT NULL,
	[Description] [nvarchar](300) NULL,
 CONSTRAINT [PK_Personnels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriceTypes]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceTypes](
	[Id] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PriceTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategories]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategories](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
 CONSTRAINT [PK_Categorise] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductPriceLists]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductPriceLists](
	[Id] [int] NOT NULL,
	[PriceTypeId] [smallint] NOT NULL,
	[Price] [money] NOT NULL,
	[ProductId] [int] NOT NULL,
	[IsVisible] [bit] NOT NULL,
 CONSTRAINT [PK_ProductPriceLists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] NOT NULL,
	[ProductCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MainUnitId] [smallint] NOT NULL,
	[SubUnitId] [smallint] NULL,
	[UnitRate] [int] NULL,
	[ProductCategoryId] [int] NOT NULL,
	[Inventory] [int] NOT NULL,
	[ImageName] [varchar](50) NULL,
	[IsAvailable] [bit] NOT NULL,
	[IsSellable] [bit] NOT NULL,
	[OrderPoint] [int] NULL,
	[ProductionStatus] [tinyint] NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecieveCheques]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecieveCheques](
	[Id] [int] NOT NULL,
	[PaymentId] [int] NOT NULL,
	[AccountId] [smallint] NOT NULL,
	[Type] [smallint] NOT NULL,
	[BankId] [smallint] NOT NULL,
	[SettleBankId] [smallint] NOT NULL,
	[DueDate] [date] NOT NULL,
	[ChequeNum] [nvarchar](50) NOT NULL,
	[AccountNum] [nvarchar](50) NOT NULL,
	[OwnerName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_RecieveCheques] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sections]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sections](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[States]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers_Customers]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers_Customers](
	[Id] [smallint] NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
	[Tell] [varchar](15) NOT NULL,
	[CityId] [int] NOT NULL,
	[Address] [nvarchar](70) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[Mobile] [varchar](15) NOT NULL,
	[IsSupplier] [bit] NOT NULL,
 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Units]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Units](
	[Id] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsMain] [bit] NOT NULL,
 CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAccesses]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccesses](
	[Id] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ActionId] [int] NOT NULL,
 CONSTRAINT [PK_UserAccesses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[PersonnelId] [int] NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[SaltCode] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[LastLogin] [datetime] NOT NULL,
 CONSTRAINT [PK_Users_1] PRIMARY KEY CLUSTERED 
(
	[PersonnelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WareHouses]    Script Date: 1/7/2019 10:14:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WareHouses](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[KeeperId] [int] NOT NULL,
	[Tell] [varchar](15) NOT NULL,
	[Address] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_WareHouses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Actions]  WITH CHECK ADD  CONSTRAINT [FK_Actions_Sections] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Sections] ([Id])
GO
ALTER TABLE [dbo].[Actions] CHECK CONSTRAINT [FK_Actions_Sections]
GO
ALTER TABLE [dbo].[BankAccounts]  WITH CHECK ADD  CONSTRAINT [FK_BankAccounts_Banks] FOREIGN KEY([BankId])
REFERENCES [dbo].[Banks] ([Id])
GO
ALTER TABLE [dbo].[BankAccounts] CHECK CONSTRAINT [FK_BankAccounts_Banks]
GO
ALTER TABLE [dbo].[BankBalances]  WITH CHECK ADD  CONSTRAINT [FK_BankBalances_BankAccounts] FOREIGN KEY([BankAccountId])
REFERENCES [dbo].[BankAccounts] ([Id])
GO
ALTER TABLE [dbo].[BankBalances] CHECK CONSTRAINT [FK_BankBalances_BankAccounts]
GO
ALTER TABLE [dbo].[BankBalances]  WITH CHECK ADD  CONSTRAINT [FK_BankBalances_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([PersonnelId])
GO
ALTER TABLE [dbo].[BankBalances] CHECK CONSTRAINT [FK_BankBalances_Users]
GO
ALTER TABLE [dbo].[ChequeBooks]  WITH CHECK ADD  CONSTRAINT [FK_ChequeBooks_BankAccounts] FOREIGN KEY([Id])
REFERENCES [dbo].[BankAccounts] ([Id])
GO
ALTER TABLE [dbo].[ChequeBooks] CHECK CONSTRAINT [FK_ChequeBooks_BankAccounts]
GO
ALTER TABLE [dbo].[Cities]  WITH CHECK ADD  CONSTRAINT [FK_Cities_States] FOREIGN KEY([StateId])
REFERENCES [dbo].[States] ([Id])
GO
ALTER TABLE [dbo].[Cities] CHECK CONSTRAINT [FK_Cities_States]
GO
ALTER TABLE [dbo].[Costs]  WITH CHECK ADD  CONSTRAINT [FK_Costs_CostCategories] FOREIGN KEY([CostCategoryId])
REFERENCES [dbo].[CostCategories] ([Id])
GO
ALTER TABLE [dbo].[Costs] CHECK CONSTRAINT [FK_Costs_CostCategories]
GO
ALTER TABLE [dbo].[JobTitles]  WITH CHECK ADD  CONSTRAINT [FK_JobTitles_Sections] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Sections] ([Id])
GO
ALTER TABLE [dbo].[JobTitles] CHECK CONSTRAINT [FK_JobTitles_Sections]
GO
ALTER TABLE [dbo].[Personnels]  WITH CHECK ADD  CONSTRAINT [FK_Personnels_Cities] FOREIGN KEY([BirthPlaceId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Personnels] CHECK CONSTRAINT [FK_Personnels_Cities]
GO
ALTER TABLE [dbo].[Personnels]  WITH CHECK ADD  CONSTRAINT [FK_Personnels_JobTitles] FOREIGN KEY([JobTitleId])
REFERENCES [dbo].[JobTitles] ([Id])
GO
ALTER TABLE [dbo].[Personnels] CHECK CONSTRAINT [FK_Personnels_JobTitles]
GO
ALTER TABLE [dbo].[ProductPriceLists]  WITH CHECK ADD  CONSTRAINT [FK_ProductPriceLists_PriceTypes] FOREIGN KEY([PriceTypeId])
REFERENCES [dbo].[PriceTypes] ([Id])
GO
ALTER TABLE [dbo].[ProductPriceLists] CHECK CONSTRAINT [FK_ProductPriceLists_PriceTypes]
GO
ALTER TABLE [dbo].[ProductPriceLists]  WITH CHECK ADD  CONSTRAINT [FK_ProductPriceLists_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[ProductPriceLists] CHECK CONSTRAINT [FK_ProductPriceLists_Products]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategories] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Units] FOREIGN KEY([MainUnitId])
REFERENCES [dbo].[Units] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Units]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Units1] FOREIGN KEY([SubUnitId])
REFERENCES [dbo].[Units] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Units1]
GO
ALTER TABLE [dbo].[Suppliers_Customers]  WITH CHECK ADD  CONSTRAINT [FK_Suppliers_Cities] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Suppliers_Customers] CHECK CONSTRAINT [FK_Suppliers_Cities]
GO
ALTER TABLE [dbo].[UserAccesses]  WITH CHECK ADD  CONSTRAINT [FK_UserAccesses_Actions] FOREIGN KEY([ActionId])
REFERENCES [dbo].[Actions] ([Id])
GO
ALTER TABLE [dbo].[UserAccesses] CHECK CONSTRAINT [FK_UserAccesses_Actions]
GO
ALTER TABLE [dbo].[UserAccesses]  WITH CHECK ADD  CONSTRAINT [FK_UserAccesses_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([PersonnelId])
GO
ALTER TABLE [dbo].[UserAccesses] CHECK CONSTRAINT [FK_UserAccesses_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Personnels] FOREIGN KEY([PersonnelId])
REFERENCES [dbo].[Personnels] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Personnels]
GO
ALTER TABLE [dbo].[WareHouses]  WITH CHECK ADD  CONSTRAINT [FK_WareHouses_Users] FOREIGN KEY([KeeperId])
REFERENCES [dbo].[Users] ([PersonnelId])
GO
ALTER TABLE [dbo].[WareHouses] CHECK CONSTRAINT [FK_WareHouses_Users]
GO
USE [master]
GO
ALTER DATABASE [PunasMarketingDB] SET  READ_WRITE 
GO

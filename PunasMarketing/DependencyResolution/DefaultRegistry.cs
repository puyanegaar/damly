// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using PunasMarketing.Models.Repositories;

namespace PunasMarketing.DependencyResolution
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
					scan.With(new ControllerConvention());
                });

            #region IOC

            For<ActionRepository>().Use<ActionRepository>().Transient();
            For<ProductCategoryRepository>().Use<ProductCategoryRepository>().Transient();
            For<JobTitleRepository>().Use<JobTitleRepository>().Transient();
            For<PriceTypeRepository>().Use<PriceTypeRepository>().Transient();
            For<SectionRepository>().Use<SectionRepository>().Transient();
            For<SupplierRepository>().Use<SupplierRepository>().Transient();
            For<UnitRepository>().Use<UnitRepository>().Transient();
            For<WarehouseRepository>().Use<WarehouseRepository>().Transient();
            For<ProductRepository>().Use<ProductRepository>().Transient();
            For<UserAccessRepository>().Use<UserAccessRepository>().Transient();
            For<PersonnelRepository>().Use<PersonnelRepository>().Transient();
            For<UserRepository>().Use<UserRepository>().Transient();
            For<ProductPriceListRepository>().Use<ProductPriceListRepository>().Transient();
            For<CityRepository>().Use<CityRepository>().Transient();
            For<BankRepository>().Use<BankRepository>().Transient();
            For<BankAccountRepository>().Use<BankAccountRepository>().Transient();
            For<ChequeRepository>().Use<ChequeRepository>().Transient();
            For<ProductionStatusRepository>().Use<ProductionStatusRepository>().Transient();
            For<ChequeStatusRepository>().Use<ChequeStatusRepository>().Transient();
            For<CashDeskRepository>().Use<CashDeskRepository>().Transient();
            For<TransactionRepository>().Use<TransactionRepository>().Transient();
            For<RegionRepository>().Use<RegionRepository>().Transient();
            For<ProvinceRepository>().Use<ProvinceRepository>().Transient();
            For<CustomerRepository>().Use<CustomerRepository>().Transient();
            For<FactorRepository>().Use<FactorRepository>().Transient();
            For<FactorItemRepository>().Use<FactorItemRepository>().Transient();
           

            #endregion
        }

        #endregion
    }
}
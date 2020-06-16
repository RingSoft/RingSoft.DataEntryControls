﻿using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System;

namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public class SalesEntryViewModel : DbMaintenanceViewModel<Orders>
    {
        public override TableDefinition<Orders> TableDefinition =>
            AppGlobals.LookupContext.Orders;

        private int _orderId;
        public int OrderId
        {
            get => _orderId;
            set
            {
                if (value == _orderId) 
                    return;

                _orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }

        private AutoFillSetup _customerAutoFillSetup;

        public AutoFillSetup CustomersAutoFillSetup
        {
            get => _customerAutoFillSetup;
            set
            {
                if (_customerAutoFillSetup == value)
                    return;

                _customerAutoFillSetup = value;
                OnPropertyChanged(nameof(CustomersAutoFillSetup), false);
            }
        }

        private AutoFillValue _customer;
        public AutoFillValue Customer
        {
            get => _customer;
            set
            {
                if (_customer == value)
                    return;

                _customer = value;
                _customerDirty = true;
                OnPropertyChanged(nameof(Customer));
            }
        }

        private string _companyName;

        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (_companyName == value)
                    return;

                _companyName = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private AutoFillSetup _employeeAutoFillSetup;
        public AutoFillSetup EmployeeAutoFillSetup
        {
            get => _employeeAutoFillSetup;
            set
            {
                if (_employeeAutoFillSetup == value)
                    return;

                _employeeAutoFillSetup = value;
                OnPropertyChanged(nameof(EmployeeAutoFillSetup), false);
            }
        }

        private AutoFillValue _employee;
        public AutoFillValue Employee
        {
            get => _employee;
            set
            {
                if (_employee == value)
                    return;

                _employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }

        private AutoFillSetup _shipViaAutoFillSetup;

        public AutoFillSetup ShipViaAutoFillSetup
        {
            get => _shipViaAutoFillSetup;
            set
            {
                if (_shipViaAutoFillSetup == value)
                    return;

                _shipViaAutoFillSetup = value;
                OnPropertyChanged(nameof(ShipViaAutoFillSetup), false);
            }
        }

        private AutoFillValue _shipVia;
        public AutoFillValue ShipVia
        {
            get => _shipVia;
            set
            {
                if (_shipVia == value)
                    return;

                _shipVia = value;
                OnPropertyChanged(nameof(ShipVia));
            }
        }

        private DateTime _requiredDate;
        public DateTime RequiredDate
        {
            get => _requiredDate;
            set
            {
                if (_requiredDate == value)
                    return;
                _requiredDate = value;
                OnPropertyChanged(nameof(RequiredDate));
            }
        }

        private DateTime _orderDate;
        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                if (_orderDate == value)
                    return;
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }

        private DateTime _shippedDate;

        public DateTime ShippedDate
        {
            get => _shippedDate;
            set
            {
                if (_shippedDate == value)
                    return;

                _shippedDate = value;
                OnPropertyChanged(nameof(ShippedDate));
            }
        }

        private SalesEntryDetailsGridManager _detailsGridManager;

        public SalesEntryDetailsGridManager DetailsGridManager
        {
            get => _detailsGridManager;
            set
            {
                if (_detailsGridManager == value)
                    return;

                _detailsGridManager = value;
                OnPropertyChanged(nameof(DetailsGridManager), false);
            }
        }

        private decimal? _freight;
        public decimal? Freight
        {
            get => _freight;
            set
            {
                if (_freight == value)
                    return;

                _freight = value;
                OnPropertyChanged(nameof(Freight));
                if (!ChangingEntity)
                    RefreshTotalControls();
            }
        }

        [CanBeNull] private string _shipName;
        [CanBeNull]
        public string ShipName
        {
            get => _shipName;
            set
            {
                if (_shipName == value)
                    return;

                _shipName = value;
                OnPropertyChanged(nameof(ShipName));
            }
        }

        [CanBeNull] private string _address;

        [CanBeNull]
        public string Address
        {
            get => _address;
            set
            {
                if (_address == value)
                    return;

                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        [CanBeNull] private string _city;

        [CanBeNull]
        public string City
        {
            get => _city;
            set
            {
                if (_city == value)
                    return;

                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        [CanBeNull] private string _region;
        [CanBeNull]
        public string Region
        {
            get => _region;
            set
            {
                if (_region == value)
                    return;

                _region = value;
                OnPropertyChanged(nameof(Region));
            }
        }

        [CanBeNull] private string _postalCode;

        [CanBeNull]
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode == value)
                    return;

                _postalCode = value;
                OnPropertyChanged(nameof(PostalCode));
            }
        }

        [CanBeNull] private string _country;

        [CanBeNull]
        public string Country
        {
            get => _country;
            set
            {
                if (_country == value)
                    return;

                _country = value;
                OnPropertyChanged(nameof(Country));
            }
        }

        private decimal _subTotal;

        public decimal SubTotal
        {
            get => _subTotal;
            set
            {
                if (_subTotal == value)
                    return;

                _subTotal = value;
                OnPropertyChanged(nameof(SubTotal));
            }
        }

        private decimal _totalDiscount;
        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set
            {
                if (_totalDiscount == value)
                    return;

                _totalDiscount = value;
                OnPropertyChanged(nameof(TotalDiscount));
            }
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set
            {
                if (_total == value)
                    return;

                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        protected override string FindButtonInitialSearchFor
        {
            get
            {
                if (OrderId == 0)
                    return string.Empty;
                return OrderId.ToString();
            }
        }

        private readonly DateTime _newDateTime = DateTime.Today;

        private bool _customerDirty;

        public SalesEntryViewModel()
        {
            _orderDate = _requiredDate = _shippedDate = _newDateTime;
        }

        protected override void Initialize()
        {
            CustomersAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CustomerId));
            EmployeeAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.EmployeeId));
            ShipViaAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ShipVia));
            DetailsGridManager = new SalesEntryDetailsGridManager(this);

            base.Initialize();
        }

        protected override void LoadFromEntity(Orders newEntity)
        {
            var order = AppGlobals.DbContextProcessor.GetOrder(newEntity.OrderId);
            OrderId = order.OrderId;
            Customer = new AutoFillValue(AppGlobals.LookupContext.Customers.GetPrimaryKeyValueFromEntity(order.Customer),
                order.CustomerId);

            if (order.Customer != null)
                CompanyName = order.Customer.CompanyName;

            var employeeName = string.Empty;
            if (order.Employee != null)
                employeeName = GetEmployeeAutoFillValueText(order.Employee);

            Employee = new AutoFillValue(AppGlobals.LookupContext.Employees.GetPrimaryKeyValueFromEntity(order.Employee),
                employeeName);

            if (order.RequiredDate == null)
                RequiredDate = _newDateTime;
            else
            {
                RequiredDate = (DateTime)order.RequiredDate;
            }

            if (order.OrderDate == null)
                OrderDate = _newDateTime;
            else
            {
                OrderDate = (DateTime)order.OrderDate;
            }

            if (order.ShippedDate == null)
                ShippedDate = _newDateTime;
            else
            {
                ShippedDate = (DateTime)order.ShippedDate;
            }

            var shipCompanyName = string.Empty;
            if (order.ShipVia != null)
                shipCompanyName = order.Shipper.CompanyName;

            ShipVia = new AutoFillValue(AppGlobals.LookupContext.Shippers.GetPrimaryKeyValueFromEntity(order.Shipper),
                shipCompanyName);

            Freight = order.Freight;
            ShipName = order.ShipName;
            Address = order.ShipAddress;
            City = order.ShipCity;
            Region = order.ShipRegion;
            PostalCode = order.ShipPostalCode;
            Country = order.ShipCountry;

            RefreshTotalControls();
            _customerDirty = false;
        }

        private void RefreshTotalControls()
        {
            decimal subTotal = 0;
            decimal totalDiscount = 0;
            decimal freight = 0;
            if (Freight != null)
                freight = (decimal) Freight;

            SubTotal = subTotal;
            TotalDiscount = totalDiscount;
            Total = (SubTotal - TotalDiscount) + freight;
        }
        
        private string GetEmployeeAutoFillValueText(Employees employee)
        {
            return $"{employee.FirstName} {employee.LastName}";
        }

        protected override Orders GetEntityData()
        {
            var order = new Orders();
            order.OrderId = OrderId;

            if (Customer != null && Customer.PrimaryKeyValue.ContainsValidData())
            {
                var customer = AppGlobals.LookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);
                order.CustomerId = customer.CustomerId;
            }

            if (Employee != null && Employee.PrimaryKeyValue.ContainsValidData())
            {
                var employee = AppGlobals.LookupContext.Employees.GetEntityFromPrimaryKeyValue(Employee.PrimaryKeyValue);
                order.EmployeeId = employee.EmployeeId;
            }

            if (ShipVia != null && ShipVia.PrimaryKeyValue.ContainsValidData())
            {
                var shipVia = AppGlobals.LookupContext.Shippers.GetEntityFromPrimaryKeyValue(ShipVia.PrimaryKeyValue);
                order.ShipVia = shipVia.ShipperId;
            }

            order.OrderDate = OrderDate;
            order.RequiredDate = RequiredDate;
            order.ShippedDate = ShippedDate;
            order.Freight = Freight;
            order.ShipName = ShipName;
            order.ShipAddress = Address;
            order.ShipCity = City;
            order.ShipRegion = Region;
            order.ShipPostalCode = PostalCode;
            order.ShipCountry = Country;

            return order;
        }

        protected override void ClearData()
        {
            OrderId = 0;
            Customer = Employee = ShipVia = null;
            OrderDate = RequiredDate = ShippedDate = _newDateTime;
            CompanyName = string.Empty;
            SubTotal = TotalDiscount = Total = 0;
            Freight = 0;
            ShipName = Address = City = Region = PostalCode = Country = null;

            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                var table = LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition;
                if (table == AppGlobals.LookupContext.Customers)
                {
                    var customer =
                        AppGlobals.LookupContext.Customers.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    customer =
                        AppGlobals.DbContextProcessor.GetCustomer(customer.CustomerId);
                    Customer = new AutoFillValue(LookupAddViewArgs.ParentWindowPrimaryKeyValue, customer.CustomerId);
                    CompanyName = customer.CompanyName;
                }
                else if (table == AppGlobals.LookupContext.Employees)
                {
                    if (AppGlobals.LookupContext.Employees != null)
                    {
                        var employee =
                            AppGlobals.LookupContext.Employees.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                                .ParentWindowPrimaryKeyValue);
                        employee =
                            AppGlobals.DbContextProcessor.GetEmployee(employee.EmployeeId);
                        Employee = new AutoFillValue(LookupAddViewArgs.ParentWindowPrimaryKeyValue,
                            GetEmployeeAutoFillValueText(employee));
                    }
                }
            }

            _customerDirty = false;
        }

        protected override AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.CustomerId))
                return Customer;

            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.EmployeeId))
                return Employee;

            if (fieldDefinition == TableDefinition.GetFieldDefinition(p => p.ShipVia))
                return ShipVia;

            return base.GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        protected override bool SaveEntity(Orders entity)
        {
            return AppGlobals.DbContextProcessor.SaveOrder(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DbContextProcessor.DeleteOrder(OrderId);
        }

        public void OnCustomerIdLostFocus()
        {
            if (_customerDirty)
            {
                if (Customer?.PrimaryKeyValue == null || !Customer.PrimaryKeyValue.ContainsValidData())
                {
                    CompanyName = string.Empty;
                }
                else
                {
                    var customer = AppGlobals.LookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);
                    customer =
                        AppGlobals.DbContextProcessor.GetCustomer(customer.CustomerId);

                    if (customer != null)
                    {
                        CompanyName = ShipName = customer.CompanyName;
                        Address = customer.Address;
                        City = customer.City;
                        Region = customer.Region;
                        PostalCode = customer.PostalCode;
                        Country = customer.Country;
                    }
                }
                _customerDirty = false;
            }
        }
    }
}
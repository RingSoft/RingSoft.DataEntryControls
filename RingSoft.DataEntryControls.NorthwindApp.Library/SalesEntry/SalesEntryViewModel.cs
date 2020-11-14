using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System;


namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public interface ISalesEntryMaintenanceView : IDbMaintenanceView
    {
        InvalidProductResult CorrectInvalidProduct(AutoFillValue invalidProductValue);

        bool ShowCommentEditor(GridMemoValue comment);

        void GridValidationFail();
    }

    public class SalesEntryViewModel : DbMaintenanceViewModel<Orders>
    {
        public override TableDefinition<Orders> TableDefinition =>
            AppGlobals.LookupContext.Orders;

        public ISalesEntryMaintenanceView SalesEntryView { get; private set; }

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

        private DateTime? _requiredDate;
        public DateTime? RequiredDate
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

        private DateTime? _orderDate;
        public DateTime? OrderDate
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

        private DateTime? _shippedDate;

        public DateTime? ShippedDate
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

        private decimal _freight;
        public decimal Freight
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

        private bool _scannerMode;

        public bool ScannerMode
        {
            get => _scannerMode;
            set
            {
                if (_scannerMode == value)
                    return;

                _scannerMode = value;
                OnPropertyChanged(nameof(ScannerMode), false);
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

        private bool _customerDirty;

        protected override void Initialize()
        {
            SalesEntryView = View as ISalesEntryMaintenanceView ??
                             throw new ArgumentException(
                                 $"ViewModel requires an {nameof(ISalesEntryMaintenanceView)} interface.");

            ScannerMode = AppGlobals.SalesEntryScannerMode;

            CustomersAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.CustomerId))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            EmployeeAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.EmployeeId))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            ShipViaAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.ShipVia))
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            DetailsGridManager = new SalesEntryDetailsGridManager(this);

            base.Initialize();
        }

        protected override Orders PopulatePrimaryKeyControls(Orders newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var order = AppGlobals.DbContextProcessor.GetOrder(newEntity.OrderId);
            OrderId = order.OrderId;

            return order;
        }

        protected override void LoadFromEntity(Orders entity)
        {
            Customer = new AutoFillValue(AppGlobals.LookupContext.Customers.GetPrimaryKeyValueFromEntity(entity.Customer),
                entity.CustomerId);

            if (entity.Customer != null)
                CompanyName = entity.Customer.CompanyName;
            else
                CompanyName = string.Empty;

            var employeeName = string.Empty;
            if (entity.Employee != null)
                employeeName = GetEmployeeAutoFillValueText(entity.Employee);

            Employee = new AutoFillValue(AppGlobals.LookupContext.Employees.GetPrimaryKeyValueFromEntity(entity.Employee),
                employeeName);

            RequiredDate = entity.RequiredDate;
            OrderDate = entity.OrderDate;
            ShippedDate = entity.ShippedDate;

            var shipCompanyName = string.Empty;
            if (entity.ShipVia != null)
                shipCompanyName = entity.Shipper.CompanyName;

            ShipVia = new AutoFillValue(AppGlobals.LookupContext.Shippers.GetPrimaryKeyValueFromEntity(entity.Shipper),
                shipCompanyName);

            if (entity.Freight != null) 
                Freight = (decimal) entity.Freight;
            ShipName = entity.ShipName;
            Address = entity.ShipAddress;
            City = entity.ShipCity;
            Region = entity.ShipRegion;
            PostalCode = entity.ShipPostalCode;
            Country = entity.ShipCountry;

            DetailsGridManager.LoadGrid(entity.OrderDetails);

            RefreshTotalControls();
            _customerDirty = false;
        }

        public void RefreshTotalControls()
        {
            decimal subTotal = 0;
            decimal totalDiscount = 0;

            foreach (var dataEntryGridRow in DetailsGridManager.Rows)
            {
                var salesEntryDetailsRow = (SalesEntryDetailsRow) dataEntryGridRow;
                switch (salesEntryDetailsRow.LineType)
                {
                    case SalesEntryDetailsLineTypes.Product:
                        if (salesEntryDetailsRow is SalesEntryDetailsProductRow productRow)
                        {
                            subTotal += productRow.ExtendedPrice;
                            totalDiscount += productRow.Discount;
                        }
                        break;
                    case SalesEntryDetailsLineTypes.NonInventoryCode:
                    case SalesEntryDetailsLineTypes.SpecialOrder:
                        if (salesEntryDetailsRow is SalesEntryDetailsValueRow valueRow)
                            subTotal += valueRow.ExtendedPrice;
                        break;
                    case SalesEntryDetailsLineTypes.Comment:
                    case SalesEntryDetailsLineTypes.NewRow:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            SubTotal = subTotal;
            TotalDiscount = totalDiscount;
            Total = (SubTotal - TotalDiscount) + Freight;
        }
        
        private string GetEmployeeAutoFillValueText(Employees employee)
        {
            return $"{employee.FirstName} {employee.LastName}";
        }

        protected override Orders GetEntityData()
        {
            var order = new Orders();
            order.OrderId = OrderId;

            if (Customer != null && Customer.PrimaryKeyValue.IsValid)
            {
                var customer = AppGlobals.LookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);
                order.CustomerId = customer.CustomerId;
            }

            if (Employee != null && Employee.PrimaryKeyValue.IsValid)
            {
                var employee = AppGlobals.LookupContext.Employees.GetEntityFromPrimaryKeyValue(Employee.PrimaryKeyValue);
                order.EmployeeId = employee.EmployeeId;
            }

            if (ShipVia != null && ShipVia.PrimaryKeyValue.IsValid)
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
            OrderDate = RequiredDate = ShippedDate = null;
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

            DetailsGridManager.SetupForNewRecord();
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

        protected override bool ValidateEntity(Orders entity)
        {
            if (!ValidateCustomer())
                return false;

            if (!base.ValidateEntity(entity))
                return false;

            if (!DetailsGridManager.ValidateGrid())
                return false;

            return true;
        }

        protected override bool SaveEntity(Orders entity)
        {
            var orderDetails = DetailsGridManager.GetEntityList();
            return AppGlobals.DbContextProcessor.SaveOrder(entity, orderDetails);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DbContextProcessor.DeleteOrder(OrderId);
        }

        public void OnCustomerIdLostFocus()
        {
            if (_customerDirty)
            {
                if (Customer?.PrimaryKeyValue == null || !Customer.PrimaryKeyValue.IsValid)
                {
                    CompanyName = string.Empty;
                }
                else
                {
                    var customer = AppGlobals.LookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);

                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
                    customer =
                        AppGlobals.DbContextProcessor.GetCustomer(customer.CustomerId);
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

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

        public bool ValidateCustomer()
        {
            if (Customer != null && !string.IsNullOrEmpty(Customer.Text) &&
                !Customer.PrimaryKeyValue.IsValid)
            {
                var message = "Invalid Customer!";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Validation Fail", RsMessageBoxIcons.Exclamation);
                return false;
            }

            return true;
        }
    }
}

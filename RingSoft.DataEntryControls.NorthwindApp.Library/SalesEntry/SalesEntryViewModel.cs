using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library.Model;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.DbMaintenance;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace RingSoft.DataEntryControls.NorthwindApp.Library.SalesEntry
{
    public interface ISalesEntryMaintenanceView : IDbMaintenanceView
    {
        InvalidProductResult CorrectInvalidProduct(AutoFillValue invalidProductValue);

        bool ShowCommentEditor(DataEntryGridMemoValue comment);
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

        private bool _detailsGridReadOnlyMode;

        public bool DetailsGridReadOnlyMode
        {
            get => _detailsGridReadOnlyMode;
            set
            {
                if (_detailsGridReadOnlyMode == value)
                    return;

                _detailsGridReadOnlyMode = value;
                OnPropertyChanged(nameof(DetailsGridReadOnlyMode));
            }
        }


        private double _freight;
        public double Freight
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

        private double _subTotal;

        public double SubTotal
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

        private double _totalDiscount;
        public double TotalDiscount
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

        private double _total;

        public double Total
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

        private string _notes;

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }


        public bool SetInitialFocusToGrid { get; internal set; }

        public int InitDetailId { get; internal set; } = -1;

        public UiCommand CustomerUiCommand { get; } = new UiCommand();
        public UiCommand EmployeeUiCommand { get; } = new UiCommand();
        public UiCommand ShipUiCommand { get; } = new UiCommand();

        protected override string FindButtonInitialSearchFor
        {
            get
            {
                if (MaintenanceMode == DbMaintenanceModes.AddMode)
                    return string.Empty;

                return OrderDate.ToShortDateString();
            }
        }

        internal NorthwindViewModelInput ViewModelInput { get; private set; }

        private bool _customerDirty;

        public SalesEntryViewModel()
        {
            TablesToDelete.Add(AppGlobals.LookupContext.OrderDetails);

            CustomerUiCommand.LostFocus += CustomerUiCommand_LostFocus;
        }

        private void CustomerUiCommand_LostFocus(object sender, UiLostFocusArgs e)
        {
            if (Customer != null && !Customer.Text.IsNullOrEmpty() && !Customer.IsValid())
            {
                var message = $"Customer {Customer.Text} was not found.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid Customer", RsMessageBoxIcons.Exclamation);
                e.ContinueFocusChange = false;
            }
            else
            {
                if (Customer.IsValid())
                {
                    OnCustomerIdLostFocus();
                }
            }
        }

        protected override void Initialize()
        {
            ShipUiCommand.Caption = "Testing";
            SalesEntryView = View as ISalesEntryMaintenanceView ??
                             throw new ArgumentException(
                                 $"ViewModel requires an {nameof(ISalesEntryMaintenanceView)} interface.");

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is NorthwindViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new NorthwindViewModelInput();
            }
            ViewModelInput.SalesEntryViewModels.Add(this);


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
            RegisterGrid(DetailsGridManager);

            base.Initialize();
        }

        protected override void PopulatePrimaryKeyControls(Orders newEntity, PrimaryKeyValue primaryKeyValue)
        {
            OrderId = newEntity.OrderId;

            ReadOnlyMode = ViewModelInput.SalesEntryViewModels.Any(a => a != this && a.OrderId == OrderId);
        }

        protected override void LoadFromEntity(Orders entity)
        {
            Customer = entity.Customer.GetAutoFillValue();
            CompanyName = entity.Customer.CompanyName;
            Employee = entity.Employee.GetAutoFillValue();

            RequiredDate = entity.RequiredDate;
            if (entity.OrderDate != null) 
                OrderDate = (DateTime) entity.OrderDate;
            else
            {
                OrderDate = DateTime.Today;
            }
            ShippedDate = entity.ShippedDate;

            ShipVia = entity.Shipper.GetAutoFillValue();

            if (entity.Freight != null) 
                Freight = (double) entity.Freight;
            ShipName = entity.ShipName;
            Address = entity.ShipAddress;
            City = entity.ShipCity;
            Region = entity.ShipRegion;
            PostalCode = entity.ShipPostalCode;
            Country = entity.ShipCountry;
            Notes = entity.Notes;

            RefreshTotalControls();
            _customerDirty = false;

            if (ReadOnlyMode)
            {
                ControlsGlobals.UserInterface.ShowMessageBox("This Sale is being modified in another window.", "Editing not allowed", RsMessageBoxIcons.Exclamation);
            }
        }

        public void RefreshTotalControls()
        {
            double subTotal = 0;
            double totalDiscount = 0;

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
        

        protected override Orders GetEntityData()
        {
            var order = new Orders();
            order.OrderId = OrderId;

            if (Customer != null && Customer.PrimaryKeyValue.IsValid())
            {
                var customer = AppGlobals.LookupContext.Customers.GetEntityFromPrimaryKeyValue(Customer.PrimaryKeyValue);
                order.CustomerId = customer.CustomerId;
            }

            if (Employee != null && Employee.PrimaryKeyValue.IsValid())
            {
                var employee = AppGlobals.LookupContext.Employees.GetEntityFromPrimaryKeyValue(Employee.PrimaryKeyValue);
                order.EmployeeId = employee.EmployeeId;
            }

            if (ShipVia != null && ShipVia.PrimaryKeyValue.IsValid())
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
            order.Notes = Notes;

            return order;
        }

        protected override void ClearData()
        {
            OrderId = 0;
            Customer = Employee = ShipVia = null;
            RequiredDate = ShippedDate = null;
            OrderDate = DateTime.Today;
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
                        Employee = employee.GetAutoFillValue();
                    }
                }
            }

            Notes = string.Empty;
            _customerDirty = false;
        }

        protected override bool ValidateEntity(Orders entity)
        {
            if (!ValidateCustomer())
            {
                return false;
            }

            if (!ValidateEmployee())
            {
                return false;
            }

            if (!ValidateShipVia())
            {
                return false;
            }

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
                if (Customer?.PrimaryKeyValue == null || !Customer.PrimaryKeyValue.IsValid())
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
            if (!Customer.IsValid())
            {
                var message = "Invalid Customer!";
                CustomerUiCommand.SetFocus();
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Validation Fail", RsMessageBoxIcons.Exclamation);
                return false;
            }

            return true;
        }

        public bool ValidateEmployee()
        {
            if (!Employee.IsValid())
            {
                var message = "Invalid Employee!";
                EmployeeUiCommand.SetFocus();
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Validation Fail", RsMessageBoxIcons.Exclamation);
                return false;
            }

            return true;
        }

        public bool ValidateShipVia()
        {
            if (!ShipVia.IsValid())
            {
                var message = "Invalid Ship Via!";
                ShipUiCommand.SetFocus();
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Validation Fail", RsMessageBoxIcons.Exclamation);
                return false;
            }

            return true;
        }

        protected override TableFilterDefinitionBase GetAddViewFilter()
        {
            if (LookupAddViewArgs.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.OrderDetails)
            {
                SetInitialFocusToGrid = true;
                var orderDetail =
                    AppGlobals.LookupContext.OrderDetails.GetEntityFromPrimaryKeyValue(LookupAddViewArgs.LookupData
                        .SelectedPrimaryKeyValue);

                InitDetailId = orderDetail.OrderDetailId;
                int filterProductId = 0;
                orderDetail =
                    AppGlobals.DbContextProcessor.GetOrderDetail(orderDetail.OrderId, orderDetail.OrderDetailId);

                if (orderDetail.ProductId != null) 
                    filterProductId = (int) orderDetail.ProductId;

                var sqlStringBuilder = new StringBuilder();
                var sqlGen = AppGlobals.LookupContext.DataProcessor.SqlGenerator;

                sqlStringBuilder.AppendLine(
                    $"{sqlGen.FormatSqlObject(AppGlobals.LookupContext.Orders.TableName)}.{sqlGen.FormatSqlObject(AppGlobals.LookupContext.Orders.GetFieldDefinition(p => p.OrderId).FieldName)} IN");

                sqlStringBuilder.AppendLine("(");

                var query = new SelectQuery(AppGlobals.LookupContext.OrderDetails.TableName);
                query.AddSelectColumn(AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.OrderId).FieldName);
                query.AddWhereItem(AppGlobals.LookupContext.OrderDetails.GetFieldDefinition(p => p.ProductId).FieldName,
                    Conditions.Equals, filterProductId);
                sqlStringBuilder.AppendLine(AppGlobals.LookupContext.DataProcessor.SqlGenerator.GenerateSelectStatement(query));

                sqlStringBuilder.AppendLine(")");

                var tableFilterDefinition = new TableFilterDefinition<Orders>(TableDefinition);
                var sql = sqlStringBuilder.ToString();
                tableFilterDefinition.AddFixedFilter("Order Details", null, "", sql);

                return tableFilterDefinition;
            }

            return base.GetAddViewFilter();
        }

        protected override PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            if (addViewPrimaryKeyValue.TableDefinition == AppGlobals.LookupContext.OrderDetails)
            {
                var orderDetail =
                    AppGlobals.LookupContext.OrderDetails.GetEntityFromPrimaryKeyValue(addViewPrimaryKeyValue);

                var order = new Orders { OrderId = orderDetail.OrderId };
                return TableDefinition.GetPrimaryKeyValueFromEntity(order);
            }

            return base.GetAddViewPrimaryKeyValue(addViewPrimaryKeyValue);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.SalesEntryViewModels.Remove(this);
        }

        protected override void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1, int numericFieldIndex = 1,
            int memoFieldIndex = 1)
        {
            if (MaintenanceMode == DbMaintenanceModes.EditMode)
            {
                var order = new Orders()
                {
                    OrderId = OrderId
                };
                var orderSetup = new AutoFillSetup(FindButtonLookupDefinition);
                printerSetupArgs.CodeAutoFillValue = orderSetup.GetAutoFillValueForIdValue(OrderId);
            }
            base.SetupPrinterArgs(printerSetupArgs, stringFieldIndex, numericFieldIndex, memoFieldIndex);
        }
    }
}

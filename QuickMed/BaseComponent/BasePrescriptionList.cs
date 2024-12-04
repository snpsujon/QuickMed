using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
	public class BasePrescriptionList : ComponentBase
	{
		[Inject]
		public IMixTemp _mix { get; set; }
		[Inject]
		public IPrescription _pres { get; set; }
		[Inject]
		public IJSRuntime JS { get; set; }

		public DotNetObjectReference<BasePrescriptionList> ObjectReference { get; private set; }
		public PrescriptionVM prescription = new();
		public IEnumerable<PrescriptionVM>? prescriptions { get; set; }
		public List<TblDXTemplate> Dxs = new List<TblDXTemplate>();
		public List<DrugMedicine> Brands = new List<DrugMedicine>();
		protected override async Task OnInitializedAsync()
		{
			ObjectReference = DotNetObjectReference.Create(this);
			Dxs = new();
			Dxs = await App.Database.GetTableRowsAsync<TblDXTemplate>("TblDXTemplate");
			Brands = new();
			Brands = await _mix.GetAllMedicine();
			prescriptions = await GetFilterData();
			await RefreshDataTable();

		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await RefreshDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
				await InitializeJS();
			}
		}
		public async Task<IEnumerable<PrescriptionVM>> GetFilterData(
	 string? mobile = null,
	 string? prescriptionCode = null,
	 string? dxName = null,
	 DateTime? startDate = null,
	 DateTime? endDate = null, string? brand = null)
		{
			// Set default values if necessary
			startDate ??= new DateTime(2024, 1, 1);  // Default to January 1, 2024 if null
			endDate ??= new DateTime(2024, 12, 31);  // Default to December 31, 2024 if null

			// Create filter parameters object
			var filterParams = new PrescriptionFilterParameters
			{
				Mobile = mobile,
				PrescriptionCode = prescriptionCode,
				DxTempId = dxName,
				StartDate = startDate,
				EndDate = endDate,
				BrandId = brand
			};

			return await _pres.GetAll(filterParams);
		}


		protected async Task InitializeJS()
		{
			await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
			await JS.InvokeVoidAsync("makeSelect2", false);
			await JS.InvokeVoidAsync("makeSelect2Custom", "select2C", "GetMedicines", 3);
			await JS.InvokeVoidAsync("setMaxDate");

		}
		[JSInvokable("GetMedicines")]
		public async Task<List<DrugMedicine>> LoadMedicines(string search)
		{
			Brands = await _mix.GetAllMedicine(search);
			var filtered = string.IsNullOrEmpty(search)
				? Brands
				: Brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

			return await Task.FromResult(filtered);
		}

		protected async Task OnClearClick()
		{
			try
			{
				//await JS.InvokeVoidAsync("ClearAllFields");
				prescriptions = await GetFilterData();
				await RefreshDataTable();

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
		protected async Task OnSearchClick()
		{
			try
			{
				//await JS.InvokeVoidAsync("getSearchInput");
				var searchValue = await JS.InvokeAsync<JsonElement>("getSearchInput");


				string? mobile = searchValue.GetProperty("mobile").GetString();
				string? prescriptionCode = searchValue.GetProperty("prescriptionCode").GetString();
				string? dxName = searchValue.GetProperty("dxName").GetString() == "0" ? null : searchValue.GetProperty("dxName").GetString();
				DateTime? startDate = (Convert.ToDateTime(searchValue.GetProperty("startDate").GetString() == "" ? null : searchValue.GetProperty("startDate").GetString())) == DateTime.MinValue ? null : Convert.ToDateTime(searchValue.GetProperty("startDate").GetString());

				DateTime? endDate = (Convert.ToDateTime(searchValue.GetProperty("endDate").GetString() == "" ? null : searchValue.GetProperty("endDate").GetString())) == DateTime.MinValue ? null : Convert.ToDateTime(searchValue.GetProperty("endDate").GetString());
				string? brand = searchValue.GetProperty("brand").GetString() == "0" ? null : searchValue.GetProperty("brand").GetString();

				prescriptions = await GetFilterData(mobile, prescriptionCode, dxName, startDate, endDate, brand);
				await RefreshDataTable();



			}
			catch (Exception ex)
			{

				throw;
			}
		}

		protected async Task RefreshDataTable()
		{
			// Ensure prescriptions is not null
			if (prescriptions == null || !prescriptions.Any())
			{
				// If null or empty, pass an empty array to the JavaScript function
				await JS.InvokeVoidAsync("makeDataTableQ", "datatable-prescriptionList", new string[0][]);
				await JS.InvokeVoidAsync("ClearTable", "datatable-prescriptionList");
				return;
			}

			var tableData = prescriptions.Select((pres, index) => new[]
			{
		(index + 1).ToString(), // Serial number starts from 1
        pres?.PrescriptionCode?.ToString() ?? string.Empty, // Safely access PrescriptionCode
        pres?.PrescriptionDate.HasValue == true
			? pres.PrescriptionDate.Value.ToString("yyyy-MM-dd") // Format the DateTime
            : string.Empty, // Handle null DateTime
        pres?.PatientName?.ToString() ?? string.Empty, // Safely access PatientName
        pres?.MobileNumber?.ToString() ?? string.Empty, // Safely access MobileNumber
        pres?.Address?.ToString() ?? string.Empty, // Safely access Address
        pres?.Dx?.ToString() ?? string.Empty, // Safely access Dx
        pres?.Plan?.ToString() ?? string.Empty, // Safely access Plan
        pres != null
			? $@"
                <div style='display: flex; justify-content: flex-end;'>
                     
                     <i class='dripicons-print btn btn-soft-warning dTRowActionBtn' data-id='{pres.Id}' data-method='OnPrintClick'></i>
                     <i class='dripicons-download btn btn-soft-info dTRowActionBtn' data-id='{pres.Id}' data-method='OnDownloadClick'></i>
                     <i class='dripicons-pencil btn btn-soft-primary dTRowActionBtn' data-id='{pres.Id}' data-method='OnEditClick'></i>
                     <i class='dripicons-trash btn btn-soft-danger dTRowActionBtn' data-id='{pres.Id}' data-method='OnDeleteClick'></i>
                </div>
              "
			: string.Empty // Handle null prescription object for actions
    }).ToArray();

			// Ensure the JavaScript function is called only if tableData is valid
			if (tableData != null)
			{
				await JS.InvokeVoidAsync("makeDataTableQ", "datatable-prescriptionList", tableData);
			}
		}

		protected async Task OnPreviewClick(PrescriptionVM data)
		{
			StateHasChanged(); // Re-render the component with the updated model
		}



		[JSInvokable("OnDeleteClick")]
		public async Task OnDeleteClick(string Id)
		{
			bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

			if (isConfirmed)
			{
				var isDeleted = await _pres.DeleteAsync(Id);
				if (isDeleted)
				{
					// Show success alert with red color
					await JS.InvokeVoidAsync("showAlert", "Delete Successful", "Record has been successfully deleted.", "success", "swal-danger");

					// Refresh the list after deletion
					await OnInitializedAsync();
					StateHasChanged();  // Update the UI after deletion
				}
				else
				{
					// Handle failure case and show an error alert if necessary
					Console.WriteLine("Failed to delete record from the database.");
				}
			}
		}



	}
}

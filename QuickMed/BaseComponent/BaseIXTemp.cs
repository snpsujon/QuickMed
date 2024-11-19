using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.Components.Pages.Templates;
using QuickMed.DB;
using QuickMed.Interface;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
    public class BaseIXTemp : ComponentBase
    {
        [Inject]
        public IIXTemp _ixTemp { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }



        public TblIXTemplate model = new();
        public IEnumerable<TblIXTemplate>? models { get; set; }
        public List<TblIXDetails> templateDetails { get; set; }


        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        await JS.InvokeVoidAsync("makeDataTable", "datatable-ixTemp");
        //        await JS.InvokeVoidAsync("setupEditableTable", "makeEditable_IxTemp", "but_add_IxTemp");
        //    }
        //}

        //protected override async Task OnInitializedAsync()
        //{
        //    //models = await _ixTemp.GetAsync(); // Load the initial data
        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
                await InitializeJS();
               
                
            }
        }

        protected async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
           
        }
        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setupEditableTable", "makeEditable_IxTemp", "but_add_IxTemp", false);
            await JS.InvokeVoidAsync("makeTableDragable", "makeEditable_IxTemp");
            await JS.InvokeVoidAsync("makeSelect2", false);
        }


        protected async Task OnSaveBtnClick()
        {
            var result = await JS.InvokeAsync<JsonElement>("GetIXTempData");
            if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
            {
                if (result.TryGetProperty("templateName", out JsonElement templateNameElement))
                {
                    var templateName = templateNameElement.GetString();
                    model = new()
                    {
                        Id = Guid.NewGuid(),
                        TemplateName = templateName
                    };
                    await _ixTemp.SaveAsync(model);

                }

                if (result.TryGetProperty("tempData", out JsonElement jsonelement))
                {
                    if (jsonelement.ValueKind == JsonValueKind.Array)
                    {
                        var ListData = jsonelement.EnumerateArray()
                            .Select(item => item.GetString())
                            .ToList();

                        if (ListData.Count() > 0)
                        {

                            templateDetails = new List<TblIXDetails>();
                            foreach (var item in ListData)
                            {
                                templateDetails.Add(new TblIXDetails
                                {
                                    Id = Guid.NewGuid(),
                                    TblIXTempMasterId = model.Id,
                                    Name = item
                                });
                            }
                            var saveDetails = await _ixTemp.SaveTemplateDetails(templateDetails);
                            await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("templateName not found.");
                }

                await InitializeDataTable();

                await OnInitializedAsync();

                StateHasChanged();


            }



        }

    }
}

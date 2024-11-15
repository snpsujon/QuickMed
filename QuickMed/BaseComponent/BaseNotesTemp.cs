using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.BaseComponent
{
    public class BaseNotesTemp : ComponentBase
    {
        [Inject]
        public INotesTemp _notes { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }


        public TblNotesTemplate model = new();
        public IEnumerable<TblNotesTemplate>? models { get; set; }

        protected override async Task OnInitializedAsync()
        {
            models = await _notes.GetAsync(); // Load the initial data
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
            }
        }

        protected async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        }
    }
}

﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Clinicas
{
    [Authorize(Roles = "Admin")]
    public partial class ClinicasPage
    {
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        public List<Clinica>? Clinicas { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            //var result = await repository.GetAsync<List<Clinica>>("/api/clinicas/DevuelveClinicas");

            var responseHppt = await repository.GetAsync<List<Clinica>>("/api/ApiAcler/DevuelveClinicas");
            if (responseHppt.Error)
            {
                var message = await responseHppt.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Clinicas = responseHppt.Response!;
        }
    }
}
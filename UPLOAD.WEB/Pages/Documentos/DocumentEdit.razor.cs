﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net;
using UPLOAD.SHARE.Entities;
using UPLOAD.WEB.Pages.Documentos;
using UPLOAD.WEB.Repositories;

namespace UPLOAD.WEB.Pages.Documentos
{
    [Authorize(Roles = "Admin")]
    public partial class DocumentEdit
    {
        private Image? image;

        /// para referencias la no navegacion  <summary>
        /// para poder cambiar la propiedad FormPostedSuccessfully

        private DocumentForm? documentForm;



        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService{ get; set; } = null!;


        ////pasame id como parametro para ver que Iamgen es
        /// // es
        [EditorRequired, Parameter] public int Id { get; set; }


        /// <summary>
        /// onparameterSe  como estoy pasando unparametro y depende del valor
        /// yo quiero estar seguro que el paramtro este seteado id
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            //$ es interpolacion c# para pasar como parametro
            var responseHttp = await repository.GetAsync<Image>($"/api/imagenes/{Id}");
            if (responseHttp.Error)
            {
                //si el usuario me cambio el pais por la qstring
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/document");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);

                }
            }

            //sino hubo errores y el pais es traido
            else
            {
                image = responseHttp.Response;
            }
        }

        ///cuadndo el usario dice que si va a cambiar
        private async Task EditAsync()
        {
            var responseHttp = await repository.PutAsync("/api/imagenes", image);
            //si hay error al actualiza lo pintamos
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            //
            //si grabo con exito lo mando a la pantalla de country
            Return();
            ///luego de mandarlo a la pantalla  pinto mensaje de cambio
            ///tostadita abajo y final informativo se usa tostadita
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios Guardados.");

        }

        private void Return()
        {
            //si lo grabo
            documentForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/document");
        }
    }
}

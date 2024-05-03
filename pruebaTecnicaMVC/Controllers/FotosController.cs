
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pruebaTecnicaMVC.Models;
using System.Net.Cache;

namespace pruebaTecnicaMVC.Controllers
{
    public class FotosController : Controller
    {
        private const string ApiUrl = "https://jsonplaceholder.typicode.com/photos";
        public async Task<ActionResult> Index(ModeloIndex modelo = null)
        {
            try
            {
                if (modelo == null)
                {
                    modelo = new ModeloIndex();
                    modelo.pagina = 1;
                    modelo.tamaño = 3;
                }
                else
                {
                    modelo.pagina = modelo.pagina ?? 1;
                    modelo.tamaño = modelo.tamaño ?? 3;
                }

                int numeroPagina = (int)modelo.pagina;
                int tamañoPagina = (int)modelo.tamaño;

                var Fotos = await HttpRequest(modelo);

                // Calculando el número total de páginas
                ViewBag.paginasTotal = (int)Math.Ceiling((double)Fotos.TotalCount / tamañoPagina);
                ViewBag.paginaActual = numeroPagina;
                ViewBag.tamañoPagina = tamañoPagina;
                return View(Fotos.Fotos);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<ModeloIndex> HttpRequest(ModeloIndex modelo)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{ApiUrl}?_pagina={modelo.pagina}&_limit={modelo.tamaño}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var Fotos = JsonConvert.DeserializeObject<Foto[]>(apiResponse);
                    modelo.Fotos = Fotos;

                    // Obteniendo el valor del encabezado "X-Total-Count" como una cadena
                    string totalCountHeader = response.Headers.GetValues("X-Total-Count").FirstOrDefault();
                    modelo.TotalCount = string.IsNullOrEmpty(totalCountHeader) ? 0 : int.Parse(totalCountHeader);

                    return modelo;
                }
            }
        }

    }
}

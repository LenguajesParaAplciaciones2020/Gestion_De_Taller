﻿using GestorDeTaller.BL;
using GestorDeTaller.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GestorDeTaller.Controllers
{
    [Authorize]
    public class CatalogoDeArticulosController : Controller
    {

      

        public CatalogoDeArticulosController()
        {

        }
        public async Task<IActionResult> ListarCatalogoDeArticulos()
        {

            List<Articulo> laLista = new List<Articulo>();

            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync("https://localhost:5001/api/CatalogoDeArticulos");

                string apiResponse = await response.Content.ReadAsStringAsync();

                laLista = JsonConvert.DeserializeObject<List<Articulo>>(apiResponse);
            }
            catch (Exception)
            {

                throw;
            }
            return View(laLista);

        }

       
        public async Task<IActionResult> ListarDetallesDelRepuesto(int id)
        {
            List<Repuesto> repuestoasociado = new List<Repuesto>();
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync("https://localhost:5001/api/CatalogoDeArticulos/ListarDetallesDelRepuesto/" + id.ToString());

                string apiResponse = await response.Content.ReadAsStringAsync();

                repuestoasociado = JsonConvert.DeserializeObject<List<Repuesto>>(apiResponse);
            }
            catch (Exception)
            {

                throw;
            }
            return View(repuestoasociado);
        }

        // GET: CatalogoDeArticulos/Details/5

        public async Task<IActionResult> DetallesDelArticulo(int id)
        {
            Articulo articulo;

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:5001/api/CatalogoDeArticulos/Detalles/" + id.ToString());
                string apiResponse = await response.Content.ReadAsStringAsync();
                articulo = JsonConvert.DeserializeObject<Articulo>(apiResponse);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            ViewData["Repuesto"] = articulo.repuestoasociado;
            ViewBag.OrdenesEnProceso = articulo.CantidadDeOrdenesEnProceso;
            ViewBag.OrdenesTerminadas = articulo.CantidadDeOrdenesTerminadas;

            return View(articulo);
        }

        // GET: CatalogoDeArticulos/Create
        public ActionResult AgregarArticulo()
        {
            return View();
        }

        // POST: CatalogoDeArticulos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarArticulo(Articulo articulo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var httpClient = new HttpClient();

                    string json = JsonConvert.SerializeObject(articulo);

                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);

                    var byteContent = new ByteArrayContent(buffer);

                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    await httpClient.PostAsync("https://localhost:5001/api/CatalogoDeArticulos", byteContent);

                    return RedirectToAction(nameof(ListarCatalogoDeArticulos));
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {

                return View();
            }

        }

        // GET: CatalogoDeArticulos/Edit/5
        public async Task<ActionResult> EditarCatalogoDeArticulos(int id)
        {
            Articulo ListarArticuloAEditar;
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:5001/api/CatalogoDeArticulos/" + id.ToString());
                string apiResponse = await response.Content.ReadAsStringAsync();
                ListarArticuloAEditar = JsonConvert.DeserializeObject<Articulo>(apiResponse);
            }
            catch (Exception e)
            {
                throw e;
            }
            return View(ListarArticuloAEditar);
        }

        // POST: CatalogoDeArticulos/Edit/5 //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarCatalogoDeArticulos(Articulo articulo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var httpClient = new HttpClient();

                    string json = JsonConvert.SerializeObject(articulo);

                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);

                    var byteContent = new ByteArrayContent(buffer);

                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    await httpClient.PutAsync("https://localhost:5001/api/CatalogoDeArticulos", byteContent);

                    return RedirectToAction(nameof(ListarCatalogoDeArticulos));
                }
                else
                {
                    return View(articulo);
                }
            }
            catch (Exception)
            {

                return View(articulo);
            }
        }

    }
}
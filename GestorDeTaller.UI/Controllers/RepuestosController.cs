﻿using GestorDeTaller.BL;
using GestorDeTaller.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace GestorDeTaller.UI.Controllers
{
    [Authorize]
    public class RepuestosController : Controller
    {

        

        public RepuestosController()
        {
            
        }

        public async Task<IActionResult> ListarRepuestosAsociados(int id)
        {
            List<Repuesto> repuestoasociado;
            TempData["IdArticulo"] = id;
            

            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync("https://localhost:5001/api/Repuestos/" + id.ToString() );

                string apiResponse = await response.Content.ReadAsStringAsync();

                repuestoasociado = JsonConvert.DeserializeObject<List<Repuesto>>(apiResponse);
            }
            catch (Exception)
            {

                throw;
            }
            return View(repuestoasociado);
            


            
        }
        public async Task<IActionResult> ListarRepuestosAsociadosAMantenimiento(int id)
        {
            List<Repuesto> repuestoasociado;
            TempData["IdArticulo"] = id;
            try
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync("https://localhost:5001/api/RepuestoParaMantenimiento/ListarRepuestosAsociadosAMantenimiento/" + id.ToString());

                string apiResponse = await response.Content.ReadAsStringAsync();

                repuestoasociado = JsonConvert.DeserializeObject<List<Repuesto>>(apiResponse);
            }
            catch (Exception)
            {

                throw;
            }
            
            
            ViewBag.Id = id;
            return View(repuestoasociado);
        }

        // GET: Repuestos/Details/5
        public async Task<IActionResult>  DetalleDeRepuesto(int id)
        {

            Repuesto repuesto;
            

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:5001/api/Repuestos/DetalleDeRepuesto/" + id.ToString());
                string apiResponse = await response.Content.ReadAsStringAsync();
                repuesto = JsonConvert.DeserializeObject<Repuesto>(apiResponse);
            }
            catch (Exception e)
            {
                throw e;
            }

            ViewData["Articulo"] = repuesto.articuloAsociado;
           ViewData["Mantenimiento"] = repuesto.MantenimientoAsosiado;
            ViewData["CantidadDeRepuestos"] = repuesto.dettallesRepuesto;

            
            return View(repuesto);
        }
       
        /*public ActionResult ListarMantenimientosAsociadosAlRepuesto(int id)
        {
            List<Mantenimiento> Mantenimientoasociado;
            Mantenimientoasociado = Repositorio.ObtenerMantenimientoAsociadoAlRepuesto(id);
            TempData["IdArticulo"] = id;

            return View(Mantenimientoasociado);
        }*/
        // GET: Repuestos/Create
        public IActionResult AgregarRepuesto()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarRepuesto(Repuesto repuesto)
        {
            try
            {
                int idArticulo = int.Parse(TempData["IdArticulo"].ToString());
                repuesto.Id_Articulo = idArticulo;

                if (ModelState.IsValid)
                {
                    
                    var httpClient = new HttpClient();

                    string json = JsonConvert.SerializeObject(repuesto);

                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);

                    var byteContent = new ByteArrayContent(buffer);

                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    await httpClient.PostAsync("https://localhost:5001/api/Repuestos", byteContent);

                    return RedirectToAction("ListarRepuestosAsociados", new { id = idArticulo });




                }
                else
                {
                    return View(repuesto);
                }

            }
            catch (Exception)
            {

                return View();
            }
        }

        // GET: Repuestos/Edit/5

        public async Task<IActionResult> EditarRepuesto(int id)
        {
            Repuesto editarRepuesto;
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://localhost:5001/api/Repuestos/EditarRepuesto/" + id.ToString());
                string apiResponse = await response.Content.ReadAsStringAsync();
                editarRepuesto = JsonConvert.DeserializeObject<Repuesto>(apiResponse);
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return View(editarRepuesto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarRepuesto(Repuesto repuesto)
        {
            try
            {
                int idArticulo = int.Parse(TempData["IdArticulo"].ToString());
                if (ModelState.IsValid)
                {
                    var httpClient = new HttpClient();

                    string json = JsonConvert.SerializeObject(repuesto);

                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);

                    var byteContent = new ByteArrayContent(buffer);

                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    await httpClient.PutAsync("https://localhost:5001/api/Repuestos/", byteContent);

                    return RedirectToAction("ListarRepuestosAsociados", new { id = idArticulo });
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



    }
}
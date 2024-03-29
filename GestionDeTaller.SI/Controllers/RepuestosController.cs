﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestorDeTaller.BL;
using GestorDeTaller.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestionDeTaller.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepuestosController : ControllerBase
    {
        private readonly IRepositorioDeTaller Repositorio;
        public RepuestosController(IRepositorioDeTaller repositorio)
        {
            Repositorio = repositorio;
        }
        // GET: api/<RepuestosController>
        [HttpGet("{id}")]
        public IEnumerable<Repuesto> Get(int id)
        {
            List<Repuesto> repuestoasociado;
            repuestoasociado = Repositorio.ObtenerRepuestoAsociadosAlArticulo(id);
            return repuestoasociado;
            
        }
        [HttpGet("{accion}/{id}")]
        public ActionResult<Repuesto> Get(string accion, int id)
        {
            if (accion.Equals("EditarRepuesto"))
            {
                Repuesto editarRepuesto;
                editarRepuesto = Repositorio.ObtenerRepuestoPorId(id);
                return editarRepuesto;
            }
            if (accion.Equals("DetalleDeRepuesto"))
            {
                Repuesto repuesto;
                repuesto = Repositorio.ObtenerRepuestoAdesasociar(id);

                List<Articulo> articuloAsociado;
                articuloAsociado = Repositorio.ObtenerArticuloAsociadosAlRepuesto(id);
                repuesto.articuloAsociado = articuloAsociado;
                List<Mantenimiento> MantenimientoAsosiado;
                MantenimientoAsosiado = Repositorio.ObtenerMantenimientoAsociadoAlRepuesto(id);
                repuesto.MantenimientoAsosiado = MantenimientoAsosiado;
                List<int> detallesRepuesto;
               
                detallesRepuesto = Repositorio.ObtenerDetalleRepuesto(id);
                repuesto.dettallesRepuesto=detallesRepuesto;

               
                return repuesto;
            }
            
            else
            {
                return null;
            }
        }

        // PUT api/<RepuestosController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Repuesto repuesto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Repositorio.EditarRepuesto(repuesto);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok(repuesto);

        }

        // POST api/<RepuestosController>
        [HttpPost]
        public IActionResult Post([FromBody] Repuesto repuesto)
        {
            try
            {


                if (ModelState.IsValid)
                {

                    Repositorio.AgregarRepuesto(repuesto, repuesto.Id_Articulo);


                }


            }
            catch (Exception ex)
            {

                return NotFound();
            }
            return Ok(repuesto);
        }

       

      
    }
}

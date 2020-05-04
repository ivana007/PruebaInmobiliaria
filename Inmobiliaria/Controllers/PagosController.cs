using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioPago repositorioPago;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioInmueble  repositorioInmueble;


        public PagosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioPago = new RepositorioPago(configuration);
            repositorioInquilino = new RepositorioInquilino(configuration);
            repositorioContrato = new RepositorioContrato(configuration);
            repositorioInmueble = new RepositorioInmueble(configuration);
        }
        // GET: Pagos
        public ActionResult Index()
        {
            try
            {
                var lista = repositorioPago.ObtenerTodos();
                return View(lista);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }

        }

        public ActionResult BuscarInquilinoModal() 
        {
            return View("_BuscarInquilino",new Inquilino());
        }

       
        // GET: Pagos/Details/5
        public ActionResult Details(int id)
        {
            var i = repositorioPago.ObtenerPorId(id);
            return View(i);
        }
        public ActionResult BuscarContrato()
        {
            List<Contrato> contrato = new List<Contrato>(); 
            return View(contrato);
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarContrato(BusquedaContrato busqueda)
        {
            try
            {
                // TODO: Add insert logic here


                var res = repositorioContrato.ObtenerPorDni(busqueda.Dni);

                return View(res);
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: Pagos/Create
        public ActionResult CrearPago(int id)
        {
            var contrato = repositorioContrato.ObtenerPorId(id);
            var inmueble = repositorioInmueble.ObtenerPorId(contrato.IdInmueble);
            var cantidadPagos = repositorioPago.ObtenerCantidadPagos(contrato.IdContrato);
            ViewData["importe"] = inmueble.Precio;
            ViewData["IdContrato"] = contrato.IdContrato;
            ViewData["NumeroPago"] = cantidadPagos + 1 ;
            return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearPago(Pago p)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioPago.Alta(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago p)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioPago.Alta(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repositorioPago.ObtenerPorId(id);
            return View(p);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago p)
        {
            try
            {
                // TODO: Add update logic here
                p.IdPago = id;
                int res = repositorioPago.Modificacion(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Policy = "Administrador")]
        // GET: Pagos/Delete/5
        public ActionResult Delete(int id)
        {
            var lista = repositorioPago.ObtenerPorId(id); 
            return View(lista);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
                // TODO: Add delete logic here
                int res = repositorioPago.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
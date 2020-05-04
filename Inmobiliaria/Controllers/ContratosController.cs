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
    public class ContratosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioPago repositorioPago;
        private readonly RepositorioGarante repositorioGarante;
        private readonly RepositorioInquilino repositorioInquilino;




        public ContratosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioInmueble = new RepositorioInmueble(configuration);
            repositorioContrato = new RepositorioContrato(configuration);
            repositorioPago = new RepositorioPago(configuration);
            repositorioGarante = new RepositorioGarante(configuration);
            repositorioInquilino = new RepositorioInquilino(configuration);
        }
        // GET: Contratos
        public ActionResult Index()
        {
            var lista = repositorioContrato.ObtenerTodos();
            return View(lista);
        }

        // GET: Contratos/Details/5
        public ActionResult Details(int id)
        {
            var c = repositorioContrato.ObtenerPorId(id);
            ViewBag.Garante = c.garante.Nombre + "" + c.garante.Apellido; 
            return View(c);
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
           // ViewBag.Pagos = repositorioPago.ObtenerTodos();//podria mostrar los pagos de los ultimos 2 dias o del dia actual
            ViewBag.Garantes = repositorioGarante.ObtenerTodos();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioContrato.Alta(c);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: Contratos/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            ViewBag.Garantes = repositorioGarante.ObtenerTodos();
            var c = repositorioContrato.ObtenerPorId(id);
            return View(c);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato c)
        {
            try
            {
                // TODO: Add update logic here
                c.IdContrato = id;
                int res = repositorioContrato.Modificacion(c);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var c = repositorioContrato.ObtenerPorId(id);
            return View(c);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato c)
        {
            try
            {
                // TODO: Add delete logic here
                c.IdContrato = id;
                int res = repositorioContrato.BajaLogica(c);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
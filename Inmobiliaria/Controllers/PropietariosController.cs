using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioPropietario repositorioPropietario;

        public PropietariosController(IConfiguration configuration)
        {

            this.configuration = configuration;
            repositorioPropietario = new RepositorioPropietario(configuration);
        }
        // GET: Propietarios
        public ActionResult Index()
        {
            var lista = repositorioPropietario.ObtenerTodos();
            return View(lista);
        }

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {
            var p = repositorioPropietario.ObtenerPorId(id);
            return View(p);
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                p.Condicion = "1";
                int res = repositorioPropietario.Alta(p);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {
           
           var p = repositorioPropietario.ObtenerPorId(id);
            return View(p);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = null;
            try
             {
                p = repositorioPropietario.ObtenerPorId(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Telefono = collection["Telefono"];
                p.Mail = collection["Mail"];
                repositorioPropietario.Modificacion(p);
                
                return RedirectToAction(nameof(Index));

                // return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }

        // GET: Propietarios/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorioPropietario.ObtenerPorId(id);
           
            return View(p);
           // return View();
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)//(int id, IFormCollection collection)
        {
            Propietario p = null;
            try
            {
                 p = repositorioPropietario.ObtenerPorId(id);
                // TODO: Add delete logic here
                //repositorioPropietario.Baja(id);
                p.Condicion = "0";
                repositorioPropietario.BajaLogica(p);
                //TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
                //return View();
            }
        }
    }
}
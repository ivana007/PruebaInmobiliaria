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
    public class InmueblesController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioPropietario repositorioPropietario;
        

        public InmueblesController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioInmueble = new RepositorioInmueble(configuration);
            repositorioPropietario = new RepositorioPropietario(configuration);
        }


        // GET: Inmuebles
        public ActionResult Index()
        {
            try
            {
                var lista = repositorioInmueble.ObtenerTodos();
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
                throw;
            }
            
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);
            return View(i);
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
                throw;
            }
            
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                
                i.Estado = "Disponible";
                int res = repositorioInmueble.Alta(i);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
                throw;
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            var i = repositorioInmueble.ObtenerPorId(id);
            return View(i);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble i)
        {
            Inmueble inmueble = null;
            try
            {
                // TODO: Add update logic here
                inmueble = repositorioInmueble.ObtenerPorId(id);
                //i = repositorioInquilino.ObtenerPorId(id);
                
                inmueble.Direccion = i.Direccion;
                inmueble.Estado = i.Estado;
                inmueble.TipoInmueble = i.TipoInmueble;
                inmueble.Uso = i.Uso;
                inmueble.CantHambientes = i.CantHambientes;
                inmueble.Precio = i.Precio;
                inmueble.IdPropietario = i.IdPropietario;
                


                int res = repositorioInmueble.Modificacion(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {

                return View();
            }
        }

        // GET: Inmuebles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
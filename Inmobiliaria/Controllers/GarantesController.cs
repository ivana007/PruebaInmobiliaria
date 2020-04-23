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
    public class GarantesController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioGarante repositorioGarante;

        public GarantesController(IConfiguration configuration)
        {

            this.configuration = configuration;
            repositorioGarante = new RepositorioGarante(configuration);
        }
        // GET: Garantes
        public ActionResult Index()
        {
            var lista = repositorioGarante.ObtenerTodos();
            return View(lista);
        }

        // GET: Garantes/Details/5
        public ActionResult Details(int id)
        {
            var g = repositorioGarante.ObtenerPorId(id);
            return View(g);
        }

        // GET: Garantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Garantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Garante g)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioGarante.Alta(g);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Garantes/Edit/5
        public ActionResult Edit(int id)
        {
            var g = repositorioGarante.ObtenerPorId(id);
            return View(g);
        }

        // POST: Garantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Garante g)
        {
            try
            {
                // TODO: Add update logic here
                g.IdGarante = id;
                int res = repositorioGarante.Modificacion(g);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Garantes/Delete/5
        public ActionResult Delete(int id)
        {
            var g = repositorioGarante.ObtenerPorId(id);
            return View(g);
        }

        // POST: Garantes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Garante g)
        {
            try 
            {
                // TODO: Add delete logic here
                
                int r=repositorioGarante.Baja(id);
                 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
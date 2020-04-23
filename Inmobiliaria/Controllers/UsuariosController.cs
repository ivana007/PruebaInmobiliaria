using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    
    public class UsuariosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioUsuario repositorioUsuario;


        public UsuariosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioUsuario = new RepositorioUsuario(configuration);
            
        }




        // GET: Usuarios
        public ActionResult Index()
        {
            var lista = repositorioUsuario.ObtenerTodos();
            return View(lista);
        }
        

        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            var u = repositorioUsuario.ObtenerPorId(id);
            return View(u);
        }

        [AllowAnonymous]
        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario u)
        {
            try
            {
                // TODO: Add insert logic here
                u.Estado = "1";
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: u.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                u.Clave = hashed;
                int res = repositorioUsuario.Alta(u);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            var u = repositorioUsuario.ObtenerPorId(id);
            return View(u);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario u)
        {
            try
            {
                // TODO: Add update logic here
                u.IdUsuario = id;
                int res=repositorioUsuario.Modificacion(u);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy ="Administrador")]
        public ActionResult Delete(int id)
        {
            var usuario = repositorioUsuario.ObtenerPorId(id);
            
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                // TODO: Add delete logic here
                usuario = repositorioUsuario.ObtenerPorId(id);
                usuario.Estado = "0";
                int res = repositorioUsuario.BajaLogica(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login()
        {

            return View();


        }

        // POST: Home/Login
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginView login)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: login.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                    var e = repositorioUsuario.ObtenerPorEmail(login.Mail);

                    if (e == null || e.Clave != hashed)
                    {
                        //ViewBag.Mensaje = "Mail o Contraseña Incorrectos";
                        ModelState.AddModelError("", "El mail o la clave son incorrectos");
                        return View();
                    }
                    var claim = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, e.Mail),
                    new Claim("FullName", e.Nombre + " " + e.Apellido),

                    new Claim(ClaimTypes.Role, e.Rol),
                     };

                    var claimsIdentity = new ClaimsIdentity(
                    claim, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }


        // GET: Home/Login
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
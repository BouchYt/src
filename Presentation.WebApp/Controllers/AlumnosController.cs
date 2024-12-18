using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO; // Add this for MemoryStream
using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers;

public class AlumnosController : Controller
{
      private readonly AlumnosDbContext _alumnosDbContext;
      public AlumnosController(IConfiguration configuration)
      {
            _alumnosDbContext = new AlumnosDbContext(configuration.GetConnectionString("DefaultConnection"));
      }

      public IActionResult Index()
      {
            var data = _alumnosDbContext.List();
            return View(data);
      }

      public IActionResult Details(Guid id)
      {
            var data = _alumnosDbContext.Details(id);
            return PartialView(data);
      }

      public IActionResult Create()
      {
            return View();
      }
      [HttpPost]
      public IActionResult Create(Alumno data, IFormFile foto)
      {
            if (foto != null && foto.Length > 0)
    {
        using (var memoryStream = new MemoryStream())
        {
            foto.CopyTo(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            data.Foto = Convert.ToBase64String(imageBytes); // Codificar a Base64
        }
    }

            data.Id = Guid.NewGuid();
            _alumnosDbContext.Create(data);
            return RedirectToAction("Index");
      }

      public IActionResult Edit(Guid id)
      {
            var data = _alumnosDbContext.Details(id);
            return View(data);
      }
      [HttpPost]
      public IActionResult Edit(Alumno data, IFormFile foto)
      {
            if (foto != null && foto.Length > 0)
    {
        using (var memoryStream = new MemoryStream())
        {
            foto.CopyTo(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            data.Foto = Convert.ToBase64String(imageBytes); // Codificar a Base64
        }
    }
    // Si no se sube una nueva foto, mantener la actual
    else 
    {
        var alumnoActual = _alumnosDbContext.Details(data.Id); // Obtener el alumno de la BD
        data.Foto = alumnoActual.Foto; // Asignar la foto actual al modelo
    }

            _alumnosDbContext.Edit(data);
            return RedirectToAction("Index");
      }

      public IActionResult Delete(Guid id)
      {
            _alumnosDbContext.Delete(id);
            return RedirectToAction("Index");
      }
}
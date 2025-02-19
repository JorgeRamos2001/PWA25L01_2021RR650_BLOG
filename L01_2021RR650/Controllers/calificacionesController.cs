using L01_2021RR650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2021RR650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class calificacionesController : ControllerBase
    {
        private readonly BlogDBContext _contexto;

        public calificacionesController(BlogDBContext contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        [Route("/AgregarCalificacion")]
        public IActionResult AgregarCalificacion([FromBody] calificaciones calificacion)
        {
            try
            {
                _contexto.calificaciones.Add(calificacion);
                _contexto.SaveChanges();
                return Ok(calificacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/Calificaciones")]
        public IActionResult Calificaciones()
        {
            List<calificaciones> calificaciones = (from c in _contexto.calificaciones select c).ToList();

            return Ok(calificaciones);
        }

        [HttpGet]
        [Route("/CalificacionesPorPublicacion")]
        public IActionResult CalificacionesPorPublicacion(int publicacionId)
        {
            publicaciones? publicacion = (from p in _contexto.publicaciones where p.publicacionId == publicacionId select p).FirstOrDefault();

            if (publicacion == null)
            {
                return NotFound($"La publicacion con el id: {publicacionId} no fue encontrada.");
            }

            List<calificaciones> calificaciones = (from c in _contexto.calificaciones where c.publicacionId == publicacionId select c).ToList();

            return Ok(calificaciones);
        }

        [HttpGet]
        [Route("/CalificacionPorId")]
        public IActionResult CalificacionPorId(int calificacionId)
        {
            calificaciones? calificacion = (from c in _contexto.calificaciones where c.calificacionId ==  calificacionId select c).FirstOrDefault();

            if (calificacion == null)
            {
                return NotFound($"La calificacion con el id: {calificacionId} no fue encontrada.");
            }

            return Ok(calificacion);
        }

        [HttpPut]
        [Route("/EditarCalificacion")]
        public IActionResult EditarCalificacion(int calificacionId, [FromBody]calificaciones calificacionEditada)
        {
            calificaciones? calificacion = (from c in _contexto.calificaciones where c.calificacionId == calificacionId select c).FirstOrDefault();

            if (calificacion == null)
            {
                return NotFound($"La calificacion con el id: {calificacionId} no fue encontrada.");
            }

            calificacion.publicacionId = calificacionEditada.publicacionId;
            calificacion.usuarioId = calificacionEditada.usuarioId;
            calificacion.calificacion = calificacionEditada.calificacion;

            _contexto.calificaciones.Entry(calificacion).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(calificacion);
        }

        [HttpDelete]
        [Route("/EliminarCalificacion")]
        public IActionResult EliminarCalificacion(int calificacionId)
        {
            calificaciones? calificacion = (from c in _contexto.calificaciones where c.calificacionId == calificacionId select c).FirstOrDefault();

            if (calificacion == null)
            {
                return NotFound($"La calificacion con el id: {calificacionId} no fue encontrada.");
            }

            _contexto.calificaciones.Attach(calificacion);
            _contexto.calificaciones.Remove(calificacion);
            _contexto.SaveChanges();

            return Ok(calificacion);
        }
    }
}

using L01_2021RR650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2021RR650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class comentariosController : ControllerBase
    {
        private readonly BlogDBContext _contexto;

        public comentariosController(BlogDBContext contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        [Route("/AgregarComentario")]
        public IActionResult AgregarComentario([FromBody]comentarios comentario)
        {
            try
            {
                _contexto.comentarios.Add(comentario);
                _contexto.SaveChanges();
                return Ok(comentario);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/Comentarios")]
        public IActionResult Comentarios()
        {
            List<comentarios> comentarios = (from c in _contexto.comentarios select c).ToList();

            return Ok(comentarios);
        }

        [HttpGet]
        [Route("/ComentariosPorPublicacion")]
        public IActionResult ComentariosPorPublicacion(int publicacionId)
        {
            publicaciones? publicacion = (from p in _contexto.publicaciones where p.publicacionId == publicacionId select p).FirstOrDefault();

            if (publicacion == null)
            {
                return NotFound($"La publicacion con el id: {publicacionId} no fue encontrada.");
            }

            List<comentarios> comentarios = (from c in _contexto.comentarios where c.publicacionId == publicacionId select c).ToList();

            return Ok(comentarios);
        }

        [HttpGet]
        [Route("/ComentariosPorUsuario")]
        public IActionResult ComentariosPorUsuario(int usuarioId)
        {
            usuarios? usuario = (from u in _contexto.usuarios where u.usuarioId == usuarioId select u).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"El usuario con el id: {usuarioId} no fue encontrado.");
            }


            var comentarios = (from u in _contexto.usuarios
                               join r in _contexto.roles
                               on u.rolId equals r.rolId
                               join c in _contexto.comentarios
                               on u.usuarioId equals c.usuarioId into comentariosUsuario
                               where u.usuarioId == usuarioId
                               select new
                               {
                                   u.usuarioId,
                                   u.rolId,
                                   r.rol,
                                   u.nombreUsuario,
                                   u.clave,
                                   u.nombre,
                                   u.apellido,
                                   Comentarios = comentariosUsuario.Select(c => c).ToList()
                               }).FirstOrDefault();

            return Ok(comentarios);
        }

        [HttpPut]
        [Route("/EditarComentario")]
        public IActionResult EditarComentario(int comentarioId, [FromBody]comentarios comentarioEditado)
        {
            comentarios? comentario = (from c in _contexto.comentarios where c.comentarioId == comentarioId select c).FirstOrDefault();

            if (comentario == null)
            {
                return NotFound($"El comentario con el id: {comentarioId} no fue encontrado.");
            }

            comentario.publicacionId = comentarioEditado.publicacionId;
            comentario.usuarioId = comentarioEditado.usuarioId;
            comentario.comentario = comentarioEditado.comentario;

            _contexto.comentarios.Entry(comentario).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(comentario);
        }

        [HttpDelete]
        [Route("/EliminarComentario")]
        public IActionResult EliminarComentario(int comentarioId)
        {
            comentarios? comentario = (from c in _contexto.comentarios where c.comentarioId == comentarioId select c).FirstOrDefault();

            if (comentario == null)
            {
                return NotFound($"El comentario con el id: {comentarioId} no fue encontrado.");
            }


            _contexto.comentarios.Attach(comentario);
            _contexto.comentarios.Remove(comentario);
            _contexto.SaveChanges();

            return Ok(comentario);
        }
    }
}

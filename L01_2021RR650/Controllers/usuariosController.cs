using L01_2021RR650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2021RR650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly BlogDBContext _contexto;

        public usuariosController(BlogDBContext contexto)
        {
            _contexto = contexto;
        }


        [HttpPost]
        [Route("/CrearUsuario")]
        public IActionResult CrearUsuario([FromBody]usuarios usuario)
        {
            try
            {
                _contexto.usuarios.Add(usuario);
                _contexto.SaveChanges();
                return Ok(usuario);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/Usuarios")]
        public IActionResult Usuarios()
        {
            List<usuarios> usuarios = (from u in _contexto.usuarios select u).ToList();

            return Ok(usuarios);
        }

        [HttpGet]
        [Route("/UsuarioPorId")]
        public IActionResult UsuarioPorId(int usuarioId)
        {
            usuarios? usuario = (from u in _contexto.usuarios where u.usuarioId == usuarioId select u).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"El usuario con el id: {usuarioId} no fue encontrado.");
            }

            return Ok(usuario);
        }

        [HttpGet]
        [Route("/UsuariosPorNombre")]
        public IActionResult UsuariosPorNombre(string nombre)
        {
            List<usuarios> usuarios = (from u in _contexto.usuarios where (u.nombre + " " + u.apellido).Contains(nombre) select u).ToList();

            return Ok(usuarios);
        }

        [HttpGet]
        [Route("/UsuariosPorRol")]
        public IActionResult UsuariosPorRol(int rolId)
        {
            roles? rol = (from r in _contexto.roles where r.rolId == rolId select r).FirstOrDefault();

            if (rol == null)
            {
                return NotFound($"El rol con el id: {rolId} no fue encontrado.");
            }

            List<usuarios> usuarios = (from u in _contexto.usuarios where u.rolId == rolId select u).ToList();

            return Ok(usuarios);
        }

        [HttpPut]
        [Route("/EditarUsuario")]
        public IActionResult EditarUsuario(int usuarioId, [FromBody]usuarios usuarioEditado)
        {
            usuarios? usuario = (from u in _contexto.usuarios where u.usuarioId == usuarioId select u).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"El usuario con el id: {usuarioId} no fue encontrado.");
            }

            usuario.rolId = usuarioEditado.rolId;
            usuario.nombreUsuario = usuarioEditado.nombreUsuario;
            usuario.clave = usuarioEditado.clave;
            usuario.nombre = usuarioEditado.nombre;
            usuario.apellido = usuarioEditado.apellido;

            _contexto.usuarios.Entry(usuario).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(usuario);
        }

        [HttpDelete]
        [Route("/EliminarUsuario")]
        public IActionResult EliminarUsuario(int usuarioId)
        {
            usuarios? usuario = (from u in _contexto.usuarios where u.usuarioId == usuarioId select u).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"El usuario con el id: {usuarioId} no fue encontrado.");
            }

            _contexto.usuarios.Attach(usuario);
            _contexto.usuarios.Remove(usuario);
            _contexto.SaveChanges();

            return Ok(usuario);
        }
    }
}

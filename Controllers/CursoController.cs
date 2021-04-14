using Api.Data.Collections;
using ApiCursos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;

namespace ApiCursos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Curso> _cursoCollection;

        public CursoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _cursoCollection = _mongoDB.DB.GetCollection<Curso>(typeof(Curso).Name.ToLower());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Curso))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetCurso()
        {
            var cursos = _cursoCollection.Find(Builders<Curso>.Filter.Empty).ToList();
            if(cursos != null)
            {
                return Ok(cursos);
            }

            return NotFound();
       }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult PostCurso(CursoModel model)
        {
            var Curso = new Curso(model.id, model.nome, model.descricao, model.idMaterias);
            _cursoCollection.InsertOne(Curso);
            return StatusCode(201, Curso.ToString());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DeleteCurso(int id)
        {
            _cursoCollection.DeleteOne(Builders<Curso>.Filter.Where(x => x.id == id));
            return Ok();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Curso))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult PatchCurso(int id, [FromBody] JsonPatchDocument<Curso> patchCurso)
        {
            if(patchCurso == null)
            {
                return BadRequest();
            }
            var filter = Builders<Curso>.Filter.Eq(x => x.id, id);
            var aAlterar = _cursoCollection.Find(filter).FirstOrDefault();
            
            if(aAlterar== null)
            {
                return NotFound();
            }

            patchCurso.ApplyTo(aAlterar);

            _cursoCollection.ReplaceOne(filter, aAlterar);
            return Ok(aAlterar);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Curso))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PutCurso(int id, CursoModel model)
        {
            if(model == null)
            {
                return BadRequest();
            }

            var Curso = new Curso(model.id, model.nome, model.descricao, model.idMaterias);
            var filter = Builders<Curso>.Filter.Eq(x => x.id, id);
            _cursoCollection.ReplaceOne(filter, Curso);

            return Ok(Curso);
        }
    }
}
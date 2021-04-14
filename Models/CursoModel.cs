using System;
using System.Collections.Generic; 

namespace ApiCursos.Models
{
    public class CursoModel
    {
        public int id;
        public string nome;
        public string descricao;
        public List<int> idMaterias;
    }
}
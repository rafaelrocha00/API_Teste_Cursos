using System;
using System.Collections.Generic;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Api.Data.Collections
{
    public class Curso
    {
        public int id { get; private set; }
        public string nome;
        public string descricao;
        public List<int> idMaterias;

        public Curso(int id, string nome, string descricao, List<int> idMaterias)
        {
            this.id = id;
            this.nome = nome;
            this.descricao = descricao;
            this.idMaterias = idMaterias;
        }

        
        
        
    }
}
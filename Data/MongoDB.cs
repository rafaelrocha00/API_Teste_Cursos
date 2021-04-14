using System;
using Api.Data.Collections;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace ApiCursos.Data
{
    public class MongoDB
    {
       public IMongoDatabase DB { get; }

       public MongoDB(IConfiguration configuration)
       {
           try
           {
             var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"]));
             var client = new MongoClient(settings);
             DB = client.GetDatabase(configuration["NomeBanco"]);
             MapClasses();
           }
           catch(Exception ex)
           {
               throw new MongoException("Não foi possivel se conectar com o banco de dados.", ex);
           }
       }

       void MapClasses()
       {
           var conventionPack = new ConventionPack{ new CamelCaseElementNameConvention()};
           ConventionRegistry.Register("camelCase", conventionPack, t => true);

           if(!BsonClassMap.IsClassMapRegistered(typeof(Curso)))
           {
               BsonClassMap.RegisterClassMap<Curso>(i =>
               {
                   i.AutoMap();
                   i.SetIgnoreExtraElements(true);
               });
           }
       }
    }
}
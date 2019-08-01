using AutoMapper;
using DocumentRepositoryApi.Models;
using DocumentEntity = DocumentRepositoryApi.DataAccess.Entities.Document;
namespace DocumentRepositoryApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Document, DocumentEntity>();
            CreateMap<DocumentEntity, Document>();
        }
    }
}

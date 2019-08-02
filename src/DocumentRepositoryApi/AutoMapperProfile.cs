using AutoMapper;
using DocumentRepositoryApi.Models;
using DocumentEntity = DocumentRepositoryApi.DataAccess.Entities.Document;
using UserEntity = DocumentRepositoryApi.DataAccess.Entities.User;
namespace DocumentRepositoryApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Document, DocumentEntity>();
            CreateMap<DocumentEntity, Document>();
            CreateMap<User, UserEntity>();
            CreateMap<UserEntity, User>();
        }
    }
}

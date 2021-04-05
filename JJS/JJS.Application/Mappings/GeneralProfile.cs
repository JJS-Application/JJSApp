using AutoMapper;

using JJS.Application.Features.Company.Commands.CreateCompany;
using JJS.Application.Features.Company.Queries.GetAllCompany;
using JJS.Application.Features.Products.Commands.CreateProduct;
using JJS.Application.Features.Products.Queries.GetAllProducts;
using JJS.Application.Features.Stream.Commands.CreateStream;
using JJS.Application.Features.Stream.Queries.GetAllStream;
using JJS.Domain.Entities;
using JJS.Domain.Entities.CompanyTables;

using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductQuery, GetAllProductParameter>();


            CreateMap<Organization, GetAllCompanyViewModel>().ReverseMap();
            CreateMap<CreateCompanyCommand, Organization>();
            CreateMap<GetAllCompanyQuery, GetAllCompanyParameter>();


            CreateMap<BusinessStream, GetAllStreamViewModel>().ReverseMap();
            CreateMap<CreateStreamCommand, BusinessStream>();
            CreateMap<GetAllStreamQuery, GetAllStreamViewModel>();
        }
    }
}

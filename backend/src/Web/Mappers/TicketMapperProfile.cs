﻿using AutoMapper;
using EventManagement.ApplicationCore.Models;
using System;

namespace EventManagement.WebApp.Mappers
{
    public class TicketMapperProfile : Profile
    {
        public TicketMapperProfile()
        {
            CreateMap<Ticket, Models.Ticket>()
                .ForMember(e => e.Creator, opt => opt.MapFrom(e => e.Creator.Name))
                .ForMember(e => e.Editor, opt => opt.MapFrom(e => e.Editor.Name))
                .ForMember(e => e.Gender, opt => opt.MapFrom(
                    e => e.Gender.HasValue ? e.Gender.Value.GetStringValue() : null))
                .ReverseMap()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.TicketSecret, opt => opt.Ignore())
                .ForMember(e => e.Creator, opt => opt.Ignore())
                .ForMember(e => e.Editor, opt => opt.Ignore())
                .ForMember(e => e.AmountPaid, opt => opt.MapFrom(
                    e => e.PaymentStatus == PaymentStatus.PaidPartial ? e.AmountPaid : null))
                .ForMember(e => e.Gender, opt => opt.MapFrom(
                    e => GenderExtensions.FromStringValue(e.Gender)))
                .ForMember(e => e.BirthDate, opt => opt.MapFrom(
                    e => e.BirthDate.HasValue ? (DateTime?)e.BirthDate.Value.ToLocalTime().Date : null));
        }
    }
}
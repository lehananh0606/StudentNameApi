﻿using Data.Models;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IBookingDetailRepository : IGenericRepository<BookingDetail>
    {
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NssRestClient.Dto;

namespace NssRestClient.Services
{
    public interface ISystemService
    {
        Task<RestResult<ApiUser>> GetLoggedInUser();
    }
}

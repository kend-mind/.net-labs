using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using System.Net;
using System.Linq;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.API.Common.Middlewares;
using VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Schema;
using VPBANK.RMD.Repositories.PhoenixData.Interfaces.Schema;
using VPBANK.RMD.Repositories.Collection.Interfaces.Schema;
using VPBANK.RMD.Repositories.IFRS9_Conf.Interfaces.Schema;
using VPBANK.RMD.Repositories.IFRS9_Data.Interfaces.Schema;

namespace VPBANK.RMD.API.IFRS9.Controllers.Common
{
    [Authorize]
    [Route(template: "/information-schema")]
    public class SchemaController : ControllerBase
    {
        private readonly IConfTableRepository _confTableRepository;
        private readonly IConfColumnRepository _confColumnRepository;

        private readonly IDataTableRepository _dataTableRepository;
        private readonly IDataColumnRepository _dataColumnRepository;

        private readonly IColnTableRepository _colnTableRepository;
        private readonly IColnColumnRepository _colnColumnRepository;

        private readonly IIfrs9ConfTableRepository _ifrsConfTableRepository;
        private readonly IIfrs9ConfColumnRepository _ifrsConfColumnRepository;

        private readonly IIfrs9DataTableRepository _ifrsDataTableRepository;
        private readonly IIfrs9DataColumnRepository _ifrsDataColumnRepository;

        public SchemaController(IConfTableRepository confTableRepository, IConfColumnRepository confColumnRepository,
            IDataTableRepository dataTableRepository, IDataColumnRepository dataColumnRepository,
            IColnTableRepository colnTableRepository, IColnColumnRepository colnColumnRepository,
            IIfrs9ConfTableRepository ifrsConfTableRepository, IIfrs9ConfColumnRepository ifrsConfColumnRepository,
            IIfrs9DataTableRepository ifrsDataTableRepository, IIfrs9DataColumnRepository ifrsDataColumnRepository)
        {
            _confTableRepository = confTableRepository;
            _confColumnRepository = confColumnRepository;

            _dataTableRepository = dataTableRepository;
            _dataColumnRepository = dataColumnRepository;

            _colnTableRepository = colnTableRepository;
            _colnColumnRepository = colnColumnRepository;

            _ifrsConfTableRepository = ifrsConfTableRepository;
            _ifrsConfColumnRepository = ifrsConfColumnRepository;

            _ifrsDataTableRepository = ifrsDataTableRepository;
            _ifrsDataColumnRepository = ifrsDataColumnRepository;
        }

        [HttpGet]
        [Route(template: "tables")]
        public virtual IActionResult Tables()
        {
            try
            {
                var confTables = _confTableRepository.FindAll();
                var dataTables = _dataTableRepository.FindAll();
                var colnTables = _colnTableRepository.FindAll();
                var ifrsConfTables = _ifrsConfTableRepository.FindAll();
                var ifrsDataTables = _ifrsDataTableRepository.FindAll();
                return Ok(confTables.Union(dataTables).Union(colnTables).Union(ifrsConfTables).Union(ifrsDataTables));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw;
                else
                    throw new HttpErrorException(HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
            }
        }

        [HttpGet]
        [Route(template: "columns")]
        public virtual IActionResult Columns()
        {
            try
            {
                var confColumns = _confColumnRepository.FindAll();
                var dataColumns = _dataColumnRepository.FindAll();
                var colnColumns = _colnColumnRepository.FindAll();
                var ifrsConfColumns = _ifrsConfColumnRepository.FindAll();
                var ifrsDataColumns = _ifrsDataColumnRepository.FindAll();
                return Ok(confColumns.Union(dataColumns).Union(colnColumns).Union(ifrsConfColumns).Union(ifrsDataColumns));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException?.Message);
                if (ex is HttpErrorException)
                    throw;
                else
                    throw new HttpErrorException(HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError), string.Format(ErrorMessages.SE000, ex.Message));
            }
            finally
            {
            }
        }
    }
}

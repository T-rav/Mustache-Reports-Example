﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Report.Server.Boundry.ReportRendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Presenters;

namespace Simple.Report.Server.Controllers.Web
{
    [Route("api/reporting")]
    public class ReportingController
    {
        private readonly IRenderReportUseCase _usecase;

        public ReportingController(IRenderReportUseCase usecase)
        {
            _usecase = usecase;
        }

        // todo : add create/pdf
        [ProducesResponseType(typeof(File), 200)]
        [Produces("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [ProducesResponseType(typeof(ErrorOutputMessage), 422)]
        [HttpGet("create/word")]
        public IActionResult Create()
        {
            var jsonData = File.ReadAllText("ReportRendering\\ExampleData\\WithImagesSampleData.json");

            var inputMessage = new RenderReportInputMessage
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport.docx",
                JsonModel = jsonData
            };
            var presenter = new DownloadFilePresenter();
            _usecase.Execute(inputMessage, presenter); // base64 string ok, just the swagger? Yep, download from url works fine.
            return presenter.Render();
        }

    }
}
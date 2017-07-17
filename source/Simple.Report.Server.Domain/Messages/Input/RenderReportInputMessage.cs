﻿namespace Simple.Report.Server.Domain.Messages.Input
{
    public class RenderReportInputMessage
    {
        public string JsonModel { get; set; }
        public string TemplateName { get; set; }
        public string ReportName { get; set; }

        public RenderReportInputMessage()
        {
            ReportName = "report.docx";
        }
    }
}
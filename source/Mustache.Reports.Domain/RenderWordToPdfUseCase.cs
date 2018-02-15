﻿using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Pdf;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain
{
    public class RenderWordToPdfUseCase : IRenderDocxToPdfUseCase
    {
        private readonly IPdfGateway _reportRepository;

        public RenderWordToPdfUseCase(IPdfGateway reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public void Execute(RenderPdfInput inputTo, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter)
        {
            var output = _reportRepository.ConvertToPdf(inputTo);

            if (RenderErrors(output))
            {
                RespondWithErrors(presenter, output);
            }

            RespondWithPdf(inputTo, presenter, output);
        }

        private void RespondWithPdf(RenderPdfInput inputTo, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter,
            RenderedDocummentOutput output)
        {
            presenter.Respond(new PdfFileOutput(inputTo.FileName, output.FetchDocumentAsByteArray()));
        }

        private void RespondWithErrors(IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter, RenderedDocummentOutput output)
        {
            var errorOutputMessage = new ErrorOutputMessage();
            errorOutputMessage.AddErrors(output.ErrorMessages);
            presenter.Respond(errorOutputMessage);
        }

        private bool RenderErrors(RenderedDocummentOutput output)
        {
            return output.HasErrors();
        }
    }
}